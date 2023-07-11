namespace BankStoreApi.Models;

public class BankStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string BankCollectionName { get; set; } = null!;
}