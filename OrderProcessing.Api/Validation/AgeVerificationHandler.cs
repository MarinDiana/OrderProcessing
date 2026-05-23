namespace OrderProcessing.Api.Validation;

using OrderProcessing.Api.Domain;

public class AgeVerificationHandler : BaseValidationHandler
{
    protected override ValidationResult Validate(Order order)
    {
        bool hasRestrictedItems = order.Items.Any(
            item => item.HasAgeRestriction);

        if (hasRestrictedItems && order.Customer.Age < 18)
        {
            return ValidationResult.Failure(
                "Customer must be at least 18 years old.");
        }

        return ValidationResult.Success();
    }
}