namespace Application.Clients.Create;

public sealed record CreateClientRequest(
    string Email,
    string Name,
    string Prefix,
    string Phone,
    string Gender);
