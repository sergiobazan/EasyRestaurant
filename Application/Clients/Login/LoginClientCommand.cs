using Application.Abstractions.Messaging;

namespace Application.Clients.Login;

public sealed record LoginClientCommand(string Email, string Password) : ICommand<string>;


public sealed record LoginClientRequest(string Email, string Password);