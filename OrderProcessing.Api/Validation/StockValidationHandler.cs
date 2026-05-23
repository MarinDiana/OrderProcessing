namespace OrderProcessing.Api.Validation;

using OrderProcessing.Api.Domain;

public class StockValidationHandler : BaseValidationHandler
{
    private readonly Dictionary<string, int> _stock = new()
    {
        { "P001", 10 },
        { "P002", 5 },
        { "P003", 3 }
    };

    protected override ValidationResult Validate(Order order)
    {
        foreach (var item in order.Items)
        {
            if (!_stock.ContainsKey(item.ProductId))
            {
                return ValidationResult.Failure(
                $"{item.ProductName} is not in stock");
            }

            if (_stock[item.ProductId] < item.Quantity)
            {
                return ValidationResult.Failure(
                    $"Insufficient stock for product {item.ProductId}");
            }
        }

        return ValidationResult.Success();
    }
}