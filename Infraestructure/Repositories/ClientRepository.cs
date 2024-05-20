using Domain.Clients;

namespace Infraestructure.Repositories;

internal class ClientRepository : Repository<Client>, IClientRepository
{
    public ClientRepository(ApplicationDbContext context) : base(context)
    {
    }
}
