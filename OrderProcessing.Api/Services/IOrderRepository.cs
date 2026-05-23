namespace OrderProcessing.Api.Services;

using OrderProcessing.Api.Domain;

public interface IOrderRepository
{
    void Add(Order order);

    Order? GetById(Guid id);

    List<Order> GetAll();

    void Update(Order order);
}