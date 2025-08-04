using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCM.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            UsersRoles = new HashSet<ApplicationUserRole>();
        }

        [ForeignKey(nameof(Employee))]
        public Guid? EmployeeId { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual ICollection<ApplicationUserRole> UsersRoles { get; set; } = null!;
    }
}
