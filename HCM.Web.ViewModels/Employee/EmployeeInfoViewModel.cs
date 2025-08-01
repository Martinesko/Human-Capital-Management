using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static HCM.Common.ValidaitonConstants.Employee;

namespace HCM.Web.ViewModels.Employee
{
    public class EmployeeInfoViewModel
    {
        [Required]
        [MaxLength(FirstNameMaxLength + LastNameMaxLength + 1)]
        public string? FullName { get; set; }

        [Required]
        [MaxLength(JobTitleMaxLength)]
        public string? JobTitle { get; set; }

        [Required]
        [MaxLength(EmailMaxLength)]
        public string? Email { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public string? Department { get; set; }

        [Required]
        public string? UserRole { get; set; }
        [Required]
        public bool IsPasswordChanged { get; set; }
    }
}
