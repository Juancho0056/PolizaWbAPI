using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PolizaWebAPI.Domain;
[BsonIgnoreExtraElements]
[BsonDiscriminator("poliza")]
public class Poliza : BaseAuditableEntity
{
    [BsonElement("numero")]
    public string Numero { get; set; } = string.Empty;
    [BsonElement("cliente")]
    public Cliente Cliente { get; set; } = new Cliente();

    [BsonElement("fechaInicio")]
    public DateTime FechaInicio { get; set; }

    [BsonElement("fechaFin")]
    public DateTime FechaFin { get; set; }

    [BsonElement("cobertura")]
    public List<string> Cobertura { get; set; } = new List<string>();

    [BsonElement("montoCobertura")]
    public decimal MontoCobertura { get; set; }

    [BsonElement("nombrePlan")]
    public string NombrePlan { get; set; } = string.Empty;
    [BsonElement("vehiculo")]
    public Vehiculo Vehiculo { get; set; } = new Vehiculo();
}