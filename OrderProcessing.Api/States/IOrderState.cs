namespace OrderProcessing.Api.States;

using OrderProcessing.Api.Domain;

public interface IOrderState
{
    string Name { get; }

    void Pay(Order order);
    void Process(Order order);
    void Ship(Order order);
    void Deliver(Order order);
    void Cancel(Order order);
}