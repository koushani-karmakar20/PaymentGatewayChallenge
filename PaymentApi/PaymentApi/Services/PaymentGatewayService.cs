using PaymentApi.Models;
//using BookStoreApi.Models;
using PaymentApi.Exceptions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Net.Http;
using System.Net.Http.Headers;
 using System.Web;
using System.Text.Json;
using System.Net;
using System.Text;
//using Newtonsoft.Json.Linq;
// using System.Text.Json.JsonSerializer;

namespace PaymentApi.Services;

public class PaymentGatewayService
{
    private readonly IMongoCollection<Payment> _paymentsCollection;
    private readonly IMongoCollection<Merchant> _merchantsCollection;

    // private const string URL = "https://jsonplaceholder.typicode.com/todos";
    // private string urlParameters = "?api_key=123";

    public PaymentGatewayService(
        IOptions<PaymentGatewayStoreDatabaseSettings> paymentGatewayStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            paymentGatewayStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            paymentGatewayStoreDatabaseSettings.Value.DatabaseName);

        _paymentsCollection = mongoDatabase.GetCollection<Payment>(
            paymentGatewayStoreDatabaseSettings.Value.PaymentCollectionName);

        _merchantsCollection = mongoDatabase.GetCollection<Merchant>(
            paymentGatewayStoreDatabaseSettings.Value.MerchantCollectionName);

    }

    public async Task<List<Payment>> GetAsyncPayment() =>
        await _paymentsCollection.Find(_ => true).ToListAsync();

    public async Task<List<Merchant>> GetAsyncMerchant() =>
        await _merchantsCollection.Find(_ => true).ToListAsync();


    

    

    public async Task<Payment?> GetAsyncPaymentWithId(string id) =>
        await _paymentsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

     public async Task<Merchant?> GetAsyncMerchantWithId(string id) =>
        await _merchantsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    

    public async Task CreateAsync(Payment newPayment) =>
        await _paymentsCollection.InsertOneAsync(newPayment);
    
    public async Task CreateAsync(Merchant newMerchant) =>
        await _merchantsCollection.InsertOneAsync(newMerchant);

    public async Task UpdateAsync(string id, Payment updatedPayment) =>
        await _paymentsCollection.ReplaceOneAsync(x => x.Id == id, updatedPayment);
    
    public async Task UpdateAsync(string id, Merchant updatedMerchant) =>
        await _merchantsCollection.ReplaceOneAsync(x => x.Id == id, updatedMerchant);

    

    public async Task RemoveAsyncPayment(string id) =>
        await _paymentsCollection.DeleteOneAsync(x => x.Id == id);

    public async Task RemoveAsyncMerchant(string id) =>
        await _merchantsCollection.DeleteOneAsync(x => x.Id == id);


    
    public async Task <PaymentResponse?> SendPaymentRequest(PaymentPayload paymentPayload) {

        //check if the merchant id exists in the payment payload , or if the merchant is active or not 

        Merchant merchant=await GetAsyncMerchantWithId(paymentPayload.Merchant_id);
        if(merchant is null)
        {
            throw new MerchantNotFoundException($"Merchant : {paymentPayload.Merchant_id} not found");
        }
        if(!merchant.Is_active)
        {
            throw new MerchantInactiveException($"Merchant : {paymentPayload.Merchant_id} is no longer active");
        }

        //api authentication to be done here 
        //if authenticated , currency conversion from payload currency to euro to be done
        var client2=new HttpClient();
        var result=await client2.GetStringAsync("https://v6.exchangerate-api.com/v6/769f358153c8481724c568b6/pair/"+paymentPayload.Currency+"/EUR");
        JsonDocument doc = JsonDocument.Parse(result);  
        JsonElement root = doc.RootElement;  
        double conversion = root.GetProperty("conversion_rate").GetDouble(); 
        double converted_amount=paymentPayload.Amount * conversion;
        Console.WriteLine(converted_amount);

        HttpClient client = new HttpClient();



        client.BaseAddress = new Uri("http://localhost:5121/Bank/");

        // Add an Accept header for JSON format.
        client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

        var obj = new BankPayload{
            Customer_card_number = paymentPayload.Customer_card_number,
            Expiry= paymentPayload.Expiry,
            CVV= paymentPayload.CVV,
            Amount= converted_amount,
            Merchant_card_number= merchant.Card_number
        };
       
       //also handle exceptions that may be thrown by bank while processing payment - cut not found , invalid cvv etc 


        var payload = System.Text.Json.JsonSerializer.Serialize(obj);

// Wrap our JSON inside a StringContent object
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        // // List data response.
        HttpResponseMessage response= client.PostAsync("PerformTransaction", content).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
        //once the payment is successfully completed , create a record of the payment in payments database
        //rn considering only successful payment

        //string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(response);

        string description="";
        bool status;
        if (response.IsSuccessStatusCode)
        {   status=true;
            description="Payment successful";
        }
        else{
            status=false;
            description="Payment unsuccessful: "+response.ReasonPhrase;
        }

        
        Payment payment=new Payment{
            Status=status,
            Timestamp=DateTime.Now,
            Customer_card_number=paymentPayload.Customer_card_number,
            Expiry= paymentPayload.Expiry,
            CVV= paymentPayload.CVV,
            Amount= paymentPayload.Amount,
            Currency=paymentPayload.Currency,
            Merchant_id=paymentPayload.Merchant_id,
            Description=description
        };

        await CreateAsync(payment);

        PaymentResponse paymentResponse=new PaymentResponse{
            Id=payment.Id,
            Status=payment.Status,
            Timestamp=payment.Timestamp,
            Description=payment.Description
        };
        return paymentResponse;
        //Console.WriteLine(response);
        // if (response.IsSuccessStatusCode)
        // {
        //     // Parse the response body.
        //     var dataObjects = await response.Content.ReadAsStringAsync(); //right!
            
        //     Console.WriteLine(JsonConvert.DeserializeObject<object>(dataObjects));
        //     // foreach (var d in dataObjects)
        //     // {
        //     //     Console.WriteLine(d);
        //     // }
        //     //  Console.WriteLine("{0}");
        // }
        // else
        // {
        //     Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        // }
        //await _paymentsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();


        //  HttpClient client2 = new HttpClient();



        // client2.BaseAddress = new Uri("https://v6.exchangerate-api.com/v6/769f358153c8481724c568b6/pair/EUR/GBP");

        // // Add an Accept header for JSON format.
        // client2.DefaultRequestHeaders.Accept.Add(
        // new MediaTypeWithQualityHeaderValue("application/json"));

        
        // // List data response.
        
       //Task< HttpResponseMessage> response2= client2.GetAsync("https://v6.exchangerate-api.com/v6/769f358153c8481724c568b6/pair/EUR/GBP");
       //Console.WriteLine(result);

        
    }

    public async Task<RetrieveResponse?> RetrievePayment(RetrievePayload retrievePayload) {
        Merchant merchant=await GetAsyncMerchantWithId(retrievePayload.Merchant_id);
        if(merchant is null)
        {
            throw new MerchantNotFoundException($"Merchant : {retrievePayload.Merchant_id} not found");
        }
        if(!merchant.Is_active)
        {
            throw new MerchantInactiveException($"Merchant : {retrievePayload.Merchant_id} is no longer active");
        }
        Payment payment=await GetAsyncPaymentWithId(retrievePayload.Payment_id);

        RetrieveResponse retrieveResponse=new RetrieveResponse{
            Status=payment.Status,
            Timestamp=payment.Timestamp,
            Customer_card_number=payment.Customer_card_number,
            Expiry=payment.Expiry,
            Amount=payment.Amount,
            Currency=payment.Currency,
            Merchant_id=payment.Merchant_id,
            Description=payment.Description
        };
        return retrieveResponse;
    }
        
    

    
}