namespace Service.OpenApi;

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // This controls only action attribute not controller. If action has 'AllowAnonymousAttribute', no security requirement added.
        if (context.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any())
        {
            return;
        }

        operation.Security ??= new List<OpenApiSecurityRequirement>();

        var authorization = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Authorization" } };
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [authorization] = new List<string>(),
        });
    }
}
