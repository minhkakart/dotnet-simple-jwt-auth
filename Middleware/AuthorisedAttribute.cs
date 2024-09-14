using BaseAuth.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BaseAuth.Middleware;

public class AuthorisedAttribute(params string[] roles) : ActionFilterAttribute, IOperationFilter
{
    public AuthorisedAttribute() : this([])
    {
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("List of roles: [" + string.Join(", ", roles) + "]");
        string token = context.HttpContext.Request.Headers["Authorization"];
        if (token == null)
        {
            context.Result = new UnauthorizedResult();
        }
        else
        {
            Console.WriteLine("Token: " + token);
        }
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // throw new NotImplementedException();
    }
}