using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Newtonsoft.Json;
using PolizaWebAPI.Application.Features.Polizas.Commands.Create;
using PolizaWebAPI.Application.Features.Polizas.Queries.Get;
using PolizaWebAPI.WebUI.Controllers;

namespace WebUI.Controllers;

/// <summary>
/// Controlador para la gestión de pólizas
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class PolizaController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public PolizaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Crea una nueva póliza
    /// </summary>
    /// <param name="command">Datos de la póliza a crear</param>
    /// <returns>Respuesta de la acción</returns>
    [HttpPost("[action]")]
    [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Create([FromBody] CreatePolizaCommand command)
    {
        return await base.Command<CreatePolizaCommand, Unit>(command);
    }

    /// <summary>
    /// Obtiene una lista de pólizas que coinciden con los filtros especificados
    /// </summary>
    /// <param name="query">Filtros de búsqueda</param>
    /// <returns>Lista de pólizas</returns>
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IEnumerable<PolizaDto>> Get([FromQuery] GetPolizaRequest query)
    {
        return await _mediator.Send(query);
    }
}