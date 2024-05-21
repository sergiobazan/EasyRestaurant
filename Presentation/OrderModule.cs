﻿using Application.Orders.Cancel;
using Application.Orders.Create;
using Application.Orders.Delivered;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Presentation;

public class OrderModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            var command = new CreateOrderCommand(request.ClientId, request.Description, request.DishIds);
            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Created($"orders/{result.Value}", result.Value);
        });

        app.MapPost("/orders/{id:guid}/delivered", async (Guid id, ISender sender) =>
        {
            var command = new OrderDeliveredCommand(id);
            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok();
        });

        app.MapPost("/orders/{id:guid}/canceled", async (Guid id, ISender sender) =>
        {
            var command = new OrderCanceledCommand(id);
            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok();
        });
    }

    public sealed record CreateOrderRequest(Guid ClientId, string? Description, List<Guid> DishIds);
}
