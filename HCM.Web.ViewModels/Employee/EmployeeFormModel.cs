using System.ComponentModel.DataAnnotations;
using static HCM.Common.ValidaitonConstants.Employee;

namespace HCM.Web.ViewModels.Employee
{
    public class EmployeeFormModel
    {
        public string? Id { get; set; }

        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [EmailAddress]
        [MaxLength(EmailMaxLength)]
        public string Email { get; set; } = null!;

        [MaxLength(JobTitleMaxLength)]
        public string JobTitle { get; set; } = null!;

        public decimal Salary { get; set; }

        public string DepartmentId { get; set; } = null!;

        public string RoleId { get; set; } = null!;

        [DataType(DataType.Password)]
        [MinLength(PasswordMinLength)]
        [MaxLength(PasswordMaxLength)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = PasswordsDontMatch)]
        public string ConfirmPassword { get; set; } = null!;
    }
}