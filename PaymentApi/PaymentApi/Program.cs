using PaymentApi.Models;
using PaymentApi.Services;
// using System.Net.Http.Headers;

// using HttpClient client = new();
// var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<PaymentGatewayStoreDatabaseSettings>(
    builder.Configuration.GetSection("PaymentGatewayStoreDatabase"));

builder.Services.AddSingleton<PaymentGatewayService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// client.DefaultRequestHeaders.Accept.Clear();
// client.DefaultRequestHeaders.Accept.Add(
//     new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
// client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

// await ProcessRepositoriesAsync(client);

// static async Task ProcessRepositoriesAsync(HttpClient client)
// {

//     var json = await client.GetStringAsync(
//          "http://localhost:5121/Bank/PerformTransaction");

//      Console.Write(json);
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


// using PaymentApi.Models;
// using PaymentApi.Services;
// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.Configure<PaymentGatewayStoreDatabaseSettings>(
//     builder.Configuration.GetSection("PaymentGatewayStoreDatabase"));

//     builder.Services.AddSingleton<PaymentGatewayService>();

// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers();

// app.Run();


