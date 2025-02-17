using System.Reflection;
using BaseAuth.Extension;
using BaseAuth.Middleware;
using BaseAuth.Util;
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

        var requestBodyAttribute = context.MethodInfo.GetCustomAttribute<RequestBodyAttribute>();
        if (requestBodyAttribute != null)
        {
            Console.WriteLine(new TypeMigrator().ConvertClassToTypeScript(requestBodyAttribute.Type));
            operation.RequestBody ??= operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>()
            };

            operation.RequestBody.Content.Add("typescript/interface", new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString(new TypeMigrator().ConvertClassToTypeScript(requestBodyAttribute.Type))
                }
            });
        }
        
        var responseBodyAttribute = context.MethodInfo.GetCustomAttribute<ResponseBodyAttribute>();
        if (responseBodyAttribute != null)
        {
            var responseWrapperType = responseBodyAttribute.Type;
            operation.Responses ??= new OpenApiResponses();
            operation.Responses["200"] ??= new OpenApiResponse
            {
                Description = "Success",
                Content = new Dictionary<string, OpenApiMediaType>()
            };
            
            operation.Responses["200"].Content.Add("typescript/interface", new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString(new TypeMigrator().ConvertClassToTypeScript(responseWrapperType))  
                }
            });
        }
    }
}