using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Reflection;
using WebUI.Swagger.Filters;

namespace WebUI.Swagger;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureSwaggerDocument(this IServiceCollection services)
    {

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Documentacion swagger API",
                Description = "Documentacion swagger API",
             
            });
            c.OperationFilter<FileOperationFilter>();

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,

            });
            c.OperationFilter<AuthenticationRequirementsOperationFilter>();
            
            var xmlFileDocumentationName = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
            var xmlFileDocumentationPath = Path.Combine(AppContext.BaseDirectory, xmlFileDocumentationName);
            c.IncludeXmlComments(xmlFileDocumentationPath, true);

            
            //xmlFileDocumentationPath = Path.Combine(AppContext.BaseDirectory, "Infrastructure.xml");
            //c.IncludeXmlComments(xmlFileDocumentationPath, true);
            //xmlFileDocumentationPath = Path.Combine(AppContext.BaseDirectory, "Application.xml");
            //c.IncludeXmlComments(xmlFileDocumentationPath, true);
            //xmlFileDocumentationPath = Path.Combine(AppContext.BaseDirectory, "Domain.xml");
            c.IncludeXmlComments(xmlFileDocumentationPath, true);
            c.EnableAnnotations();
        });

        return services;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration config)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        return services;
    }
}