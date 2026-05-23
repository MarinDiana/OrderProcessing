namespace OrderProcessing.Api.Domain;

public record OrderItem(
    string ProductId,
    string ProductName,
    int Quantity,
    Money UnitPrice,
    bool HasAgeRestriction);