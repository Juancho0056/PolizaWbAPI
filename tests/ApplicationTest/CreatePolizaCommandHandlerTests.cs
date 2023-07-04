using System.Linq.Expressions;
using System.Reflection;
using Moq;
using PolizaWebAPI.Application.Common.Interfaces;
using PolizaWebAPI.Application.Features.Polizas.Commands.Create;
using PolizaWebAPI.Domain.Exceptions;
using PolizaWebAPI.Domain;

namespace ApplicationTest;

public class CreatePolizaCommandHandlerTests
{
    private Mock<IMongoDbService> _mongoDbServiceMock;
    private Mock<ICurrentUserService> _currentUserServiceMock;
    private CreatePolizaCommandHandler _commandHandler;

    public CreatePolizaCommandHandlerTests()
    {
        _mongoDbServiceMock = new Mock<IMongoDbService>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _commandHandler = new CreatePolizaCommandHandler(
            _mongoDbServiceMock.Object,
            _currentUserServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_RequestValido()
    {
        // Arrange
        var request = new CreatePolizaCommand
        {
            Numero = "123490",
            NombreCliente = "Juan Perez",
            FechaNacimientoCliente = new DateTime(1980,1, 1),
            DireccionCliente = "Calle Principal 123",
            CiudadResidenciaCliente = "Ciudad de Mexico",
            FechaInicio =  DateTime.Parse("2023-07-01"),
            FechaFin = DateTime.Parse("2024-07-01"),
            Cobertura = new List<string>() { "Daños Materiales", "Robo Total" },
            MontoCobertura = 50000000,
            NombrePlan = "Plan Básico",
            PlacaVehiculo = "ABC123",
            ModeloVehiculo = "Sedan",
            VehiculoInspeccionado = true

        };

        _mongoDbServiceMock.Setup(m => m.Query<Poliza>(It.IsAny<string>(), It.IsAny<Expression<Func<Poliza, bool>>>()))
                  .ReturnsAsync(new List<Poliza>());

        _mongoDbServiceMock.Setup(m => m.Add(It.IsAny<Poliza>(), It.IsAny<string>()))
            .ReturnsAsync(new Poliza());

        // Act
        await _commandHandler.Handle(request, CancellationToken.None);

        // Assert
        _mongoDbServiceMock.Verify(m => m.Query<Poliza>("polizas", It.IsAny<Expression<Func<Poliza, bool>>>()), Times.Once);
        _mongoDbServiceMock.Verify(m => m.Add(It.IsAny<Poliza>(), "polizas"), Times.Once);
    }

    [Fact]
    public async Task Handle_PolizaNoVigente()
    {
        // Arrange
        var request = new CreatePolizaCommand
        {
            Numero = "123490",
            NombreCliente = "Juan Perez",
            FechaNacimientoCliente = new DateTime(1980, 1, 1),
            DireccionCliente = "Calle Principal 123",
            CiudadResidenciaCliente = "Ciudad de Mexico",
            FechaInicio = DateTime.Parse("2023-08-01"),
            FechaFin = DateTime.Parse("2024-07-01"),
            Cobertura = new List<string>() { "Daños Materiales", "Robo Total" },
            MontoCobertura = 50000000,
            NombrePlan = "Plan Basico",
            PlacaVehiculo = "ABC123",
            ModeloVehiculo = "Sedan",
            VehiculoInspeccionado = true
        };

        
        _commandHandler.GetType()
            .GetMethod("EsPolizaVigente", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(_commandHandler, new object[] { request.FechaInicio, request.FechaFin });

        // Act & Assert
        await Assert.ThrowsAsync<UnsupportedPolizaException>(() => _commandHandler.Handle(request, CancellationToken.None));
    }
}
