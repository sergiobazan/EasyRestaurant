using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Infraestructure.Authentication;

public class JwtBearerConfigurationSetup : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly JwtOptions _options;

    public JwtBearerConfigurationSetup(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        options.Audience = _options.Audience;
        options.Authority = _options.Issuer;
        options.TokenValidationParameters.ValidIssuer = _options.Issuer;
    }
}