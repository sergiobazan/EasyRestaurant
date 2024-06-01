using Application.Abstractions.Messaging;

namespace Application.Clients.Update;

public sealed record UpdateClientCommand(Guid Id, UpdateClientRequest Client) : ICommand<Guid>;

public sealed record UpdateClientRequest(
    string Name,
    string Phone);