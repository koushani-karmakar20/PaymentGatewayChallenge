using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentApi.Models;

public class Payment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    //[BsonElement("Name")]
   // public string Payment_id { get; set; } = null!;

    public bool Status { get; set; }

    public DateTime Timestamp { get; set; } 

    public string Customer_card_number { get; set; } = null!;

    public string Expiry { get; set; } = null!;
    public double Amount { get; set; } 

    public string Currency { get; set; } = null!;

    public int CVV { get; set; }

    public string Merchant_id { get; set; } = null!;

    public string Description { get; set; } = null!;



}