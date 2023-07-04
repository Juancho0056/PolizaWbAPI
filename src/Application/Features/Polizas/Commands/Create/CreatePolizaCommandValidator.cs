using FluentValidation;
using PolizaWebAPI.Application.Common.Exceptions;

namespace PolizaWebAPI.Application.Features.Polizas.Commands.Create;
public class PolizaCommandValidator : AbstractValidator<CreatePolizaCommand>
{
    public PolizaCommandValidator()
    {
        RuleFor(x => x.Numero)
            .NotEmpty()
            .WithMessage(ErrorMessage.IsRequired)
            .MaximumLength(50).WithMessage(ErrorMessage.MaxLength + "50")
            .Matches(@"^[a-zA-Z0-9]+$")
            .WithMessage(ErrorMessage.IsOnlyCharacter);


        RuleFor(x => x.NombreCliente)
           .NotEmpty()
           .WithMessage(ErrorMessage.IsRequired)
           .MaximumLength(100)
           .WithMessage(ErrorMessage.MaxLength + "100")
           .Matches(@"^[a-zA-Z0-9\s]+$")
           .WithMessage(ErrorMessage.IsCharacter);

        RuleFor(x => x.FechaNacimientoCliente)
            .NotNull()
            .WithMessage(ErrorMessage.IsRequired)
            .NotEmpty()
            .WithMessage(ErrorMessage.IsRequired);

        RuleFor(x => x.CiudadResidenciaCliente)
          .NotEmpty()
          .WithMessage(ErrorMessage.IsRequired)
          .MaximumLength(100)
          .WithMessage(ErrorMessage.MaxLength + "100")
          .Matches(@"^[a-zA-Z0-9\s]+$")
          .WithMessage(ErrorMessage.IsCharacter);

        RuleFor(x => x.FechaInicio)
            .NotEmpty()
            .WithMessage(ErrorMessage.IsRequired);

        RuleFor(x => x.FechaFin)
            .NotEmpty()
            .WithMessage(ErrorMessage.IsRequired)
            .GreaterThan(x => x.FechaInicio).WithMessage(x => ErrorMessage.MinDateMaxDate + x.FechaInicio);

        RuleFor(x => x.Cobertura)
             .NotNull()
             .WithMessage(ErrorMessage.IsRequired)
             .Must(cobertura => cobertura != null && cobertura.Count > 0)
             .WithMessage(ErrorMessage.MinCount);

        RuleFor(x => x.MontoCobertura)
            .GreaterThan(1)
            .WithMessage(ErrorMessage.MinValue + "1")
            .PrecisionScale(14, 2, false)
            .WithMessage(ErrorMessage.MaxValue + 14 + "," + ErrorMessage.MaxValuePrecision + 2);

        RuleFor(x => x.NombrePlan)
            .NotEmpty()
            .WithMessage(ErrorMessage.IsRequired)
            .MaximumLength(100)
            .WithMessage(ErrorMessage.MaxLength + "100");


        RuleFor(x => x.PlacaVehiculo)
            .NotEmpty()
            .WithMessage(ErrorMessage.IsRequired)
            .MaximumLength(10)
            .WithMessage(ErrorMessage.MaxLength + "10")
            .Matches(@"^[a-zA-Z0-9]+$")
            .WithMessage(ErrorMessage.IsOnlyCharacter);

        RuleFor(x => x.ModeloVehiculo)
            .NotEmpty()
            .WithMessage(ErrorMessage.IsRequired)
            .MaximumLength(10)
            .WithMessage(ErrorMessage.MaxLength + "10")
            .Matches(@"^[a-zA-Z0-9]+$")
            .WithMessage(ErrorMessage.IsOnlyCharacter);

        RuleFor(x => x.VehiculoInspeccionado)
            .NotNull()
            .WithMessage(ErrorMessage.IsRequired);
    }
}
