using MongoDB.Bson.Serialization.Attributes;

namespace PolizaWebAPI.Domain;
[BsonIgnoreExtraElements]
[BsonDiscriminator("vehiculo")]
public class Vehiculo 
{
    [BsonElement("placa")]
    public string Placa { get; set; } = string.Empty;

    [BsonElement("modelo")]
    public string Modelo { get; set; } = string.Empty;

    [BsonElement("inspeccionado")]
    public bool Inspeccionado { get; set; }
}