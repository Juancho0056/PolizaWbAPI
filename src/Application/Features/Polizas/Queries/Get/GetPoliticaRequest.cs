using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using PolizaWebAPI.Application.Common.Interfaces;
using PolizaWebAPI.Application.Common.Models;
using PolizaWebAPI.Domain;

namespace PolizaWebAPI.Application.Features.Polizas.Queries.Get;
public record GetPolizaRequest : IRequest<IEnumerable<PolizaDto>>
{
    public string NumeroPoliza { get; init; } = string.Empty;
    public string PlacaVehiculo { get; set; } = string.Empty;
}

public class GetPolizaQueryHandler : IRequestHandler<GetPolizaRequest, IEnumerable<PolizaDto>>
{
    private readonly IMongoDbService _mongoDbService;
    private readonly IMapper _mapper;

    public GetPolizaQueryHandler(IMongoDbService mongoDbService, IMapper mapper)
    {
        _mongoDbService = mongoDbService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PolizaDto>> Handle(GetPolizaRequest request, CancellationToken cancellationToken)
    {
        var poliza = await _mongoDbService.Query("polizas", (Poliza poliza) => (poliza.Numero.Equals(request.NumeroPoliza)));

        if(poliza is null || !poliza.Any())
            poliza = await _mongoDbService.Query("polizas", (Poliza poliza) => (poliza.Vehiculo.Placa.Equals(request.PlacaVehiculo)));

        return poliza?.AsQueryable()
             .ProjectTo<PolizaDto>(_mapper.ConfigurationProvider) ?? Enumerable.Empty<PolizaDto>();
    }
}