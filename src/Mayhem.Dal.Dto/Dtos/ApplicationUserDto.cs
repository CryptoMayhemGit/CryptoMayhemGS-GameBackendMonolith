using Microsoft.AspNetCore.Identity;

namespace Mayhem.Dal.Dto.Dtos
{
    public class ApplicationUserDto : IdentityUser
    {
        public int UserId { get; set; }
    }
}
