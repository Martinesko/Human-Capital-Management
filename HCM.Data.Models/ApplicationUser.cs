using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCM.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [ForeignKey(nameof(Employee))]
        public Guid? EmployeeId { get; set; }
        
        public bool IsPasswordChanged { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual ICollection<ApplicationUserRole> UsersRoles { get; set; } = null!;
    }
}
