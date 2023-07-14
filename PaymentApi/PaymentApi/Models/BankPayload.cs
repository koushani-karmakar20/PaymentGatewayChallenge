using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentApi.Models;

public class BankPayload
{
    public string Customer_card_number { get; set; } = null!;

    public string Expiry { get; set; }=null!;

    public double Amount{ get; set; } 

    public int CVV { get; set; } 


    public string Merchant_card_number { get; set; } = null!;
}
