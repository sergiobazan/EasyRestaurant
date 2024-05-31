using Domain.Clients;
using Domain.Menus;
using Domain.Menus.Responses;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

internal class MenuRepository : Repository<Menu>, IMenuRepository
{
    public MenuRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<MenuOrder?> GetOrdersAsync(Guid id)
    {
        return await _context.Set<Menu>()
            .AsNoTracking()
            .Include(menu => menu.Orders)
            .ThenInclude(order => order.Dishes)
            .AsSplitQuery()
            .Where(menu => menu.Id == id)
            .Select(menu => new MenuOrder(
                menu.Id,
                menu.Date.Value,
                menu.Orders
                    .OrderByDescending(o => o.IsPriority)
                    .ThenBy(o => o.Date)
                    .Select(order => new OrderMenu(
                        order.Id,
                        order.Date,
                        order.Status,
                        order.Description.Value,
                        order.IsPriority,
                        order.ClientId,
                        _context.Set<Client>().Where(c => c.Id == order.ClientId).Select(c => c.Name.Value).FirstOrDefault()!,
                        order.Dishes
                            .Select(dish => new DishMenu(
                                dish.Id,
                                dish.Name.Value,
                                dish.Description.Value,
                                dish.Quantity.Value,
                                dish.Price.Amount,
                                dish.Status))
                        ))
                ))
            .FirstOrDefaultAsync();
    }
}
