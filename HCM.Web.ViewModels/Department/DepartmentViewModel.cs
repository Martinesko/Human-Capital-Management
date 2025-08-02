using System.ComponentModel.DataAnnotations;
using static HCM.Common.ValidaitonConstants.Department;

namespace HCM.Web.ViewModels.Department
{
    public class DepartmentViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
