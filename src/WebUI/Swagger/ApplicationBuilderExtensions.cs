namespace WebUI.Swagger;

/// <summary>
/// 
/// </summary>
public static class ApplicationBuilderExtensions
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder ConfigureSwaggerUI(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "PolizaWebAPI Services v1");
        });
        return app;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder ConfigureCors(this IApplicationBuilder app)
    {
        app.UseCors("CorsPolicy");
        return app;
    }
}