using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Abstractions;

namespace Application.Clients.Login;

public class LoginClientCommandHandler : ICommandHandler<LoginClientCommand, string>
{
    private readonly IJwtService _jwtService;

    public LoginClientCommandHandler(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public async Task<Result<string>> Handle(LoginClientCommand request, CancellationToken cancellationToken)
    {
        return await _jwtService.GetJwtAsync(request.Email, request.Password);
    }
}