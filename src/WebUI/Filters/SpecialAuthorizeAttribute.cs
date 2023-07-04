using PolizaWebAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using PolizaWebAPI.Application.Common.Interfaces;
using MediatR;
using System;
using Microsoft.AspNetCore.Http;

namespace WebUI.Filters;

public class SpecialAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    
    public string Feature { get; set; } = string.Empty;

  
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {


        var jwt = context.HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(jwt))
        {
            SetUnauthorizedResult(context, "Missing Jwt Token");
            return;
        }

        //if (alcanosAuthService is null)
        //{
        //    SetUnauthorizedResult(context, "Invalid Jwt Token");
        //    return;
        //}

        //if (!(await alcanosAuthService?.ValidarTokenAsync(jwt, Feature)))
        //{
        //    SetUnauthorizedResult(context, "Invalid Jwt Token");
        //    return;
        //}

        var decodedToken = new JwtSecurityToken(jwt);

        var userIdClaim = decodedToken.Claims.FirstOrDefault(x => x.Type == "Identificacion");

        var userId = userIdClaim?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            SetUnauthorizedResult(context, "Missing UserId");
            return;
        }

        context.HttpContext.Items["UserId"] = userId;
    }

    private void SetUnauthorizedResult(AuthorizationFilterContext context, string errorTitle)
    {

        var logger = context.HttpContext.RequestServices.GetService(typeof(ILoggerManager)) as ILoggerManager;

        var transactionService = context.HttpContext.RequestServices.GetService(typeof(ICurrentTransactionService)) as ICurrentTransactionService;
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = errorTitle,
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
        };
        logger?.LogWarning(details, transactionService?.Transaction);
        context.Result = new UnauthorizedObjectResult(details);
    }
}