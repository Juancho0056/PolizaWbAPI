using FluentValidation.Results;

namespace PolizaWebAPI.Application.Common.Exceptions;

/// <summary>
/// Excepción que se lanza cuando se producen uno o más errores de validación.
/// </summary>
[Serializable]
public class ValidationException : Exception
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ValidationException"/>.
    /// </summary>
    public ValidationException()
        : base("Se han producido uno o más errores de validación.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ValidationException"/> con los errores de validación especificados.
    /// </summary>
    /// <param name="failures">Las fallas de validación.</param>
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    /// <summary>
    /// Obtiene los errores de validación asociados a la excepción.
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }
}