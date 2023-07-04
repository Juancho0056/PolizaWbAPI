using PolizaWebAPI.Application.Common.Interfaces;
using MediatR;

namespace PolizaWebAPI.Application.Common.Behaviours;
/// <summary>
/// Pipeline de comportamiento de registro.
/// </summary>
/// <typeparam name="TRequest">Tipo de solicitud.</typeparam>
/// <typeparam name="TResponse">Tipo de respuesta.</typeparam>
public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentTransactionService _currentTransactionService;
    private readonly ILoggerManager _logger;

    /// <summary>
    /// Constructor de la clase LoggingBehaviour.
    /// </summary>
    /// <param name="currentUserService">Servicio para obtener información del usuario actual.</param>
    /// <param name="logger">Administrador de registros.</param>
    /// <param name="currentTransactionService">Servicio para obtener información de la transaccion actual.</param>
    public LoggingBehaviour(ICurrentUserService currentUserService, ILoggerManager logger, ICurrentTransactionService currentTransactionService)
    {
        _currentUserService = currentUserService;
        _logger = logger;
        _currentTransactionService = currentTransactionService;
    }

    /// <summary>
    /// Maneja la solicitud y realiza el registro correspondiente antes y después de pasar al siguiente comportamiento en la tubería.
    /// </summary>
    /// <param name="request">La solicitud a manejar.</param>
    /// <param name="next">Delegado para invocar el siguiente comportamiento en la tubería.</param>
    /// <param name="cancellationToken">Token de cancelación para cancelar la ejecución.</param>
    /// <returns>La respuesta de la solicitud.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInfo(request, _currentUserService?.UserId + _currentTransactionService?.Transaction);
        
        var response = await next();

        _logger.LogInfo(response, _currentUserService?.UserId + _currentTransactionService?.Transaction);

        return response;
    }
}