using Application.Abstractions.Messaging;

namespace Application.Orders.GetAll;

public sealed record GetAllOrdersQuery(Guid MenuId);
