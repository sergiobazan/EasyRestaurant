namespace Application.Clients.Create;

public sealed record CreateClientRequest(
    string Name,
    string Prefix,
    string Phone,
    string Gender);
