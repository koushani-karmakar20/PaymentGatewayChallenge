using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Moq.Protected;
using System.Threading;
using PaymentApi.Services;
using PaymentApi.Models;
using RichardSzalay.MockHttp;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Text;

namespace StringLibraryTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public async Task TestSendPaymentRequest() {
        
    Merchant merchant = new Merchant{
        Id = "64ad531263ef5521562afa49",
        Merchant_id = "64ad531263ef5521562afa49",
        Is_active = true,
        API_key = "820d76c8f753caa9bdb241f2fb7c81af35298978222cb4f661e287a0a8d09541",
        Card_number = "1234-5678-9234-9878",
    };


    var mockRepo = new Mock<PaymentGatewayService>();
        mockRepo.Setup(repo => repo.GetAsyncMerchantWithId("64ad531263ef5521562afa49"))
        .ReturnsAsync(merchant);

    PaymentPayload paymentPayload=new PaymentPayload{
                Customer_card_number= "2356-9809-888-0000",
                Expiry= "2028/09",
                Amount= 100,
                Currency= "EUR",
                CVV= 989,
                Merchant_id= "64ad531263ef5521562afa49"
            };
    PaymentGatewayStoreDatabaseSettings paymentGatewayStoreDatabaseSettings = new PaymentGatewayStoreDatabaseSettings{
        ConnectionString = "",
        DatabaseName = "",
        MerchantCollectionName = "",
        PaymentCollectionName = ""
    };
    PaymentGatewayService _paymentGatewayService=new PaymentGatewayService();
            
    var mockHttpMessageHandler = new MockHttpMessageHandler();

    mockHttpMessageHandler
        .When("https://v6.exchangerate-api.com/v6/769f358153c8481724c568b6/pair/"+paymentPayload.Currency+"/EUR")
        .Respond("application/json", "{'result':'success','documentation':'https://www.exchangerate-api.com/docs','terms_of_use':'https://www.exchangerate-api.com/terms','time_last_update_unix':1689206401,'time_last_update_utc':'Thu, 13 Jul 2023 00:00:01 +0000','time_next_update_unix':1689292801,'time_next_update_utc':'Fri, 14 Jul 2023 00:00:01 +0000','base_code':'EUR','target_code':'EUR','conversion_rate':1}"); // return JSON

    var obj = new BankPayload{
            Customer_card_number = "2356-9809-888-0000",
            Expiry= "2028/09",
            CVV=989,
            Amount= 100,
            Merchant_card_number= merchant.Card_number
        };
       
       //also handle exceptions that may be thrown by bank while processing payment - cut not found , invalid cvv etc 


        var payload = System.Text.Json.JsonSerializer.Serialize(obj);

        var content = new StringContent(payload, Encoding.UTF8, "application/json");
    HttpResponseMessage httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

    PaymentResponse paymentResponse = new PaymentResponse{
            Id = It.IsAny<string>(),
            Status = true,
            Description = "Payment successful"
        };
    var result = await mockRepo.Object.SendPaymentRequest(paymentPayload);
    Assert.AreEqual(result.Status, paymentResponse.Status);
	}
}
