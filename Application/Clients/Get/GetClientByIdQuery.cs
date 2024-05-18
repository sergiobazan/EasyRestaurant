using Domain.Abstractions;
using MediatR;

namespace Application.Clients.Get;

public sealed record GetClientByIdQuery(Guid Id) : IRequest<Result<GetClientByIdResponse>>;
