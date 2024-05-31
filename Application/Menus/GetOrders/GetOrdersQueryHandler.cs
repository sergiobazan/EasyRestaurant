using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Menus;
using Domain.Menus.Responses;

namespace Application.Menus.GetOrders;

public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, MenuOrder>
{
    private readonly IMenuRepository _menuRepository;

    public GetOrdersQueryHandler(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<Result<MenuOrder>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        MenuOrder? menu = await _menuRepository.GetOrdersAsync(request.MenuId);

        if (menu is null)
        {
            return Result.Failure<MenuOrder>(MenuErrors.MenuNotFound(request.MenuId));
        }

        return menu;
    }
}
