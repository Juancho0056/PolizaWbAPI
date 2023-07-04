using System.Reflection.Metadata;
using PolizaWebAPI.Application.Common.Interfaces;
using PolizaWebAPI.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using PolizaWebAPI.Infrastructure.Common;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Amazon.Runtime.Internal;
using PolizaWebAPI.Infrastructure.Identity;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

    

        services.AddScoped<IRequestGuid, RequestGuidService>();

        services.Configure<MongoConfiguration>(c => configuration.GetSection("MongoConfiguration").Bind(c));

        services.Configure<CacheConfiguration>(c => configuration.GetSection("CacheConfiguration").Bind(c));

        services.AddSingleton<IMongoDbService, MongoDbService>();


        MongoConfiguration mongoConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<MongoConfiguration>>().Value;



        services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddRoles<ApplicationRole>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(mongoConfiguration.ConnectionString, mongoConfiguration.DatabaseName);

        services.AddScoped<RoleManager<ApplicationRole>>();

        services.AddSingleton<IMemoryObjectCache, MemoryObjectCache>();

        services.AddScoped<ILoggerManager, LoggerManager>();

        services.AddTransient<IIdentityService, IdentityService>();

        services.AddScoped<IdentityInitialiser>();

        return services;
    }
}
