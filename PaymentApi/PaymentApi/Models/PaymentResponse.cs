using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentApi.Models;

public class PaymentResponse
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    //[BsonElement("Name")]
   // public string Payment_id { get; set; } = null!;

    public bool Status { get; set; }

    public DateTime Timestamp { get; set; } 


    public string Description { get; set; } = null!;



}