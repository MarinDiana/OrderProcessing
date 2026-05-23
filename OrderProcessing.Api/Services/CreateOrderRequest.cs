namespace OrderProcessing.Api.Services;

using OrderProcessing.Api.Domain;

public record CreateOrderRequest(
    Customer Customer,
    Address ShippingAddress,
    List<OrderItem> Items);