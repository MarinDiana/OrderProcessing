namespace OrderProcessing.Api.Validation;

using OrderProcessing.Api.Domain;

public class PriceValidationHandler : BaseValidationHandler
{
    protected override ValidationResult Validate(Order order)
    {
        decimal calculatedTotal = 0;

        foreach (var item in order.Items)
        {
            if (item.UnitPrice.Amount <= 0)
            {
                return ValidationResult.Failure(
                    $"Invalid price for product {item.ProductId}");
            }

            calculatedTotal += item.Quantity * item.UnitPrice.Amount;
        }

        if (calculatedTotal != order.Total.Amount)
        {
            return ValidationResult.Failure(
                "Order total does not match calculated total");
        }

        return ValidationResult.Success();
    }
}