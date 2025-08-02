using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCM.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            UsersRoles = new HashSet<ApplicationUserRole>();
            IsPasswordChanged = false;
            //IsDeleted = false;
        }

        [ForeignKey(nameof(Employee))]
        public Guid? EmployeeId { get; set; }
        
        public bool IsPasswordChanged { get; set; }
        //public bool IsDeleted { get; set; } 

        public virtual Employee? Employee { get; set; }
        public virtual ICollection<ApplicationUserRole> UsersRoles { get; set; } = null!;
    }
}
