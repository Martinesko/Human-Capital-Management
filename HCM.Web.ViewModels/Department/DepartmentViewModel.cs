using System.ComponentModel.DataAnnotations;
using static HCM.Common.ValidaitonConstants.Department;

namespace HCM.Web.ViewModels.Department
{
    public class DepartmentViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = string.Empty;
    }
}
