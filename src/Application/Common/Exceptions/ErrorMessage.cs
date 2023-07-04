using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolizaWebAPI.Application.Common.Exceptions;
/// <summary>
/// Clase estática que define mensajes de error comunes utilizados en validaciones.
/// </summary>
public static class ErrorMessage
{
    /// <summary>
    /// Representa el mensaje de error para la cantidad mínima de registros en una lista.
    /// </summary>
    public const string Count = "El mínimo de registros en la lista debe ser ";

    /// <summary>
    /// Representa el mensaje de error para un rango permitido.
    /// </summary>
    public const string Range = "Rango permitido de ";

    /// <summary>
    /// Representa el mensaje de error cuando ya existe un registro con los datos ingresados.
    /// </summary>
    public const string Exist = "Ya existe un registro con los datos ingresados ";

    /// <summary>
    /// Representa el mensaje de error para un campo requerido.
    /// </summary>
    public const string IsRequired = "Campo Requerido";

    /// <summary>
    /// La lista debe contener al menos 1 elemento
    /// </summary>
    public const string MinCount = "La lista debe contener al menos 1 elemento";

    /// <summary>
    /// Representa el mensaje de error para la longitud máxima de caracteres.
    /// </summary>
    public const string MaxLength = "Máximo de caracteres ";

    /// <summary>
    /// Representa el mensaje de error para la longitud mínima de caracteres.
    /// </summary>
    public const string MinLength = "Mínimo de caracteres ";

    /// <summary>
    /// Representa el mensaje de error para el valor máximo.
    /// </summary>
    public const string MaxValue = "Valor máximo ";

    /// <summary>
    /// Representa el mensaje de error para el valor máximo de precisión.
    /// </summary>
    public const string MaxValuePrecision = "Valor máximo de precisión ";

    /// <summary>
    /// Representa el mensaje de error para el valor mínimo.
    /// </summary>
    public const string MinValue = "Valor mínimo ";

    /// <summary>
    /// Representa el mensaje de error cuando se espera un campo numérico.
    /// </summary>
    public const string IsNumeric = "El campo debe ser numérico.";

    /// <summary>
    /// Representa el mensaje de error cuando se espera un campo alfabético.
    /// </summary>
    public const string IsAlphabetic = "El campo debe ser alfabético.";

    /// <summary>
    /// Representa el mensaje de error cuando se espera un campo alfanumérico.
    /// </summary>
    public const string IsAlphaNumeric = "El campo debe contener números y letras.";

    /// <summary>
    /// Representa el mensaje de error para el formato de fecha DD/MM/AAAA.
    /// </summary>
    public const string IsDate = "El formato de fecha debe ser DD/MM/AAAA.";

    /// <summary>
    /// Representa el mensaje de error para una dirección de correo electrónico inválida.
    /// </summary>
    public const string IsEmail = "El correo electrónico ingresado no es válido.";

    /// <summary>
    /// Representa el mensaje de error para una URL inválida.
    /// </summary>
    public const string IsUrl = "La URL ingresada no es válida.";

    /// <summary>
    /// Representa el mensaje de error para un formato no válido.
    /// </summary>
    public const string BadFormat = "No es un formato válido.";

    /// <summary>
    /// Representa el mensaje de error para la longitud decimal permitida.
    /// </summary>
    public const string DecimalLength = "Sólo se permite hasta X decimales";

    /// <summary>
    /// Representa el mensaje de error cuando se espera un formato de imagen.
    /// </summary>
    public const string IsImage = "El formato debe ser tipo imagen.";

    /// <summary>
    /// Representa el mensaje de error cuando se espera un formato PDF.
    /// </summary>
    public const string IsPdf = "El formato debe ser PDF.";

    /// <summary>
    /// Representa el mensaje de error cuando la fecha mínima no debe ser mayor a la fecha actual.
    /// </summary>
    public const string MinDateToday = "La fecha no debe ser mayor a la fecha actual.";

    /// <summary>
    /// Representa el mensaje de error cuando la fecha inicial no debe ser mayor a la fecha fin.
    /// </summary>
    public const string MinDateMaxDate = "La fecha inicial no debe ser mayor a la fecha fin.";

    /// <summary>
    /// Representa el mensaje de error cuando la fecha fin no debe ser menor a la fecha inicial.
    /// </summary>
    public const string MaxDateMinDate = "La fecha fin no debe ser menor a la fecha inicial.";

    /// <summary>
    /// Representa el mensaje de error cuando el valor no es un elemento del enumerador.
    /// </summary>
    public const string IsEnum = "No es un valor del enumerador.";

    public const string IsCharacter = "El campo solo puede tener espacios, numeros y letras.";

    public const string IsOnlyCharacter = "El campo solo puede tener numeros y letras.";

    /// <summary>
    /// Crea un mensaje de error personalizado cuando no se encuentra un objeto.
    /// </summary>
    /// <param name="entidad">La entidad buscada.</param>
    /// <returns>El mensaje de error personalizado.</returns>
    public static string NotFound(string entidad)
    {
        return $"No existe {entidad} con los datos ingresados.";
    }

    /// <summary>
    /// Crea un mensaje de error personalizado para la falla en la eliminación de un objeto.
    /// </summary>
    /// <param name="entidad">La entidad que no pudo ser eliminada.</param>
    /// <returns>El mensaje de error personalizado.</returns>
    public static string DeleteFailure(string entidad)
    {
        return $"La eliminación del objeto {entidad} ha fallado.";
    }

    /// <summary>
    /// Crea un mensaje de error personalizado cuando un objeto está en uso por otra entidad.
    /// </summary>
    /// <param name="entidad">La entidad que utiliza el objeto.</param>
    /// <returns>El mensaje de error personalizado.</returns>
    public static string InUse(string entidad)
    {
        return $"Objeto en uso por {entidad}";
    }
}