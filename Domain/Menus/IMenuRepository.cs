using Domain.Menus.Responses;

namespace Domain.Menus;

public interface IMenuRepository
{
    Task<Menu?> GetAsync(Guid id);
    void Add(Menu menu);

    Task<MenuOrder?> GetOrdersAsync(Guid id);
}
