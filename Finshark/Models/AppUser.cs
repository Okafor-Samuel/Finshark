using Microsoft.AspNetCore.Identity;

namespace Finshark.Models
{
    public class AppUser : IdentityUser
    {
       public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}
