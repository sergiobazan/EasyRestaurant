using Domain.Menus.Responses;

namespace Domain.Menus;

public interface IMenuRepository
{
    Task<Menu?> GetAsync(Guid id);
    void Add(Menu menu);

    Task<List<MenuOrder>> GetOrdersAsync(Guid id);
}
