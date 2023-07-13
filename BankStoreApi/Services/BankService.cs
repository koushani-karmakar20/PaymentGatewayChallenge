using BankStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using BankStoreApi.Exceptions;

namespace BankStoreApi.Services;

public class BankService
{
    private  IMongoCollection<Customer> _customersCollection;
     private IMongoClient mongoClient;
    private readonly IOptions<BankStoreDatabaseSettings> bankStoreDatabaseSetting;
    public BankService(
        IOptions<BankStoreDatabaseSettings> bankStoreDatabaseSettings)
    {
       bankStoreDatabaseSetting = bankStoreDatabaseSettings;
        mongoClient = new MongoClient(
            bankStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bankStoreDatabaseSettings.Value.DatabaseName);

        _customersCollection = mongoDatabase.GetCollection<Customer>(
            bankStoreDatabaseSettings.Value.BankCollectionName);
    }

    public async Task<List<Customer>> GetAsync() =>
        await _customersCollection.Find(_ => true).ToListAsync();

    public async Task<Customer?> GetAsync(string Card_number) =>
        await _customersCollection.Find(x => x.Card_number == Card_number).FirstOrDefaultAsync();

    public async Task CreateAsync(Customer newCustomer) =>
        await _customersCollection.InsertOneAsync(newCustomer);

    public async Task UpdateAsync(string Card_number, Customer updatedCustomer) =>
        await _customersCollection.ReplaceOneAsync(x => x.Card_number == Card_number, updatedCustomer);

    public async Task RemoveAsync(string id) =>
        await _customersCollection.DeleteOneAsync(x => x.Id == id);

    public async Task Transaction(Payload payload )
    {

        Customer customer = await GetAsync(payload.Customer_card_number);
        if (customer is null)
           throw new CustomerNotFoundException($"{payload.Customer_card_number} not found");
        if(customer.CVV!=payload.CVV)
        throw new CredentialMismatchException("CVV mismatch");
        else if(!string.Equals(customer.Expiry,payload.Expiry))
        throw new CredentialMismatchException("Expiry month/year mismatch");
        else if(payload.Amount>customer.Balance)
        throw new InsufficientBalanceException("Current balance is less than amount requested");

        Customer merchant=await GetAsync(payload.Merchant_card_number);
        if(merchant is null)
            throw new CustomerNotFoundException($"{payload.Merchant_card_number} not found");
        
       

        using(var session = await mongoClient.StartSessionAsync())
        {
            session.StartTransaction();
            try {
                customer.Balance -= payload.Amount;

                UpdateAsync(customer.Card_number,customer);
                merchant.Balance += payload.Amount;
                UpdateAsync(merchant.Card_number,merchant);

                await session.CommitTransactionAsync();
        
        }
        catch(Exception e)
        {
            Console.WriteLine("error in transaction :"+e.Message);
            await session.AbortTransactionAsync();
            throw new Exception("Transcation rolled back!");
        }
    }
    }
}