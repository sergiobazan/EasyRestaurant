using Application.Abstractions.Authentication;
using Domain.Clients;
using Infraestructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infraestructure.Services;

internal class AuthService : IAuthService
{
    private readonly JwtOptions _jwtOptions;

    public AuthService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(Client client, CancellationToken cancellationToken = default)
    {
        var claims = GetClaims(client);

        var signinKey = GetCredentials();

        var securityToken = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddMinutes(15),
            signinKey);

        var tokenHandler = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return tokenHandler;
    }

    private SigningCredentials GetCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);
    }

    private static List<Claim> GetClaims(Client client)
    {
        List<Claim> claims = new()
        {
            new(JwtRegisteredClaimNames.Sub, client.Id.ToString()),
            new(JwtRegisteredClaimNames.Gender, client.Gender.Value),
            new(JwtRegisteredClaimNames.Name, client.Name.Value),
            new(ClaimTypes.MobilePhone, client.Phone.Value)
        };

        return claims;
    }
}
