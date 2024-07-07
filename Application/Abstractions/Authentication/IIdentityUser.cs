namespace Application.Abstractions.Authentication;

public interface IIdentityUser
{
    Task<string> RegisterUserAsync(string email, string password);
}