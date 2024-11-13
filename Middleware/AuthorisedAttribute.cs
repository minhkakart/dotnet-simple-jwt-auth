using System.Security.Claims;
using BaseAuth.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BaseAuth.Middleware;

public class AuthorisedAttribute(params string[] roles) : ActionFilterAttribute, IOperationFilter
{
    // private readonly string[] roles = roles;
    
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
            var token = authorization.Substring("Bearer ".Length, authorization.Length - "Bearer ".Length).Trim();
            if (string.IsNullOrEmpty(token) && !TokenManager.ValidateToken(token))
            {
                context.Result = new UnauthorizedResult();
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
                    context.Result = new ObjectResult("Forbidden")
                    {
                        StatusCode = 403
                    };
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
                throw new Exception("Roles claim not found");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            context.Result = new ObjectResult("Internal server error")
            {
                StatusCode = 500
            };
        }
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // throw new NotImplementedException();
    }
}