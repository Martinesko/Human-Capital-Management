using System.ComponentModel.DataAnnotations;
using static HCM.Common.ValidaitonConstants.Department;

namespace HCM.Web.ViewModels.Department
{
    public class DepartmentFormModel
    {
        public string? Id { get; set; } = null!;

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
