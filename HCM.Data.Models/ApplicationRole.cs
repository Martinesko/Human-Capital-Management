using Microsoft.AspNetCore.Identity;

namespace HCM.Data.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public virtual ICollection<ApplicationUserRole> UsersRoles { get; set; } = null!;
    }
}
