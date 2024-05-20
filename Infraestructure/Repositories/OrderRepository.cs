using Domain.Orders;

namespace Infraestructure.Repositories;

internal class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }
}
