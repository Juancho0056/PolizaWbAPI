
using AutoMapper;
using PolizaWebAPI.Domain;
using PolizaWebAPI.Domain.Common;

namespace PolizaWebAPI.Application.Features.Polizas.Queries.Get;
public class PolizaDto
{
    public string Numero { get; set; } = string.Empty;
    public ClienteDto Cliente { get; set; } = new ClienteDto();
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public List<string> Cobertura { get; set; } = new List<string>();
    public decimal MontoCobertura { get; set; }
    public string NombrePlan { get; set; } = string.Empty;
    public VehiculoDto Vehiculo { get; set; } = new VehiculoDto();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Poliza, PolizaDto>();
            CreateMap<Vehiculo, VehiculoDto>();
            CreateMap<Cliente, ClienteDto>();
            CreateMap<Ciudad, CiudadDto>();
        }
    }
}

public class VehiculoDto
{
    public string Placa { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public bool Inspeccionado { get; set; }
}

public class ClienteDto
{
    public string Nombre { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public CiudadDto Ciudad { get; set; } = new CiudadDto();
    public string Direccion { get; set; } = string.Empty;
}

public class CiudadDto
{
    public string Nombre { get; set; } = string.Empty;
}