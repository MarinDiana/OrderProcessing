namespace OrderProcessing.Api.States;

using OrderProcessing.Api.Domain;

public class ShippedState : IOrderState
{
    public string Name => "Shipped";

    public void Pay(Order order)
    {
        throw new InvalidOrderTransitionException(Name, "Pay");
    }

    public void Process(Order order)
    {
        throw new InvalidOrderTransitionException(Name, "Process");
    }

    public void Ship(Order order)
    {
        throw new InvalidOrderTransitionException(Name, "Ship");
    }

    public void Deliver(Order order)
    {
        order.SetState(new DeliveredState());
    }

    public void Cancel(Order order)
    {
        throw new InvalidOrderTransitionException(Name, "Cancel");
    }
}