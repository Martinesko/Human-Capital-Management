using System.ComponentModel.DataAnnotations;
using static HCM.Common.ValidaitonConstants.Employee;

namespace HCM.Web.ViewModels.Employee
{
    public class EmployeeCreateFormModel
    {
        public string? Id  { get; set; }

        [MaxLength(FirstNameMaxLength)]
        [Display(Name = FirstNameDisplayName)]
        public string FirstName { get; set; } = null!;

        [MaxLength(LastNameMaxLength)]
        [Display(Name = LastNameDisplayName)]
        public string LastName { get; set; } = null!;

        [EmailAddress]
        [MaxLength(EmailMaxLength)]
        public string Email { get; set; } = null!;

        [MaxLength(JobTitleMaxLength)]
        [Display(Name = JobTitleDisplayName)]
        public string JobTitle { get; set; } = null!;

        public decimal Salary { get; set; }

        [Display(Name = DepartmentDisplayName)]
        public string DepartmentId { get; set; } = null!;

        [Display(Name = RoleDisplayName)]
        public string RoleName { get; set; } = null!;

        [DataType(DataType.Password)]
        [MinLength(PasswordMinLength)]
        [MaxLength(PasswordMaxLength)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = ConfirmPasswordDisplayName)]
        [Compare(nameof(Password), ErrorMessage = PasswordsDontMatch)]
        public string ConfirmPassword { get; set; } = null!;
    }
}