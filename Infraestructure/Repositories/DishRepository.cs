using Domain.Dishes;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

internal class DishRepository : Repository<Dish>, IDishRepository
{
    public DishRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Dish>> GetByIdsAsync(List<Guid> ids)
    {
        return await _context.Set<Dish>()
            .Where(dish => ids.Contains(dish.Id))
            .ToListAsync();
    }
}
