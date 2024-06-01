using Domain.Clients;
using Domain.Clients.Responses;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infraestructure.Repositories;

internal class ClientRepository : Repository<Client>, IClientRepository
{
    private readonly IDistributedCache _cache;
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        ContractResolver = new PrivateConstructorContractResolver(),
    };

    public ClientRepository(ApplicationDbContext context, IDistributedCache cache) : base(context)
    {
        _cache = cache;
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

    public override async Task<Client?> GetAsync(Guid id)
    {
        var key = $"client-{id}";

        var cachedClient = await _cache.GetStringAsync(key);

        Client? client;

        if (string.IsNullOrEmpty(cachedClient))
        {
            client = await base.GetAsync(id);

            if (client is null)
            {
                return client;
            }

            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(client), new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(1),
            });

            return client;
        }

        client = JsonConvert.DeserializeObject<Client>(cachedClient, SerializerSettings);

        if (client is not null)
        {
            _context.Set<Client>().Attach(client);
            _context.Entry(client).State = EntityState.Unchanged;
        }

        return client;
    }
}
