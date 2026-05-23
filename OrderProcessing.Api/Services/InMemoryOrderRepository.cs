namespace OrderProcessing.Api.Services;

using OrderProcessing.Api.Domain;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();

    public void Add(Order order)
    {
        _orders.Add(order);
    }

    public Order? GetById(Guid id)
    {
        return _orders.FirstOrDefault(order => order.Id.Value == id);
    }

    public List<Order> GetAll()
    {
        return _orders;
    }

    public void Update(Order order)
    {
        
    }
}