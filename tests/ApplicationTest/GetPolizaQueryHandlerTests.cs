using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using PolizaWebAPI.Application.Common.Interfaces;
using PolizaWebAPI.Application.Features.Polizas.Queries.Get;
using PolizaWebAPI.Domain;

namespace ApplicationTest;
public class GetPolizaQueryHandlerTests
{
    [Fact]
    public async Task Handle_CuandoExistePolizaPorNumero()
    {
        // Arrange
        var request = new GetPolizaRequest { NumeroPoliza = "12345" };

        var polizas = new List<Poliza>
        {
            new Poliza { Numero = "12345", Vehiculo = new Vehiculo { Placa = "ABC123" }, Cliente = new Cliente { Nombre = "Juan" } },
            new Poliza { Numero = "67890", Vehiculo = new Vehiculo { Placa = "DEF456" }, Cliente = new Cliente { Nombre = "Pedro" } }
        };

        var polizaDtos = new List<PolizaDto>
        {
            new PolizaDto { Numero = "12345", Vehiculo = new VehiculoDto { Placa = "ABC123" }, Cliente = new ClienteDto { Nombre = "Juan" } }
        };

        var mongoDbServiceMock = new Mock<IMongoDbService>();
        mongoDbServiceMock.Setup(m => m.Query<Poliza>("polizas", It.IsAny<Expression<Func<Poliza, bool>>>()))
                         .ReturnsAsync((string collection, Expression<Func<Poliza, bool>> filter) =>
                         {
                             return polizas.Where(filter.Compile()).ToList();
                         });

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Poliza, PolizaDto>()
                .ForMember(dest => dest.Vehiculo, opt => opt.MapFrom(src => src.Vehiculo))
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente));
            cfg.CreateMap<Vehiculo, VehiculoDto>();
            cfg.CreateMap<Cliente, ClienteDto>()
                .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad));
            cfg.CreateMap<Ciudad, CiudadDto>();
        });

        var mapper = mapperConfig.CreateMapper();

        var queryHandler = new GetPolizaQueryHandler(mongoDbServiceMock.Object, mapper);

        // Act
        var result = await queryHandler.Handle(request, CancellationToken.None);

        

        // Assert
        Assert.Equal(polizaDtos?.FirstOrDefault()?.Numero, result.FirstOrDefault()?.Numero);
       
    }
}