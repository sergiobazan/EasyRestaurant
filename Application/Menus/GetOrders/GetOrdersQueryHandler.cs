using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Menus;
using Domain.Menus.Responses;

namespace Application.Menus.GetOrders;

public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, List<MenuOrder>>
{
    private readonly IMenuRepository _menuRepository;

    public GetOrdersQueryHandler(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<Result<List<MenuOrder>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _menuRepository.GetOrdersAsync(request.MenuId);
    }
}
