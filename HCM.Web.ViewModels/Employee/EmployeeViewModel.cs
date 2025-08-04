namespace HCM.Web.ViewModels.Employee
{
    public class EmployeeViewModel
    {
        public string EmployeeId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string JobTitle { get; set; } = null!;
        public decimal Salary { get; set; }
        public string Department { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
