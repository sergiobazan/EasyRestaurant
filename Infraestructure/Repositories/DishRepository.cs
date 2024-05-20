using Domain.Dishes;

namespace Infraestructure.Repositories;

internal class DishRepository : Repository<Dish>, IDishRepository
{
    public DishRepository(ApplicationDbContext context) : base(context)
    {
    }
}
