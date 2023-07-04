using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PolizaWebAPI.Domain.Common;

public abstract class BaseEntity
{
    [BsonId]
    public ObjectId Id { get; set; }
    public bool Status { get; set; }

}
