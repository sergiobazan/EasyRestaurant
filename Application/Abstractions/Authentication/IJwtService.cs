namespace Application.Abstractions.Authentication;

public interface IJwtService
{
    Task<string> GetJwtAsync(string email, string password);
}