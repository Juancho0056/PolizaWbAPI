using MediatR;
using PolizaWebAPI.Domain;

namespace PolizaWebAPI.Application.Features.Polizas.Commands.Create;
public class CreatePolizaCommand : IRequest<Unit>
{
    public string Numero { get; set; } = string.Empty;
    #region Cliente
    public string NombreCliente { get; set; } = string.Empty;
    public DateTime FechaNacimientoCliente { get; set; }
    
    public string DireccionCliente { get; set; } = string.Empty;

    #region ciudad
    public string CiudadResidenciaCliente { get; set; } = string.Empty;

    #endregion ciudad

    #endregion Cliente
    public DateTime FechaInicio { get; set; } = DateTime.Now;
    public DateTime FechaFin { get; set; }
    public List<string> Cobertura { get; set; } = new List<string>();
    public decimal MontoCobertura { get; set; }
    public string NombrePlan { get; set; } = string.Empty;
    #region Vehiculo
    public string PlacaVehiculo { get; set; } = string.Empty;
    public string ModeloVehiculo { get; set; } = string.Empty;
    public bool VehiculoInspeccionado { get; set; }
    #endregion Vehiculo
}