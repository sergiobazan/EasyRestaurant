using Application.Dishes.Create;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Presentation;

public class DishModule : CarterModule
{
    public DishModule()
        : base("api/dishes")
    {
        WithTags("Dishes");
    }
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateDishRequest request, ISender sender) =>
        {
            var command = new CreateDishCommand(request);
            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Created($"api/dishes/{result.Value}", result.Value);
        });
    }
}
