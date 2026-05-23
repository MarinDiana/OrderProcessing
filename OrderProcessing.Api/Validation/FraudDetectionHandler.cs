namespace OrderProcessing.Api.Validation;

using OrderProcessing.Api.Domain;

public class FraudDetectionHandler : BaseValidationHandler
{
    protected override ValidationResult Validate(Order order)
    {
        if (order.Total.Amount > 10000 &&
            order.Customer.IsTrusted == false)
        {
            return ValidationResult.Failure(
                "Potential fraud detected: untrusted customer with high value order");
        }

        if (order.Items.Count > 50)
        {
            return ValidationResult.Failure(
                "Potential fraud detected: too many distinct items");
        }

        return ValidationResult.Success();
    }
}