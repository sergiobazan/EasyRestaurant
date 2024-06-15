using Application.Abstractions.Messaging;

namespace Application.Clients.Login;

public sealed record LoginClientCommand(LoginClientRequest Login) : ICommand<string>;

public sealed record LoginClientRequest(string Prefix, string Phone);