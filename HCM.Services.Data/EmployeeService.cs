using HCM.Data;
using HCM.Services.Data.Contracts;
using HCM.Web.ViewModels.Employee;
using Microsoft.EntityFrameworkCore;
using static HCM.Common.HCMConstants.RoleConstants;

namespace HCM.Services.Data
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HcmDbContext context;

        public EmployeeService(HcmDbContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<EmployeeViewModel>> GetAllAsync(EmployeeAllViewModel model, string id, bool isAdmin)
        {
            var employees = await context.Employees
                .AsNoTracking()
                .Where(e => isAdmin || (e.Department.Managers.Any(m => m.Manager.User.Id == Guid.Parse(id)) && e.User.UsersRoles.Any(ur => ur.Role.Name == EmployeeRoleName)))
                .Select(e => new EmployeeViewModel
                {
                    EmployeeId = e.Id.ToString(),
                    UserId = e.User.Id.ToString(),
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Role = e.User.UsersRoles.First().Role.Name!,
                    Email = e.User.Email!,
                })
                .ToListAsync();

            return employees;
        }

        public async Task DeleteAsync(string id)
        {
            var employee = await context.Employees
                .Include(e => e.User)
                .ThenInclude(u => u.UsersRoles)
                .Include(e => e.ManagedDepartments)
                .FirstAsync(e => e.Id == Guid.Parse(id));

            context.RemoveRange(employee.ManagedDepartments);
            context.Remove(employee);
            await context.SaveChangesAsync();
        }
        public async Task<EmployeeCreateFormModel> GetCreateAsync(string id)
        {
            return await context.Employees
                .AsNoTracking()
                .Where(e => e.Id == Guid.Parse(id))
                .Select(e => new EmployeeCreateFormModel
                {
                    Id = e.Id.ToString(),
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary,
                    DepartmentId = e.DepartmentId.ToString(),
                    Email = e.User.Email!,
                    RoleName = e.User.UsersRoles.First().Role.Name!
                })
                .FirstAsync();
        }

        public async Task<EmployeeEditFormModel> GetEditAsync(string id)
        {
            return await context.Employees
                .AsNoTracking()
                .Where(e => e.Id == Guid.Parse(id))
                .Select(e => new EmployeeEditFormModel
                {
                    Id = e.Id.ToString(),
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary,
                    DepartmentId = e.DepartmentId.ToString(),
                    Email = e.User.Email!,
                    RoleName = e.User.UsersRoles.First().Role.Name!
                })
                .FirstAsync();
        }
    }
}
