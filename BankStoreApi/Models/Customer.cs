using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BankStoreApi.Models;

public class Customer
{
    [BsonId]
     [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

     public string Card_number { get; set; }=null!;

    //[BsonElement("Name")]
     public string Expiry{ get; set; } = null!;

    public int CVV { get; set; }

    public double Balance { get; set; }

   // public string Author { get; set; } = null!;
}