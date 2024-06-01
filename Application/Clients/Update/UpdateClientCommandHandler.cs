using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Clients;
using Domain.Shared;

namespace Application.Clients.Update;

internal class UpdateClientCommandHandler : ICommandHandler<UpdateClientCommand, Guid>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetAsync(request.Id);

        if (client is null)
        {
            return Result.Failure<Guid>(ClientErrors.ClientNotFound(request.Id));
        }

        var phone = Phone.Create(client.Phone.Prefix, request.Client.Phone);

        if (phone.IsFailure)
        {
            return Result.Failure<Guid>(phone.Error);
        }

        client.Update(
            new Name(request.Client.Name),
            phone.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return client.Id;
    }
}
