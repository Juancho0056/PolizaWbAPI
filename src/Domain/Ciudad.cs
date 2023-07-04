using MongoDB.Bson.Serialization.Attributes;

namespace PolizaWebAPI.Domain;
[BsonIgnoreExtraElements()]
[BsonDiscriminator("ciudad")]
public class Ciudad 
{
    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;
}