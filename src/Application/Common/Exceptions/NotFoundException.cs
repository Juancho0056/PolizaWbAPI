namespace PolizaWebAPI.Application.Common.Exceptions;

/// <summary>
/// Excepción que se lanza cuando no se encuentra una entidad específica.
/// </summary>
[Serializable]
public class NotFoundException : Exception
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NotFoundException"/>.
    /// </summary>
    public NotFoundException()
        : base()
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NotFoundException"/> con un mensaje de error especificado.
    /// </summary>
    /// <param name="message">El mensaje que describe el error.</param>
    public NotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NotFoundException"/> con un mensaje de error especificado y una referencia a la excepción interna que es la causa de esta excepción.
    /// </summary>
    /// <param name="message">El mensaje que describe el error.</param>
    /// <param name="innerException">La excepción que es la causa del error actual.</param>
    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NotFoundException"/> con el nombre y la clave de la entidad que no se encontró.
    /// </summary>
    /// <param name="name">El nombre de la entidad que no se encontró.</param>
    /// <param name="key">La clave de la entidad que no se encontró.</param>
    public NotFoundException(string name, object key)
        : base($"No se encontró la entidad \"{name}\" ({key}).")
    {
    }
}