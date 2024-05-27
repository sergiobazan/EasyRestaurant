using Domain.Menus;

namespace Infraestructure.Repositories;

internal class MenuRepository : Repository<Menu>, IMenuRepository
{
    public MenuRepository(ApplicationDbContext context) : base(context)
    {
    }
}
