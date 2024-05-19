using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Clients;
using Domain.Shared;

namespace Application.Clients.Create;

public sealed class CreateClientCommandHandler : ICommandHandler<CreateClientCommand, Guid>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var phone = Phone.Create(request.Client.Prefix, request.Client.Phone);

        if (phone.IsFailure)
        {
            return Result.Failure<Guid>(phone.Error);
        }
        
        var gender = Gender.Create(request.Client.Gender);

        if (gender.IsFailure)
        {
            return Result.Failure<Guid>(gender.Error);
        }

        var client = Client.Create(
            new Name(request.Client.Name), 
            phone.Value,
            gender.Value);

        _clientRepository.Add(client.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(client.Value.Id);
    }
}
