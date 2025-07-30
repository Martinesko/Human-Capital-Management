using HCM.Data;
using HCM.Services.Data.Contracts;
using HCM.Web.ViewModels.Employee;
using Microsoft.EntityFrameworkCore;

namespace HCM.Services.Data
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HcmDbContext context;

        public EmployeeService(HcmDbContext context)
        {
            this.context = context;
        }
        public EmployeeInfoViewModel? GetEmployeeInfoByUserId(string userId)
        {
            var employeeId = context.Users.FirstOrDefault(u => u.Id.ToString() == userId)?.EmployeeId;
            if (employeeId != null)
            {
                var employeeInfoViewModel = context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == employeeId);
                return new EmployeeInfoViewModel
                {
                    FullName = $"{employeeInfoViewModel?.FirstName ?? string.Empty} {employeeInfoViewModel?.LastName ?? string.Empty}",
                    Email = employeeInfoViewModel?.Email ?? string.Empty,
                    JobTitle = employeeInfoViewModel?.JobTitle ?? string.Empty,
                    Salary = employeeInfoViewModel?.Salary ?? 0,
                    Department = employeeInfoViewModel?.Department.Name ?? string.Empty
                };
            }
            return null;
        }
    }
}
