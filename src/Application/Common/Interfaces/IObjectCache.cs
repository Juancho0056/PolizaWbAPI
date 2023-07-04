using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PolizaWebAPI.Application.Common.Interfaces;
/// <summary>
/// Interfaz para la administración de objetos en una caché.
/// </summary>
public interface IObjectCache
{
    /// <summary>
    /// Agrega un objeto a la caché con una clave y tiempo de expiración específicos.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto a almacenar en la caché.</typeparam>
    /// <param name="key">La clave asociada al objeto en la caché.</param>
    /// <param name="value">El objeto a almacenar en la caché.</param>
    /// <param name="expirationTime">El tiempo de expiración del objeto en la caché.</param>
    void Add<T>(string key, T value, TimeSpan expirationTime);

    /// <summary>
    /// Obtiene un objeto de la caché utilizando la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto a obtener de la caché.</typeparam>
    /// <param name="key">La clave asociada al objeto en la caché.</param>
    /// <returns>El objeto almacenado en la caché, o null si no se encuentra.</returns>
    T Get<T>(string key);

    /// <summary>
    /// Elimina un objeto de la caché utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave asociada al objeto en la caché.</param>
    void Remove(string key);

    /// <summary>
    /// Obtiene una colección de objetos de la caché que cumplan con un filtro opcional y se pueden ordenar opcionalmente.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto a obtener de la caché.</typeparam>
    /// <param name="filter">La expresión de filtro para aplicar a los objetos.</param>
    /// <param name="orderBy">La expresión de ordenación para aplicar a los objetos.</param>
    /// <param name="isDescending">Indica si la ordenación es descendente.</param>
    /// <returns>Una enumeración de objetos que cumplen con el filtro y ordenación especificados.</returns>
    IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> filter, Expression<Func<T, object>>? orderBy = null, bool isDescending = false) where T : class;

    T? GetFirst<T>(Expression<Func<T, bool>> filter) where T : class;

    /// <summary>
    /// Actualiza un objeto en la caché. Si el objeto ya existe en la caché, se elimina y se agrega el nuevo valor.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto a actualizar en la caché.</typeparam>
    /// <param name="key">La clave asociada al objeto en la caché.</param>
    /// <param name="newValue">El nuevo valor del objeto.</param>
    /// <param name="expirationTime">El tiempo de expiración del objeto en la caché.</param>
    void Update<T>(string key, T newValue, TimeSpan expirationTime) where T : class;
}