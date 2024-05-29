using Application.Abstractions.Messaging;
using Domain.Menus.Responses;

namespace Application.Menus.GetOrders;

public sealed record GetOrdersQuery(Guid MenuId) : IQuery<List<MenuOrder>>;

