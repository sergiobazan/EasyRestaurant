using Application.Abstractions.Messaging;
using Domain.Clients.Responses;

namespace Application.Clients.GetOrders;

public sealed record GetClientOrdersQuery(Guid CientId) : IQuery<GetClientOrdersResponse>;
