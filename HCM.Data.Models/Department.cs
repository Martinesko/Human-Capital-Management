using System.ComponentModel.DataAnnotations;
using static HCM.Common.ValidaitonConstants.Department;

namespace HCM.Data.Models
{
    public class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
            Managers = new HashSet<DepartmentManager>();
        }

        [Key]
        public Guid Id { get; set; }
      
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<DepartmentManager> Managers { get; set; }
    }
}
