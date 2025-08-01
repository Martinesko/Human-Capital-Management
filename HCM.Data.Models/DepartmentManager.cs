using System.ComponentModel.DataAnnotations.Schema;

namespace HCM.Data.Models
{
    public class DepartmentManager
    {
        [ForeignKey(nameof(Manager))]
        public Guid ManagerId { get; set; }

        [ForeignKey(nameof(Department))]
        public Guid DepartmentId { get; set; }

        public virtual Employee Manager { get; set; } = null!;
        public virtual Department Department { get; set; } = null!;
    }
}
