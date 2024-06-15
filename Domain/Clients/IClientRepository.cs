using Domain.Clients.Responses;

namespace Domain.Clients;

public interface IClientRepository
{
    void Add(Client client);
    Task<Client?> GetAsync(Guid id);
    Task<List<ClientOrderResponse>> GetOrdersAsync(Guid id);
    Task<Client?> GetByPhoneAsync(Phone phone);
}
