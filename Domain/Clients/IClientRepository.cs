namespace Domain.Clients;

public interface IClientRepository
{
    void Add(Client client);
    Task<Client?> GetAsync(Guid id);
}
