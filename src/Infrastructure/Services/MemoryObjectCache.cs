using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using PolizaWebAPI.Application.Common.Interfaces;
using PolizaWebAPI.Domain.Common;

namespace PolizaWebAPI.Infrastructure.Services;
/// <summary>
/// Implementación de caché en memoria utilizando IMemoryObjectCache.
/// </summary>
public class MemoryObjectCache : IMemoryObjectCache
{
    private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

    /// <summary>
    /// Agrega un objeto a la caché con una clave y tiempo de expiración específicos.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto a almacenar en la caché.</typeparam>
    /// <param name="key">La clave asociada al objeto en la caché.</param>
    /// <param name="value">El objeto a almacenar en la caché.</param>
    /// <param name="expirationTime">El tiempo de expiración del objeto en la caché.</param>
    public void Add<T>(string key, T value, TimeSpan expirationTime)
    {
        _cache.Set(key, value, expirationTime);
    }

    /// <summary>
    /// Obtiene un objeto de la caché utilizando la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto a obtener de la caché.</typeparam>
    /// <param name="key">La clave asociada al objeto en la caché.</param>
    /// <returns>El objeto almacenado en la caché, o null si no se encuentra.</returns>
    public T Get<T>(string key)
    {
        return (T)_cache.Get(key);
    }

    /// <summary>
    /// Elimina un objeto de la caché utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave asociada al objeto en la caché.</param>
    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    /// <summary>
    /// Obtiene una colección de objetos de la caché que cumplan con un filtro opcional y se pueden ordenar opcionalmente.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto a obtener de la caché.</typeparam>
    /// <param name="filter">La expresión de filtro para aplicar a los objetos.</param>
    /// <param name="orderBy">La expresión de ordenación para aplicar a los objetos.</param>
    /// <param name="isDescending">Indica si la ordenación es descendente.</param>
    /// <returns>Una enumeración de objetos que cumplen con el filtro y ordenación especificados.</returns>
    /// <exception cref="InvalidOperationException">Se produce cuando la caché no está inicializada.</exception>
    public IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> filter, Expression<Func<T, object>>? orderBy = null, bool isDescending = false) where T : class
    {
        if (_cache == null)
        {
            throw new InvalidOperationException("La caché no está inicializada.");
        }

        var collection = _cache.GetOrCreate(typeof(T).Name, entry =>
        {
            entry.Size = 1;
            return new List<T>();
        });

        IQueryable<T> queryable = collection.AsQueryable();

        if (filter != null)
        {
            queryable = queryable.Where(filter);
        }

        if (orderBy != null)
        {
            queryable = isDescending ? queryable.OrderByDescending(orderBy) : queryable.OrderBy(orderBy);
        }

        return queryable.AsEnumerable();
    }

    public T? GetFirst<T>(Expression<Func<T, bool>> filter) where T : class
    {
        if (_cache == null)
        {
            throw new InvalidOperationException("La caché no está inicializada.");
        }

        var collection = _cache.GetOrCreate(typeof(T).Name, entry =>
        {
            entry.Size = 1;
            return new List<T>();
        });

        IQueryable<T> queryable = collection.AsQueryable();

        return  queryable.FirstOrDefault(filter);
       
    }

    /// <summary>
    /// Actualiza un objeto en la caché. Si el objeto ya existe en la caché, se elimina y se agrega el nuevo valor.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto a actualizar en la caché.</typeparam>
    /// <param name="key">La clave asociada al objeto en la caché.</param>
    /// <param name="newValue">El nuevo valor del objeto.</param>
    /// <param name="expirationTime">El tiempo de expiración del objeto en la caché.</param>
    public void Update<T>(string key, T newValue, TimeSpan expirationTime) where T : class
    {
        if (Get<T>(key) != null)
        {
            _cache.Remove(key);
        }

        Add(key, newValue, expirationTime);
    }
}