namespace OrderProcessing.Api.Validation;

using OrderProcessing.Api.Domain;

public interface IOrderValidationHandler
{
    IOrderValidationHandler SetNext(IOrderValidationHandler handler);

    ValidationResult Handle(Order order);
}