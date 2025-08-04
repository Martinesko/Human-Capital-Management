using HCM.Data;
using HCM.Data.Models;
using HCM.Services.Data.Contracts;
using HCM.Web.ViewModels.User;
using Microsoft.EntityFrameworkCore;

namespace HCM.Services.Data
{
    public class UserService : IUserService
    {
        private readonly HcmDbContext context;

        public UserService(HcmDbContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<ApplicationUser>> GetAllByDepartmentAsync(string id)
        {
            return await context.Users
                 .Where(u => u.Employee!.DepartmentId == Guid.Parse(id))
                 .ToListAsync();
        }

        public async Task<ApplicationUser> GetByEmployeeIdAsync(string id)
        {
            return await context.Users
                .Include(u => u.Employee)
                .ThenInclude(e => e.ManagedDepartments)
                .Where(u => u.EmployeeId == Guid.Parse(id))
                .FirstAsync();
        }

        public async Task<ProfileViewModel> GetProfileInfoAsync(string id)
        {
            return await context.Users
                .AsNoTracking()
                .Where(u => u.Id == Guid.Parse(id))
                .Select(u => new ProfileViewModel()
                {
                    FullName = $"{u.Employee!.FirstName} {u.Employee.LastName}",
                    JobTitle = u.Employee.JobTitle,
                    Salary = u.Employee.Salary,
                    Department = u.Employee.Department.Name,
                    Email = u.Email!,
                    Role = u.UsersRoles.First().Role.Name!
                }).FirstAsync();
        }

        public async Task<bool> IsManagingEmployeeAsync(string userId, string employeeId)
        {
            return await context.Users
                .AnyAsync(u => u.Id == Guid.Parse(userId) && u.Employee!.ManagedDepartments.Any(md => md.Department.Employees.Any(e => e.Id == Guid.Parse(employeeId))));
        }
    }
}
