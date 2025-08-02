using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using static HCM.Common.ValidaitonConstants.Employee;

namespace HCM.Data.Models
{
    public class Employee
    {
        public Employee()
        {
            ManagedDepartments = new HashSet<DepartmentManager>();
            Users = new HashSet<ApplicationUser>();
            //IsDeleted = false;
        }

        [Key]
        public Guid Id { get; set; }

        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [MaxLength(JobTitleMaxLength)]
        public string JobTitle { get; set; } = null!;

        public decimal Salary { get; set; }

        [ForeignKey(nameof(Department))]
        public Guid DepartmentId { get; set; }

        //public bool IsDeleted { get; set; } = false;

        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<DepartmentManager> ManagedDepartments { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
