using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebUI.Swagger.Filters;

/// <summary>
/// Filter property File Swagger
/// </summary>
public class FileOperationFilter : IOperationFilter
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var isMultipart = context.ApiDescription.ActionDescriptor.ActionConstraints
         .Select(r => r as ConsumesAttribute)
         .Any(r => r?.ContentTypes?.Any(p => p == "multipart/form-data") ?? false);

        if (isMultipart)
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["multipart/form-data"] = new OpenApiMediaType()
                    {
                        Encoding =
                        {
                            ["files"] = new OpenApiEncoding()
                            {
                                Style = ParameterStyle.Form
                            }
                        },
                        Schema = new OpenApiSchema()
                        {
                            Type = "object",
                            Properties =
                            {
                                ["files"] = new OpenApiSchema()
                                {
                                    Description = "Select file",
                                    Type = "array",
                                    Items = new OpenApiSchema()
                                    {
                                        Type="string",
                                        Format="binary",
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}