namespace PaymentApi.Models;

public class PaymentGatewayStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string MerchantCollectionName { get; set; } = null!;

    public string PaymentCollectionName { get; set; } = null!;
}