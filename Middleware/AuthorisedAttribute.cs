using System.Security.Claims;
using BaseAuth.Application;
using BaseAuth.Manager;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;

namespace BaseAuth.Middleware;

public class AuthorisedAttribute(params string[] roles) : ActionFilterAttribute
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

            // if (TokenManager.GetTokenType(token) == TokenType.Refresh)
            // {
            //     Console.WriteLine("Refresh token is not allowed");
            // }

            if (roles.Length == 0)
            {
                base.OnActionExecuting(context);
                return;
            }

            // Get roles from token
            var userInfo = TokenManager.ClaimUserInfo(token);
            if (userInfo.Roles.Count > 0)
            {
                // Normalize claimed roles to lowercase
                if (!userInfo.Roles.Select(r => r.ToString().ToLower()).ToList().Intersect(GetRoles()).Any())
                {
                    throw new AppException(ErrorCode.Forbidden); 
                }

                // Create a new identity with the roles
                var claims = userInfo.Roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
                var userInfoClaim = new Claim(ClaimTypes.UserData, JObject.FromObject(userInfo).ToString());
                claims.Add(userInfoClaim);
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
}