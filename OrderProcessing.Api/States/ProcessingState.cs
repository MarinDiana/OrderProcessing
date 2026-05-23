namespace OrderProcessing.Api.States;

using OrderProcessing.Api.Domain;

public class ProcessingState : IOrderState
{
    public string Name => "Processing";

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
        order.SetState(new ShippedState());
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