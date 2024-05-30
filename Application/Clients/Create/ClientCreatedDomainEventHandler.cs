using Application.Abstractions;
using Domain.Clients;
using MediatR;

namespace Application.Clients.Create;

internal class ClientCreatedDomainEventHandler : INotificationHandler<ClientCreatedDomainEvent>
{
    private readonly IEmailService _emailService;
    private readonly IClientRepository _clientRepository;

    public ClientCreatedDomainEventHandler(IEmailService emailService, IClientRepository clientRepository)
    {
        _emailService = emailService;
        _clientRepository = clientRepository;
    }

    public async Task Handle(ClientCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetAsync(notification.Id);

        if (client is null)
        {
            return;
        }

        await _emailService.SendEmail(client.Name.Value, "Welcome to EasyRestaurant", "Glad you join it's gonna be awesome");
    }
}
