using System.Diagnostics;
using PolizaWebAPI.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace PolizaWebAPI.Application.Common.Behaviours;

/// <summary>
/// Representa un comportamiento de rendimiento que mide el tiempo de ejecución de una solicitud y registra un mensaje de advertencia si excede un umbral especificado.
/// </summary>
/// <typeparam name="TRequest">Tipo de la solicitud.</typeparam>
/// <typeparam name="TResponse">Tipo de la respuesta.</typeparam>
public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILoggerManager _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentTransactionService _currentTransactionService;

    /// <summary>
    /// Inicializa una nueva instancia de la clase PerformanceBehaviour.
    /// </summary>
    /// <param name="logger">Instancia del logger utilizado para registrar mensajes de advertencia.</param>
    /// <param name="currentUserService">Instancia del servicio que proporciona información del usuario actual.</param>
    /// <param name="currentTransactionService">Instancia del servicio que proporciona información de la transacción actual.</param>
    public PerformanceBehaviour(
        ILoggerManager logger,
        ICurrentUserService currentUserService,
        ICurrentTransactionService currentTransactionService)
    {
        _timer = new Stopwatch();
        _logger = logger;
        _currentUserService = currentUserService;
        _currentTransactionService = currentTransactionService;
    }

    /// <summary>
    /// Ejecuta el comportamiento de rendimiento.
    /// </summary>
    /// <param name="request">La solicitud a procesar.</param>
    /// <param name="next">El siguiente delegado de manipulador de solicitudes.</param>
    /// <param name="cancellationToken">Token de cancelación para cancelar la operación asincrónica.</param>
    /// <returns>La respuesta resultante de la manipulación de la solicitud.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? string.Empty;
            var transaction = _currentTransactionService.Transaction ?? string.Empty;

            var elapsedObject = new
            {
                Title = "PolizaWebAPI Long Running Request",
                Name = requestName,
                ElapsedMilliseconds = elapsedMilliseconds,
                UserId = userId,
                Transaction = transaction,
                Request = request
            };

            _logger.LogWarning(elapsedObject, userId + transaction);
        }

        return response;
    }
}