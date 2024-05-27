using Application.Menus.Create;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Presentation;

public class MenuModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("menus", async (CreateMenuRequest request, ISender sender) =>
        {
            var command = new CreateMenuCommand(request.DishIds);
            var result = await sender.Send(command);

            return result.IsSuccess ? Results.Created($"menus/{result.Value}", result.Value) : Results.BadRequest(result.Error); 
        });
    }

    public sealed record CreateMenuRequest(List<Guid> DishIds);
}
