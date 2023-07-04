using System.Net;
using PolizaWebAPI.Application.Common.Exceptions;
using PolizaWebAPI.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PolizaWebAPI.Domain.Exceptions;

namespace PolizaWebAPI.WebUI.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext, ILoggerManager>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext, ILoggerManager>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            { typeof(UnsupportedPolizaException), UnsupportedPolizaException }
        };
    }
    public override void OnException(ExceptionContext context)
    {
        var logger = GetLogger(context.HttpContext);
        HandleException(context, logger);

        base.OnException(context);
    }
    private void HandleException(ExceptionContext context, ILoggerManager logger)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.TryGetValue(type, out var value))
        {
            value.Invoke(context, logger);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleValidationException(context, logger);
        }

        LogAndSetInternalServerError(context, logger);
    }
    private void LogAndSetInternalServerError(ExceptionContext context, ILoggerManager? logger)
    {
        var exception = context.Exception;
        logger.LogError(exception, "");

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        };

        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;
    }
    private void UnsupportedPolizaException(ExceptionContext context, ILoggerManager? logger)
    {
        var exception = (UnsupportedPolizaException)context.Exception;
        var details = CreateProblemDetails(exception, HttpStatusCode.BadRequest, "https://tools.ietf.org/html/rfc7231#section-6.5.1", exception.Message);
        logger.LogWarning(details, "");

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }
    private void HandleValidationException(ExceptionContext context, ILoggerManager? logger)
    {
        var exception = (ValidationException)context.Exception;
        var details = CreateProblemDetails(exception, HttpStatusCode.BadRequest, "https://tools.ietf.org/html/rfc7231#section-6.5.1", "Se encontraron uno o mas errores de validacion");
        logger.LogWarning(details, "");

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }
    private void HandleNotFoundException(ExceptionContext context, ILoggerManager logger)
    {
        var exception = (NotFoundException)context.Exception;
        var details = CreateProblemDetails(exception, HttpStatusCode.NotFound, "https://tools.ietf.org/html/rfc7231#section-6.5.4", "Recurso no encontrado");
        logger.LogWarning(details, "");

        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }
    private void HandleUnauthorizedAccessException(ExceptionContext context, ILoggerManager logger)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
        };
        logger.LogWarning(details, "");

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };
        context.ExceptionHandled = true;
    }

    private void HandleForbiddenAccessException(ExceptionContext context, ILoggerManager logger)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        };
        logger.LogWarning(details, "");

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };
        context.ExceptionHandled = true;
    }
    private ProblemDetails CreateProblemDetails(Exception exception, HttpStatusCode status, string type, string title)
    {
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = (int)status,
            Type = type
        };

        var validationException = exception as ValidationException;
        if (validationException?.Errors != null && validationException.Errors.Any())
        {
            problemDetails.Extensions["errors"] = validationException.Errors;
        }

        return problemDetails;
    }
    private ILoggerManager? GetLogger(HttpContext httpContext)
    {
        return httpContext.RequestServices.GetService(typeof(ILoggerManager)) as ILoggerManager;
    }
}