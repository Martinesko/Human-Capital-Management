using System.ComponentModel.DataAnnotations;
using static HCM.Common.ValidaitonConstants.Employee;

public class CreateEmployeeViewModel : IValidatableObject
{
    public string? Id { get; set; }

    [Required]
    [MaxLength(FirstNameMaxLength)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(LastNameMaxLength)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(EmailMaxLength)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(JobTitleMaxLength)]
    public string JobTitle { get; set; } = string.Empty;

    [Required]
    public decimal Salary { get; set; }

    [Required]
    public Guid Department { get; set; }

    [Required]
    public string Role { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [MinLength(PasswordMinLength)]
    [MaxLength(PasswordMaxLength)]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string? ConfirmPassword { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Id))
        {
            if (string.IsNullOrWhiteSpace(Password))
                yield return new ValidationResult("The Password field is required.", new[] { nameof(Password) });
            if (string.IsNullOrWhiteSpace(ConfirmPassword))
                yield return new ValidationResult("The ConfirmPassword field is required.", new[] { nameof(ConfirmPassword) });
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(Password) && Password != ConfirmPassword)
                yield return new ValidationResult("Passwords do not match.", new[] { nameof(ConfirmPassword) });
        }
    }
}
