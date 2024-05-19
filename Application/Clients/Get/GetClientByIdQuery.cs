using Application.Abstractions.Messaging;

namespace Application.Clients.Get;

public sealed record GetClientByIdQuery(Guid Id) : IQuery<GetClientByIdResponse>;
