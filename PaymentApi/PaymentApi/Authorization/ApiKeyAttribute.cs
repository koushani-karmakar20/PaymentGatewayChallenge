using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentApi.Services;
using MongoDB.Driver;
using PaymentApi.Models;
using System.Text;

namespace AwesomeApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class ApiKeyAttribute : Attribute, IAuthorizationFilter
{
    private const string API_KEY_HEADER_NAME = "X-API-Key";

    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        var submittedApiKey = GetSubmittedApiKey(context.HttpContext);
        var apiKey = await GetApiKey(context.HttpContext);

        if (!IsApiKeyValid(apiKey, submittedApiKey))
        {
            context.Result = new UnauthorizedResult();
        }
    }

    private static string GetSubmittedApiKey(HttpContext context)
    {
        return context.Request.Headers[API_KEY_HEADER_NAME];
    }

    private static async Task<string> GetApiKey(HttpContext context)
    {
        string merchantIdentifier = context.Request.Query["merchantIdentifier"].ToString();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var mongoClient = new MongoClient(configuration.GetValue<string>("PaymentGatewayStoreDatabase:ConnectionString"));

        var mongoDatabase = mongoClient.GetDatabase(configuration.GetValue<string>("PaymentGatewayStoreDatabase:DatabaseName"));

        IMongoCollection<Merchant> merchantsCollection = mongoDatabase.GetCollection<Merchant>(configuration.GetValue<string>("PaymentGatewayStoreDatabase:MerchantCollectionName"));
        Merchant merchant = await merchantsCollection.Find(x => x.Id == merchantIdentifier).FirstOrDefaultAsync();

        return merchant.API_key;
    }

    private static bool IsApiKeyValid(string apiKey, string submittedApiKey)
    {
        if (string.IsNullOrEmpty(submittedApiKey)) return false;

        return apiKey==submittedApiKey;
    }
}