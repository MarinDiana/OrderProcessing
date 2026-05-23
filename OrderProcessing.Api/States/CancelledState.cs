namespace OrderProcessing.Api.States;

using OrderProcessing.Api.Domain;

public class CancelledState : IOrderState
{
    public string Name => "Cancelled";

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
        throw new InvalidOrderTransitionException(Name, "Deliver");
    }

    public void Cancel(Order order)
    {
        throw new InvalidOrderTransitionException(Name, "Cancel");
    }
}