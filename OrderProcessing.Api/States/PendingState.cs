namespace OrderProcessing.Api.States;

using OrderProcessing.Api.Domain;

public class PendingState : IOrderState
{
    public string Name => "Pending";

    public void Pay(Order order)
    {
        order.SetState(new ConfirmedState());
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
        order.SetState(new CancelledState());
    }
}