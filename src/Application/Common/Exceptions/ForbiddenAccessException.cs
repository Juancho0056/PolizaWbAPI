namespace PolizaWebAPI.Application.Common.Exceptions;

/// <summary>
/// Excepción que se lanza cuando se produce un acceso no permitido.
/// </summary>
[Serializable]
public class ForbiddenAccessException : Exception
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ForbiddenAccessException"/>.
    /// </summary>
    public ForbiddenAccessException()
        : base()
    {
    }
}