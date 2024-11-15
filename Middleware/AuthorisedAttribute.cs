using System.Security.Claims;
using BaseAuth.AppError;
using BaseAuth.Manager;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BaseAuth.Middleware;

public class AuthorisedAttribute(params string[] roles) : ActionFilterAttribute, IOperationFilter
{
    public AuthorisedAttribute() : this([])
    {
    }

    private List<string> GetRoles()
    {
        // Normalize roles to lowercase
        return roles.Select(r => r.ToLower()).ToList();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        try
        {
            var authorization = context.HttpContext.Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
            {
                throw new AppException(ErrorCode.UnAuthorized);
            }

            var token = authorization.Substring("Bearer ".Length, authorization.Length - "Bearer ".Length).Trim();
            if (string.IsNullOrEmpty(token) || !TokenManager.ValidateToken(token))
            {
                throw new AppException(ErrorCode.UnAuthorized);
            }

            if (roles.Length == 0)
            {
                base.OnActionExecuting(context);
                return;
            }

            // Get roles from token
            var claimList = TokenManager.Claims(token);
            if (claimList.FirstOrDefault(c => c.Key == "roles").Value is JArray claimRoles)
            {
                // Normalize claimed roles to lowercase
                var userRoles = claimRoles.Select(r => r.ToString().ToLower()).ToList();
                if (!userRoles.Intersect(GetRoles()).Any())
                {
                    throw new AppException(ErrorCode.Forbidden); 
                }

                // Create a new identity with the roles
                var claims = userRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
                context.HttpContext.User.AddIdentity(new ClaimsIdentity(claims, "Bearer"));
                // Todo: Add more claims if needed

                // Continue to the next middleware
                base.OnActionExecuting(context);
            }
            else
            {
                throw new AppException(ErrorCode.MissingRoles);
            }
        }
        catch (Exception e)
        {
            context.Result = ResponseWrappedAttribute.OnException(e, null);
        }
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // throw new NotImplementedException();
    }
}