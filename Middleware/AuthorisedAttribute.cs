using BaseAuth.Database;
using BaseAuth.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BaseAuth.Middleware;

public class AuthorisedAttribute(params string[] roles) : ActionFilterAttribute, IOperationFilter
{
    private string[] roles = roles;
    public AuthorisedAttribute() : this([])
    {
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("List of roles: [" + string.Join(", ", roles) + "]");
        try
        {
            var arr = context.HttpContext.Request.Headers["Authorization"].ToString().Split("Bearer ");
            var token = arr.Length > 1 ? arr[1] : "";
            if (!string.IsNullOrEmpty(token) && TokenManager.ValidateToken(token))
            {
                if (roles.Length > 0)
                {
                    var claimList = TokenManager.Claims(token);
            
                    var roles = claimList.FirstOrDefault(c => c.Key == "roles").Value as JArray;
                    var rolesList = roles != null ? roles.Select(r => r.ToString()).ToList() : new List<string>();
                    
                    if (!rolesList.Intersect(this.roles).Any())
                    {
                        context.Result = new ObjectResult("Forbidden")
                        {
                            StatusCode = 403
                        };
                    }
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
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