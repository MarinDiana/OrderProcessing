namespace OrderProcessing.Api.Domain;

using OrderProcessing.Api.States;

public class Order
{
    private readonly List<OrderItem> _items = new();

    public OrderId Id { get; }
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    private IOrderState _state = new PendingState();
    public Customer Customer { get; }
    public Address ShippingAddress { get; }
    public Money Total { get; private set; }
    public string CurrentState => _state.Name;

    public List<object> History { get; } = new();

    public Order(Customer customer, Address shippingAddress, IEnumerable<OrderItem> items)
    {
        Id = OrderId.New();
        Customer = customer;
        ShippingAddress = shippingAddress;

        _items.AddRange(items);
        Total = CalculateTotal();
    }

    public void SetState(IOrderState newState)
    {
        History.Add(new
        {
            fromState = _state.Name,
            toState = newState.Name,
            at = DateTime.UtcNow
        });

        _state = newState;
    }
    private Money CalculateTotal()
    {
        decimal totalAmount = 0;

        foreach (var item in _items)
        {
            totalAmount += item.UnitPrice.Amount * item.Quantity;
        }

        return new Money(totalAmount, "RON");
    }

    public void Pay()
    {
        _state.Pay(this);
    }

    public void Process()
    {
        _state.Process(this);
    }

    public void Ship()
    {
        _state.Ship(this);
    }

    public void Deliver()
    {
        _state.Deliver(this);
    }

    public void Cancel()
    {
        _state.Cancel(this);
    }
}