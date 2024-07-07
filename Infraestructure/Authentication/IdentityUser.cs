using Application.Abstractions.Authentication;
using FirebaseAdmin.Auth;

namespace Infraestructure.Authentication;

public class IdentityUser : IIdentityUser
{
    public async Task<string> RegisterUserAsync(string email, string password)
    {

        var userArgs = new UserRecordArgs()
        {
            Email = email,
            Password = password
        };

        var user = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

        return user.Uid;
    }
}