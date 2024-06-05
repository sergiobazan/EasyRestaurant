using Application.Abstractions;
using Domain.Clients;
using Domain.Clients.Responses;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

internal class ClientRepository : Repository<Client>, IClientRepository
{
    private readonly ICacheService _cacheService;
    public ClientRepository(ApplicationDbContext context, ICacheService cacheService) : base(context)
    {
        _cacheService = cacheService;
    }

    public async Task<List<ClientOrderResponse>> GetOrdersAsync(Guid id)
    {
        return await _cacheService
            .GetAsync(
            $"client-{id}-orders",
            async () =>
            {
                List<ClientOrderResponse> clientOrders = await _context
                    .Set<Order>()
                    .AsNoTracking()
                    .Include(o => o.Dishes)
                    .AsSplitQuery()
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

                return clientOrders;
            }) ?? [];
    }
}
