using System.Reflection;
using BaseAuth.Middleware;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BaseAuth.Config;

public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.GetCustomAttribute<AuthorisedAttribute>() != null)
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    [
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }
                    ] = []
                }
            };
        }

        // operation.RequestBody ??= operation.RequestBody = new OpenApiRequestBody
        operation.RequestBody = operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>()
        };

        operation.RequestBody.Content.Add("mytype", new OpenApiMediaType
        {
            Schema = new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("my example")
            }
        });

        Console.Write(context.MethodInfo.Name + ": ");
        var parameters = context.MethodInfo.GetParameters();
        if (parameters.Length == 0)
        {
            Console.WriteLine("No parameters");
            return;
        }

        foreach (var parameterInfo in context.MethodInfo.GetParameters())
        {
            Console.Write(parameterInfo.Name + ": " + parameterInfo.GetModifiedParameterType().Name + ", "); 
        }

        Console.WriteLine();
    }
}