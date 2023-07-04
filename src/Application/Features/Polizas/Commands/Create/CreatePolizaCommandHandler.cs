using MediatR;
using Microsoft.EntityFrameworkCore;
using PolizaWebAPI.Application.Common.Interfaces;
using PolizaWebAPI.Domain;
using PolizaWebAPI.Domain.Exceptions;

namespace PolizaWebAPI.Application.Features.Polizas.Commands.Create;
public class CreatePolizaCommandHandler : IRequestHandler<CreatePolizaCommand, Unit>
{
    private readonly IMongoDbService _mongoDbService;
    private readonly ICurrentUserService _currentUserService;

    public CreatePolizaCommandHandler(IMongoDbService mongoDbService, ICurrentUserService currentUserService)
    {
        _mongoDbService = mongoDbService;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(CreatePolizaCommand request, CancellationToken cancellationToken)
    {
       
        cancellationToken.ThrowIfCancellationRequested();

        if (!EsPolizaVigente(request.FechaInicio, request.FechaFin))
        {
            throw new UnsupportedPolizaException("La poliza no se encuentra vigente");
        }
        var polizaMongo = await _mongoDbService.Query("polizas", (Poliza poliza) => poliza.Numero.Equals(request.Numero));

        if(polizaMongo!=null && polizaMongo.Any())
            throw new UnsupportedPolizaException("Ya existe una poliza para el numero "+request.Numero);

        var poliza = new Poliza
        {
            Numero = request.Numero,
            Cliente = new Cliente() 
            {
                Nombre = request.NombreCliente,
                FechaNacimiento = request.FechaNacimientoCliente,
                Ciudad = new Ciudad() 
                {
                    Nombre = request.CiudadResidenciaCliente
                },
                Direccion = request.DireccionCliente,
            },
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            Cobertura = request.Cobertura,
            MontoCobertura = request.MontoCobertura,
            NombrePlan = request.NombrePlan,
            Vehiculo = new Vehiculo() 
            {
                Placa = request.PlacaVehiculo,
                Modelo = request.ModeloVehiculo,
                Inspeccionado = request.VehiculoInspeccionado
            },
            CreatedBy = _currentUserService.UserId
        };

        await _mongoDbService.Add(poliza, "polizas");

        return Unit.Value;
        
    }

    private bool EsPolizaVigente(DateTime fechaInicio, DateTime fechaFin)
    {
        DateTime now = DateTime.UtcNow;
        return fechaInicio <= now && now <= fechaFin;
    }
}