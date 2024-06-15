using Application.Abstractions.Messaging;

namespace Application.Clients.Create;

public sealed record CreateClientCommand(CreateClientRequest Client) : ICommand<string>;
