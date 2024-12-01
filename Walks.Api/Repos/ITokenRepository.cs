using Microsoft.AspNetCore.Identity;

namespace Walks.Api.Repos
{
    public interface ITokenRepository
    {
        string CreateToken(IdentityUser user, List<string> roles); 
    }
}
