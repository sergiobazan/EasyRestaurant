using Application.Clients.Create;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Application.Clients.Get;
using Application.Clients.GetOrders;
using Application.Clients.Login;

namespace Presentation;

public class ClientModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/clients", async (CreateClientRequest request, ISender sender) =>
        {
            var command = new CreateClientCommand(request);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        });

        app.MapPost("/clients/login", async (LoginClientRequest request, ISender sender) =>
        {
            var command = new LoginClientCommand(request.Email, request.Password);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        });

        app.MapGet("/clients/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetClientByIdQuery(id);
            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }

            return Results.Ok(result.Value);
        }).RequireAuthorization();

        app.MapGet("/clients/{id:guid}/orders", async (Guid id, ISender sender) =>
        {
            var query = new GetClientOrdersQuery(id);
            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }

            return Results.Ok(result.Value);
        }).RequireAuthorization();
    }
}
