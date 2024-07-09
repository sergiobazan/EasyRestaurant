using Application.Abstractions;
using Domain.Clients;
using Domain.Menus;
using Domain.Menus.Responses;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

internal class MenuRepository : Repository<Menu>, IMenuRepository
{
    private readonly ICacheService _cacheService;
    public MenuRepository(ApplicationDbContext context, ICacheService cacheService) : base(context)
    {
        _cacheService = cacheService;
    }

    public async Task<MenuOrder?> GetOrdersAsync(Guid id)
    {
        return await _cacheService
            .GetAsync(
            $"menu-{id}-orders",
            async () =>
            {
                MenuOrder? menuOrder = await _context.Set<Menu>()
                   .AsNoTracking()
                   .Include(menu => menu.Orders)
                   .ThenInclude(order => order.Dishes)
                   .AsSplitQuery()
                   .Where(menu => menu.Id == id)
                   .Select(menu => new MenuOrder(
                       menu.Id,
                       menu.Date,
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

                return menuOrder;
            });
    }
}
