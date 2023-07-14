using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;
using Moq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Moq.Protected;
using System.Threading;
using PaymentApi.Services;
using PaymentApi.Models;
//sing MockHttpRequest.UI.Services;
// using UtilityLibraries;
using RichardSzalay.MockHttp;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
// using Xunit;
using System.Text;

namespace StringLibraryTest;

[TestClass]
public class UnitTest1
{
//     [TestMethod]
//     public void TestStartsWithUpper()
//     {
//         // Tests that we expect to return true.
//         string[] words = { "Alphabet", "Zebra", "ABC", "Αθήνα", "Москва" };
//         foreach (var word in words)
//         {
//             char[] array = word.ToCharArray();
//             bool result = char.IsUpper(array[0]);
//             Assert.IsTrue(result,
//                    string.Format("Expected for '{0}': true; Actual: {1}",
//                                  word, result));
//         }
//     }

//     [TestMethod]
//     public void TestDoesNotStartWithUpper()
//     {
//         // Tests that we expect to return false.
//         string[] words = { "alphabet", "zebra", "abc", "αυτοκινητοβιομηχανία", "государство",
//                                "1234", ".", ";", " " };
//         foreach (var word in words)
//         {
//             char[] array = word.ToCharArray();
//             bool result = char.IsUpper(array[0]);
//             Assert.IsFalse(result,
//                    string.Format("Expected for '{0}': false; Actual: {1}",
//                                  word, result));
//         }
//     }

    // [TestMethod]
    // public void DirectCallWithNullOrEmpty()
    // {
    //     // Tests that we expect to return false.
    //     string?[] words = { string.Empty, null };
    //     foreach (var word in words)
    //     {
    //         bool result = StringLibrary.StartsWithUpper(word);
    //         Assert.IsFalse(result,
    //                string.Format("Expected for '{0}': false; Actual: {1}",
    //                              word == null ? "<null>" : word, result));
    //     }
    // }

    // [TestMethod]
    // public async Task TestSendPaymentRequest()
	// 	{
    //         var msgHandler = new MockHttpMessageHandler();
    //         msgHandler.When("http://localhost:5281/api/PaymentGateway/SendPaymentRequestToBank").Respond("text/json", "mocked user response");

	// 		var httpMessageHandlerMock = new Mock<HttpClient>();

	// 		HttpResponseMessage httpResponseMessage = new()
	// 		{
	// 			Content = JsonContent.Create(new
	// 			{
	// 				Id="64b00301be360fccd209ac76",
    //         Status=true,
            
    //         Description="Payment successful"
	// 			})
	// 		};

	// 		// Set up the SendAsync method behavior.
	// 		// httpMessageHandlerMock
	// 		// 	.Protected() // <= this is most important part that it need to setup.
	// 		// 	.Setup<Task<HttpResponseMessage>>(
	// 		// 		"PostAsync",ItExpr.IsAny<HttpRequestMessage>())
	// 		// 	.ReturnsAsync(httpResponseMessage);

	// 		// // create the HttpClient
	// 		// var httpClient = new HttpClient(httpMessageHandlerMock.Object)
	// 		// {
	// 		// 	BaseAddress = new System.Uri("http://localhost:5281/api/PaymentGateway/SendPaymentRequestToBank") // It should be in valid uri format.
	// 		// };
    //         PaymentPayload paymentPayload=new PaymentPayload{
    //             Customer_card_number= "2356-9809-888-0000",
    //             Expiry= "2028/09",
    //             Amount= 100,
    //             Currency= "EUR",
    //             CVV= 989,
    //             Merchant_id= "64ad531263ef5521562afa49"
    //         };
	// 		PaymentGatewayService _paymentGatewayService=null;
	// 		//Act
	// 		var result = await _paymentGatewayService.SendPaymentRequest(paymentPayload);
	// 		//Assert
	// 		Assert.AreEqual("64b00301be360fccd209ac76", result.Id);
    //         Assert.AreEqual(true, result.Status);
    //         Assert.AreEqual("Payment successful", result.Description);
	// 	}

