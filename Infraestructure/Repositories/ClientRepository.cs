using Domain.Clients;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

internal class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;

    public ClientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Client client)
    {
        _context.Set<Client>().Add(client);
    }

    public async Task<Client?> GetAsync(Guid id)
    {
        return await _context.Set<Client>().FirstOrDefaultAsync(c => c.Id == id);
    }
}
