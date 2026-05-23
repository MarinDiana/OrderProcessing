using OrderProcessing.Api.Endpoints;
using OrderProcessing.Api.Services;
using OrderProcessing.Api.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

builder.Services.AddSingleton<IOrderValidationHandler>(sp =>
{
    var stock = new StockValidationHandler();
    var price = new PriceValidationHandler();
    var fraud = new FraudDetectionHandler();
    var age = new AgeVerificationHandler();

    stock
        .SetNext(price)
        .SetNext(fraud)
        .SetNext(age);

    return stock;
});

builder.Services.AddScoped<OrderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapOrderEndpoints();

app.Run();