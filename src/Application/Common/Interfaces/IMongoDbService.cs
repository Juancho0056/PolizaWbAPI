using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;
using PolizaWebAPI.Domain.Common;

namespace PolizaWebAPI.Application.Common.Interfaces;
public interface IMongoDbService : IDisposable
{
    IMongoDatabase Conectar();
    Task<T> Add<T>(T documento, string collection) where T : BaseAuditableEntity;

    Task<IEnumerable<T>?> Query<T>(string collection, Expression<Func<T, bool>> filter) where T : BaseEntity, new();
}
