namespace OrderProcessing.Api.Endpoints;

using OrderProcessing.Api.Services;
using OrderProcessing.Api.States;

public static class OrderEndpoints
{
    private static object ToResponse(OrderProcessing.Api.Domain.Order order)
    {
        return new
        {
            id = order.Id.Value,
            status = order.CurrentState,
            customer = order.Customer,
            items = order.Items,
            address = order.ShippingAddress,
            total = order.Total,
            history = order.History
        };
    }

    public static void MapOrderEndpoints(this WebApplication app)
    {
        app.MapPost("/orders", (
            CreateOrderRequest request,
            OrderService service) =>
        {
            var result = service.CreateOrder(request);

            if (!result.ValidationResult.IsValid)
            {
                return Results.BadRequest(result.ValidationResult);
            }

            return Results.Created(
                $"/orders/{result.Order!.Id.Value}",
                ToResponse(result.Order!));
        });

        app.MapPost("/orders/{id:guid}/pay", (
            Guid id,
            OrderService service) =>
        {
            try
            {
                service.PayOrder(id);

                var order = service.GetOrder(id);

                if (order == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(ToResponse(order));
            }
            catch (InvalidOrderTransitionException ex)
            {
                return Results.Conflict(new { error = ex.Message });
            }
        });

        app.MapPost("/orders/{id:guid}/process", (
            Guid id,
            OrderService service) =>
        {
            try
            {
                service.ProcessOrder(id);

                var order = service.GetOrder(id);

                if (order == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(ToResponse(order));
            }
            catch (InvalidOrderTransitionException ex)
            {
                return Results.Conflict(new { error = ex.Message });
            }
        });

        app.MapPost("/orders/{id:guid}/ship", (
            Guid id,
            OrderService service) =>
        {
            try
            {
                service.ShipOrder(id);

                var order = service.GetOrder(id);

                if (order == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(ToResponse(order));
            }
            catch (InvalidOrderTransitionException ex)
            {
                return Results.Conflict(new { error = ex.Message });
            }
        });

        app.MapPost("/orders/{id:guid}/deliver", (
            Guid id,
            OrderService service) =>
        {
            try
            {
                service.DeliverOrder(id);

                var order = service.GetOrder(id);

                if (order == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(ToResponse(order));
            }
            catch (InvalidOrderTransitionException ex)
            {
                return Results.Conflict(new { error = ex.Message });
            }
        });

        app.MapPost("/orders/{id:guid}/cancel", (
            Guid id,
            OrderService service) =>
        {
            try
            {
                service.CancelOrder(id);

                var order = service.GetOrder(id);

                if (order == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(ToResponse(order));
            }
            catch (InvalidOrderTransitionException ex)
            {
                return Results.Conflict(new { error = ex.Message });
            }
        });

        app.MapGet("/orders/{id:guid}", (
            Guid id,
            OrderService service) =>
        {
            var order = service.GetOrder(id);

            if (order == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(ToResponse(order));
        });

        app.MapGet("/orders", (
            OrderService service) =>
        {
            return Results.Ok(
                service.GetAllOrders().Select(ToResponse));
        });
    }
}