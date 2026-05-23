namespace OrderProcessing.Api.Validation;

using OrderProcessing.Api.Domain;

public abstract class BaseValidationHandler : IOrderValidationHandler
{
    private IOrderValidationHandler? _nextHandler;

    public IOrderValidationHandler SetNext(IOrderValidationHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }

    public ValidationResult Handle(Order order)
    {
        var result = Validate(order);

        if (!result.IsValid)
        {
            return result;
        }

        if (_nextHandler != null)
        {
            return _nextHandler.Handle(order);
        }

        return ValidationResult.Success();
    }

    protected abstract ValidationResult Validate(Order order);
}