using Finshark.Models;

namespace Finshark.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
