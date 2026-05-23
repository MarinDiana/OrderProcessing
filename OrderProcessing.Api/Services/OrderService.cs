namespace OrderProcessing.Api.Services;

using OrderProcessing.Api.Domain;
using OrderProcessing.Api.States;
using OrderProcessing.Api.Validation;

public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IOrderValidationHandler _validationChain;

    public OrderService(
        IOrderRepository repository,
        IOrderValidationHandler validationChain)
    {
        _repository = repository;
        _validationChain = validationChain;
    }

    public (Order? Order, ValidationResult ValidationResult) CreateOrder(CreateOrderRequest request)
    {
        var order = new Order(
            request.Customer,
            request.ShippingAddress,
            request.Items);

        var validationResult = _validationChain.Handle(order);

        if (!validationResult.IsValid)
        {
            return (null, validationResult);
        }

        _repository.Add(order);

        return (order, ValidationResult.Success());
    }

    public Order? GetOrder(Guid id)
    {
        return _repository.GetById(id);
    }

    public List<Order> GetAllOrders()
    {
        return _repository.GetAll();
    }

    public void PayOrder(Guid id)
    {
        var order = _repository.GetById(id);

        if (order == null)
        {
            return;
        }

        order.Pay();

        _repository.Update(order);
    }

    public void ProcessOrder(Guid id)
    {
        var order = _repository.GetById(id);

        if (order == null)
        {
            return;
        }

        order.Process();

        _repository.Update(order);
    }

    public void ShipOrder(Guid id)
    {
        var order = _repository.GetById(id);

        if (order == null)
        {
            return;
        }

        order.Ship();

        _repository.Update(order);
    }

    public void DeliverOrder(Guid id)
    {
        var order = _repository.GetById(id);

        if (order == null)
        {
            return;
        }

        order.Deliver();

        _repository.Update(order);
    }

    public void CancelOrder(Guid id)
    {
        var order = _repository.GetById(id);

        if (order == null)
        {
            return;
        }

        order.Cancel();

        _repository.Update(order);
    }
}