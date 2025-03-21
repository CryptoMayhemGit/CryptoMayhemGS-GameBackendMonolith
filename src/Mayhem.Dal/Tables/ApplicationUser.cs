using Microsoft.AspNetCore.Identity;

namespace Mayhem.Dal.Tables
{
    public class ApplicationUser : IdentityUser
    {
        public int UserId { get; set; }
    }
}
