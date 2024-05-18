namespace Domain.Menus;

public interface IMenuRepository
{
    Task<List<Menu>> GetMenusAsync();
    void Add(Menu menu);
}
