namespace OrderProcessing.Api.States;

using OrderProcessing.Api.Domain;

public class ConfirmedState : IOrderState
{
    public string Name => "Confirmed";

    public void Pay(Order order)
    {
        throw new InvalidOrderTransitionException(Name, "Pay");
    }

    public void Process(Order order)
    {
        order.SetState(new ProcessingState());
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