using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentApi.Models;

public class RetrievePayload
{
    public string Payment_id { get; set; } = null!;


    public string Merchant_id { get; set; } = null!;

}