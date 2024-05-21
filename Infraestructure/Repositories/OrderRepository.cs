using Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

internal class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Order?> GetAsync(Guid id)
    {
        return await _context
            .Set<Order>()
            .Include(o => o.Dishes)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
