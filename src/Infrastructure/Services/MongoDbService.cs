using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PolizaWebAPI.Application.Common.Interfaces;
using PolizaWebAPI.Domain.Common;
using PolizaWebAPI.Infrastructure.Common;

namespace PolizaWebAPI.Infrastructure.Services;
/// <summary>
/// Servicio para interactuar con MongoDB.
/// </summary>
public class MongoDbService : IMongoDbService, IDisposable
{
    private IMongoDatabase? database;
    private readonly MongoConfiguration _mongoConfiguration;
    private readonly CacheConfiguration _cacheConfiguration;
    private readonly IMemoryObjectCache _cache;
    private bool _disposed;

    /// <summary>
    /// Constructor de la clase MongoDbService.
    /// </summary>
    /// <param name="mongoConfiguration">Configuración de MongoDB.</param>
    /// <param name="cache">Caché de objetos en memoria.</param>
    /// <param name="cacheConfiguration">Configuración de la caché.</param>
    public MongoDbService(IOptions<MongoConfiguration> mongoConfiguration, IMemoryObjectCache cache, IOptions<CacheConfiguration> cacheConfiguration)
    {
        this._mongoConfiguration = mongoConfiguration.Value;
        this._cache = cache;
        _cacheConfiguration = cacheConfiguration.Value;
        this.database = Conectar();
    }

    /// <summary>
    /// Establece la conexión con la base de datos de MongoDB.
    /// </summary>
    /// <returns>La instancia de IMongoDatabase que representa la conexión.</returns>
    public IMongoDatabase Conectar()
    {
        if (database == null)
        {
            MongoUrl? url = new MongoUrl(_mongoConfiguration.ConnectionString);
            MongoClientSettings? clientSettings = MongoClientSettings.FromUrl(url);
            clientSettings.SslSettings = new SslSettings();
            clientSettings.SslSettings.CheckCertificateRevocation = _mongoConfiguration.CheckCertificateRevocation;
            clientSettings.UseTls = _mongoConfiguration.UseTls;
            clientSettings.AllowInsecureTls = _mongoConfiguration.AllowInsecureTls;

            MongoClient? client = new(clientSettings);

            this.database = client.GetDatabase(_mongoConfiguration.DatabaseName);
        }
        return database;
    }

    /// <summary>
    /// Realiza la liberación de recursos utilizados por la clase.
    /// </summary>
    /// <param name="disposing">Indica si se están liberando los recursos de manera explícita.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;
        }
    }

    /// <summary>
    /// Realiza la liberación de recursos utilizados por la clase.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Realiza una consulta en una colección de MongoDB utilizando un filtro especificado.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad de MongoDB.</typeparam>
    /// <param name="collection">Nombre de la colección de MongoDB.</param>
    /// <param name="filter">Expresión de filtro para la consulta.</param>
    /// <returns>Una colección de entidades que cumplen con el filtro especificado.</returns>
    /// <exception cref="ArgumentNullException">Se produce cuando el filtro es nulo.</exception>
    /// <exception cref="Exception">Se produce cuando ocurre un error durante la consulta a la base de datos.</exception>
    public async Task<IEnumerable<T>?> Query<T>(string collection, Expression<Func<T, bool>> filter) where T : BaseEntity, new()
    {
        IEnumerable<T> result = _cache.GetAll<T>(filter);
        if (result != null && result.Any())
        {
            return result;
        }

        FilterDefinition<T> filterDefinition = Builders<T>.Filter.Where(filter);
        if (filterDefinition == null)
        {
            throw new ArgumentNullException(nameof(filter), "No se envió un filtro de consulta.");
        }

        this.database ??= Conectar();

        IMongoCollection<T> mongoCollection = this.database.GetCollection<T>(collection);

        result = await mongoCollection.Find(filterDefinition).ToListAsync();

        if (result != null)
        {
            Type entityType = typeof(T);

            _cache.Add(entityType.Name, result, TimeSpan.FromMinutes(_cacheConfiguration.ExpirationMinutes));
        }

        return result;
    }

    public async Task<T?> Get<T>(string collection, Expression<Func<T, bool>> filter) where T : BaseEntity, new()
    {

        T? result = _cache.GetFirst<T>(filter);
        if (result != null)
        {
            return result;
        }

        FilterDefinition<T> filterDefinition = Builders<T>.Filter.Where(filter);
        if (filterDefinition == null)
        {
            throw new ArgumentNullException(nameof(filter), "No se envió un filtro de consulta.");
        }

        this.database ??= Conectar();

        IMongoCollection<T> mongoCollection = this.database.GetCollection<T>(collection);

        T? queryResult = await mongoCollection.Find(filter).FirstOrDefaultAsync();

        if (queryResult != null)
        {
            Type entityType = typeof(T);

            _cache.Add(entityType.Name, queryResult, TimeSpan.FromMinutes(_cacheConfiguration.ExpirationMinutes));
        }

        return queryResult;
    }

    /// <summary>
    /// Agrega un documento a una colección de MongoDB.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad del documento.</typeparam>
    /// <param name="documento">El documento a agregar.</param>
    /// <param name="collection">Nombre de la colección de MongoDB.</param>
    /// <returns>El documento agregado.</returns>
    public async Task<T> Add<T>(T documento, string collection) where T : BaseAuditableEntity
    {
        DateTime now = DateTime.UtcNow;
        documento.Created = now;

        this.database ??= Conectar();
        IMongoCollection<T> colleccion = database.GetCollection<T>(collection);

        await colleccion.InsertOneAsync(documento);

        _cache.Add(nameof(T), documento, TimeSpan.FromMinutes(_cacheConfiguration.ExpirationMinutes));

        return documento;
    }

    /// <summary>
    /// Actualiza un documento en una colección de MongoDB.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad del documento.</typeparam>
    /// <param name="documento">El documento actualizado.</param>
    /// <param name="collection">Nombre de la colección de MongoDB.</param>
    /// <returns>El documento actualizado.</returns>
    private async Task<T> Update<T>(T documento, string collection) where T : BaseAuditableEntity
    {
        documento.LastModified = DateTime.UtcNow;

        this.database ??= Conectar();
        IMongoCollection<T> colleccion = database.GetCollection<T>(collection);

        ReplaceOneResult result = await colleccion.ReplaceOneAsync(d => d.Id == documento.Id, documento);

        if (result.ModifiedCount > 0)
        {
            _cache.Update(nameof(T), documento, TimeSpan.FromMinutes(_cacheConfiguration.ExpirationMinutes));
        }

        return documento;
    }
}