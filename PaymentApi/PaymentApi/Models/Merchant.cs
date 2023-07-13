using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentApi.Models;

public class Merchant
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    // [BsonElement("Name")]
    public string Merchant_id { get; set; } = null!;

    public bool Is_active { get; set; }

    public string API_key { get; set; } = null!;

    public string Card_number { get; set; } = null!;
}