        [TestMethod]
    public async Task TestSendPaymentRequest() {
        // var mockIMongoCollection = new Mock<IMongoCollection<Merchant>>();
//   var asyncCursor = new Mock<IAsyncCursor<Person>>();

//   var expectedResult = fixture.CreateMany<Person>(5);
  Merchant merchant = new Merchant{
    Id = "",
    Merchant_id = "",
    Is_active = true,
    API_key = "",
    Card_number = "",
  };

// List<Merchant> fakeRecords = new List<Merchant>()
//              {
//                  merchant
//              };

//     var mockMongoCollection = new Mock<IMongoCollection<Merchant>>();
  
//     //Mock IAsyncCursor
//     var mockCursor = new Mock<IAsyncCursor<Merchant>>();
//     mockCursor.Setup(x => x.Current).Returns(fakeRecords);
//     mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true);
             
//   mockMongoCollection.Setup(x => x.Find<Merchant>(Builders<Merchant>.Filter.Empty,default))
//                                 .Returns(merchant);
             
//   mockIMongoCollection.Setup(_collection => _collection.Find(
//       Builders<Merchant>.Filter.Empty,
//       default))
//     .ReturnsAsync(merchant);

//     mockIMongoCollection.Setup(x =>  x.Find(Builders<Merchant>.Filter.Empty, default)).ReturnsAsync(merchant);


//   asyncCursor.SetupSequence(_async => _async.MoveNext(default)).Returns(true).Returns(false);
//   asyncCursor.SetupGet(_async => _async.Current).Returns(expectedResult);

//   var result = LoadPeople(mockIMongoCollection.Object);
  
//   Assert.Equals(expectedResult, result);
// }


            var mockRepo = new Mock<PaymentGatewayService>();
        mockRepo.Setup(repo => repo.GetAsyncMerchantWithId("64ad531263ef5521562afa49"))
                .ReturnsAsync(merchant);

// var mockContactRepository = Mock.Create<PaymentGatewayService>();
// Mock.Arrange(() => mockContactRepository.GetContacts())
//   .Returns(new List<Contact>
//   {
//       new Contact { ContactId = 1 }, new Contact { ContactId = 2 }
//   });

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
            // IOptions<PaymentGatewayStoreDatabaseSettings>  paymentGatewayStoreDatabaseSettings1  = Options.Create(paymentGatewayStoreDatabaseSettings);
			PaymentGatewayService _paymentGatewayService=new PaymentGatewayService();
			//Act
            
            var mockHttpMessageHandler = new MockHttpMessageHandler();

    //setup a respond for the simple/price endpoing
    mockHttpMessageHandler
        .When("https://v6.exchangerate-api.com/v6/769f358153c8481724c568b6/pair/"+paymentPayload.Currency+"/EUR")
        .Respond("application/json", "{'result':'success','documentation':'https://www.exchangerate-api.com/docs','terms_of_use':'https://www.exchangerate-api.com/terms','time_last_update_unix':1689206401,'time_last_update_utc':'Thu, 13 Jul 2023 00:00:01 +0000','time_next_update_unix':1689292801,'time_next_update_utc':'Fri, 14 Jul 2023 00:00:01 +0000','base_code':'EUR','target_code':'EUR','conversion_rate':1}"); // return JSON


        // mockHttpMessageHandler
        // // .When("http://localhost:5121/Bank/PerformTransaction")
        // .When(HttpMethod.Post, new Uri("http://localhost:5121/Bank/PerformTransaction").ToString())
        // .Respond("application/json", "{'StatusCode':'204'"); // return JSON

    var obj = new BankPayload{
            Customer_card_number = "2356-9809-888-0000",
            Expiry= "2028/09",
            CVV=989,
            Amount= 100,
            Merchant_card_number= merchant.Card_number
        };
       
       //also handle exceptions that may be thrown by bank while processing payment - cut not found , invalid cvv etc 


        var payload = System.Text.Json.JsonSerializer.Serialize(obj);

// Wrap our JSON inside a StringContent object
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
    HttpResponseMessage httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
    // Console.WriteLine(obj.Merchant_card_number);
// var mockRepo1 = new Mock<PaymentGatewayService>();
//         mockRepo1.Setup(repo => repo.performBankTransaction(content))
//                 .ReturnsAsync(httpResponseMessage);
        // var mockRepo1 = new Mock<HttpMessageHandler>();
        // mockRepo1.Protected().Setup<Task<HttpResponseMessage>>("PostAsync", "http://localhost:5121/Bank/PerformTransaction", content)
        //         .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK));




        PaymentResponse paymentResponse = new PaymentResponse{
            Id = It.IsAny<string>(),
            Status = true,
            Description = "Payment successful"
        };
			var result = await mockRepo.Object.SendPaymentRequest(paymentPayload);
            Assert.AreEqual(result, paymentResponse);
	}
}
