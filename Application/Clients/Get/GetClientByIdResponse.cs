namespace Application.Clients.Get;

public sealed record GetClientByIdResponse(
    Guid Id,
    string Name,
    string Prefix,
    string Phone,
    string Gender);