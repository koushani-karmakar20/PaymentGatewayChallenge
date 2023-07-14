using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BankStoreApi.Models;

public class Payload
{
    public string Customer_card_number { get; set; }=null!;
    
    public string Expiry{ get; set; } = null!;

    public int CVV { get; set; }

    public double Amount { get; set; }
    public string Merchant_card_number{get; set;}=null!;
}