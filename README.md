# PaymentGatewayChallenge
![Untitled drawio](https://github.com/koushani-karmakar20/PaymentGatewayChallenge/assets/72240585/320702df-1db4-4118-a770-8f4440164acb)

## Assumptions
- Bank stores the balance in Euro.
- Bank stores the merchant and customer details in the same database since a merchant for payment gateway is also a customer for Bank.

## Architecture
The Payment Gateway is built on .NET 7.0 framework using C# language. It serves REST APIs to perform transactions and retrieve past payments. MongoDB has been used as the NoSQL database for this project. The project has been hosted on Microsoft Azure. 
- There are 2 different services for MockBank and PaymentGateway.
### Implementation
#### PaymentGateway
- The PaymentGateway uses a NoSQL database hosted in MongoDB Atlas. NoSQL database(MongoDB) is used to store record of payments processed from Merchants and the Merchant details.
- NoSQL is used to reduce hosting expenses, fast queries, flexibility to change field names, add new fields and scale vertically in future.
- The Payment Gateway receives payload for multiple currency, used `https://www.exchangerate-api.com/`'s API to convert the incoming currency to Euro.
- Payment Gateway uses a API key based authentication, API key is a SHA256 hash of the Merchant ID stored in database, which needs to be passed as a `X-API-Key` header in the POST request.
- Payment Gateway also verifies if the merchant is active or not by making a request to the database.
#### MockBank
- The BankAPI uses a NoSQL database hosted in MongoDB Atlas, NoSQL database is used to store record of customers in the bank.
- While implementing this, I kept in mind about Transactions and Atomicity of the read/write oepration,and used `mongoClient.StartSessionAsync` method to encapsulate the debit and credit process, it can be found in `BankService.cs`.
- Bank verifies the credentials such as `Expiry MM/YY` and `CVV` for a card number provided in payload and checks the feasibility of a transaction by comparing requested amount and available balance for the customer and throws appropriate exception in case of credential mismatch or insufficient balance.

## Database Design
![image](https://github.com/koushani-karmakar20/PaymentGatewayChallenge/assets/72240585/c4183c6b-44b7-4554-8a5a-3c9cb4a879e4)

## Payload and Response
- `/api/PaymentGateway/SendPaymentRequestToBank`
  
  Mock Payload: 
  ```
  {
    "customer_card_number": "2356-9809-888-0000",
    "expiry": "2028/09",
    "amount": 100,
    "currency": "EUR",
    "cvv": 989,
    "merchant_id": "64ad531263ef5521562afa49"
  }
  ```
  Mock Response:
  ```
  {
      "id": "64b091133a0d4349b763c960",
      "status": true,
      "description": "Payment successful"
  }
  ```
  
- `/api/PaymentGateway/RetrievePayment`

  Mock Payload:
  ```
  {
    "id": "64aec9c54235f9c1cf142821",
    "payment_id": "64aec9c54235f9c1cf142821",
    "merchant_id": "64ad531263ef5521562afa49"
  }
  ```
  Mock Response:
  ```
  {
    "id": null,
    "status": true,
    "timestamp": "2023-07-12T15:41:57.923Z",
    "customer_card_number": "2356-9809-888-0000",
    "expiry": "2028/09",
    "amount": 1000,
    "currency": "eur",
    "merchant_id": "64ad531263ef5521562afa49",
    "description": "Payment successful"
  }
  ```

## Flow of data
- For POST `/api/PaymentGateway/SendPaymentRequestToBank`
<img width="6064" alt="Payment Gateway" src="https://github.com/koushani-karmakar20/PaymentGatewayChallenge/assets/72240585/cb84c2a6-40f7-4c75-a249-0c71e17885fd">

- For GET `/api/PaymentGateway/RetrievePayment`
<img width="6064" alt="Payment Gateway" src="https://github.com/koushani-karmakar20/PaymentGatewayChallenge/assets/72240585/95d4b6d3-4265-4776-80cd-c3cac1941e0f">

## Local setup
- The project uses .NET 7.0 runtime.
- Clone the project using `git clone https://github.com/koushani-karmakar20/PaymentGatewayChallenge.git`.
- To setup the project to work locally, you will need the MongoDB cluster URL **which is sent in the submission email** to avert security risks of exposing database URL.
- Populate both project's `appsettings.json` file's `PaymentGatewayStoreDatabase.ConnectionString` with `mongodb+srv://koushani:<password>@cluster0.gf5i8wh.mongodb.net/` format.
- To run the MockBank Api locally, use `cd BankStoreApi` and `dotnet run`.
- To run the PaymentGateway locally, change `BankAPI.Url` to use MockBank Api's URL `http://localhost:5121` or keep it as it is to use the deployed version of MockBankAPI and then `cd PaymentApi/PaymentApi` and `dotnet run`.

## Extra Mile
- The Payment Gateway receives payload for multiple currency, used `https://www.exchangerate-api.com/`'s API to convert the incoming currency to Euro.
- API key based verification for PaymentGateway requests.
- Frontend dashboard to monitor payment history for each merchant and their API keys.
- Transaction based read/write to prevent rollback failures and persist atomicity and concurrency during credit/debit of money.

## Testing
Unit tests have been added to the code.

## Deployments
- MockBank has been deployed to Azure App Services in https://mockbank.azurewebsites.net.
- PaymentGateway has been deployed to Azure App Services in https://paymentgateway-koushani.azurewebsites.net.
- Payment Gateway frontend monitoring tool built using ReactJS has been deployed to Azure App Services in https://paymentgatewayfrontend.azurewebsites.net/.

## API Signatures
The Postman collection is attached in the repository. Also, you can find the swagger URL is run locally by appending `/swagger.html` to the localhost:port URL.

## Scope of Improvement
- A messaging queue and lambda can be added to the add merchants logic to keep the SHA256 hash generation logic sharded from the DB write logic and to write payment logs to DB.
  ![Untitled drawio (1)](https://github.com/koushani-karmakar20/PaymentGatewayChallenge/assets/72240585/b4696ceb-4b9a-461a-a49d-090ba98f8ba3)

- Better logging and analytics with tools like Google Analytics.
- Better API metric logging and monitoring.
- API rate limting using the already implemented API key logic.
- Better unit tests. 
