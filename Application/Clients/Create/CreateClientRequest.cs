namespace Application.Clients.Create;

public sealed record CreateClientRequest(
    string Email,
    string Password,
    string Name,
    string Prefix,
    string Phone,
    string Gender);
