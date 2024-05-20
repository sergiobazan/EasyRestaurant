using Domain.Clients;
using Domain.Clients.Responses;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

internal class ClientRepository : Repository<Client>, IClientRepository
{
    public ClientRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<ClientOrderResponse>> GetOrdersAsync(Guid id)
    {
        return await _context
            .Set<Order>()
            .AsNoTracking()
            .Include(o => o.Dishes)
            .Where(o => o.ClientId == id)
            .Select(o => new ClientOrderResponse(
                o.Id,
                o.ClientId,
                o.Date,
                o.Status,
                o.Dishes.Select(d => new ClientOrderDishes(
                    d.Id,
                    d.Name.Value,
                    d.Price.Amount,
                    d.Description.Value,
                    d.Quantity.Value,
                    d.DishType,
                    d.Status)
                ).ToList()
              )
            )
            .ToListAsync();
    }
}
