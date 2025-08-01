using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HCM.Common.ValidaitonConstants.Employee;

namespace HCM.Data.Models
{
    public class Employee
    {
        public Employee()
        {
            ManagedDepartments = new HashSet<DepartmentManager>();
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
        
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<DepartmentManager> ManagedDepartments { get; set; }
    }
}
