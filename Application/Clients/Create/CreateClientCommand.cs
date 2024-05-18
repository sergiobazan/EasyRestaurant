using Domain.Abstractions;
using MediatR;

namespace Application.Clients.Create;

public sealed record CreateClientCommand(CreateClientRequest Client) : IRequest<Result<Guid>>;
