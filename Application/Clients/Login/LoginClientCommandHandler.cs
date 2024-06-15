using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Clients;

namespace Application.Clients.Login;

internal class LoginClientCommandHandler : ICommandHandler<LoginClientCommand, string>
{
    private readonly IClientRepository _clientRepository;
    private readonly IAuthService _authService;

    public LoginClientCommandHandler(IClientRepository clientRepository, IAuthService authService)
    {
        _clientRepository = clientRepository;
        _authService = authService;
    }

    public async Task<Result<string>> Handle(LoginClientCommand request, CancellationToken cancellationToken)
    {
        var phone = Phone.Create(request.Login.Prefix, request.Login.Phone);

        var client = await _clientRepository.GetByPhoneAsync(phone.Value);

        if (client is null)
        {
            return Result.Failure<string>(new Error("Auth", "Authenticated"));
        }

        var token = _authService.GenerateToken(client);

        return token;
    }
}
