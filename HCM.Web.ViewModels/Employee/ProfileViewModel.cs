namespace HCM.Web.ViewModels.Employee
{
    public class ProfileViewModel
    {
        public string FullName { get; set; } = null!;
        public string JobTitle { get; set; } = null!;
        public string Email { get; set; } = null!;
        public decimal Salary { get; set; }
        public string Department { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool IsPasswordChanged { get; set; }
    }
}
