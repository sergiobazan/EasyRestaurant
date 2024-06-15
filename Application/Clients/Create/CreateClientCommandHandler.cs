using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Clients;
using Domain.Shared;

namespace Application.Clients.Create;

public sealed class CreateClientCommandHandler : ICommandHandler<CreateClientCommand, string>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public CreateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork, IAuthService authService)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public async Task<Result<string>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var phone = Phone.Create(request.Client.Prefix, request.Client.Phone);

        if (phone.IsFailure)
        {
            return Result.Failure<string>(phone.Error);
        }
        
        var gender = Gender.Create(request.Client.Gender);

        if (gender.IsFailure)
        {
            return Result.Failure<string>(gender.Error);
        }

        var client = Client.Create(
            new Name(request.Client.Name), 
            phone.Value,
            gender.Value);

        _clientRepository.Add(client.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var jwtToken = _authService.GenerateToken(client.Value, cancellationToken);

        return jwtToken;
    }
}
