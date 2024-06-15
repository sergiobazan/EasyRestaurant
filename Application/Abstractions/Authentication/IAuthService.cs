using Domain.Clients;

namespace Application.Abstractions.Authentication;

public interface IAuthService
{
    string GenerateToken(Client client, CancellationToken cancellationToken = default);
}
