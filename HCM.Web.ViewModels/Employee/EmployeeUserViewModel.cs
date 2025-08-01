using System.ComponentModel.DataAnnotations;
using static HCM.Common.ValidaitonConstants.Employee;

namespace HCM.Web.ViewModels.Employee
{
    public class EmployeeUserViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(JobTitleMaxLength)]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public string Department { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(EmailMaxLength)]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        public string UserRole { get; set; } = string.Empty;

        [MaxLength(PasswordMaxLength)]
        public string? UserPassword { get; set; }
    }
}
