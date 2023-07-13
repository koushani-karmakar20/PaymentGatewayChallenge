using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BankStoreApi.Models;

public class Payload
{
    // [BsonId]
    //  [BsonRepresentation(BsonType.ObjectId)]
    // public string? Id { get; set; }

     public string Customer_card_number { get; set; }=null!;

    //[BsonElement("Name")]
     public string Expiry{ get; set; } = null!;

    public int CVV { get; set; }

    public double Amount { get; set; }

    public string Merchant_card_number{get; set;}=null!;

   // public string Author { get; set; } = null!;
}