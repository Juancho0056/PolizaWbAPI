using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PolizaWebAPI.Domain;
[BsonIgnoreExtraElements()]
[BsonDiscriminator("cliente")]
public class Cliente 
{
    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;
    [BsonElement("fechaNacimiento")]
    public DateTime FechaNacimiento { get; set; }
    [BsonElement("ciudad")] 
    public Ciudad Ciudad { get; set; } = new Ciudad();
    [BsonElement("direccion")]
    public string Direccion { get; set; } = string.Empty;
}