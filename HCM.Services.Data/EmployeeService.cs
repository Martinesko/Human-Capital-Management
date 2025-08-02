using HCM.Data;
using HCM.Data.Models;
using HCM.Services.Data.Contracts;
using HCM.Web.ViewModels.Department;
using HCM.Web.ViewModels.Employee;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HCM.Services.Data
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HcmDbContext context;
        private readonly UserManager<ApplicationUser> userManager;


        public EmployeeService(HcmDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<ProfileViewModel> GetByIdAsync(string userId)
        {
            return await context.Users
            .Include(u => u.Employee)
            .ThenInclude(e => e!.Department)
            .Include(u => u.UsersRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.Id == Guid.Parse(userId))
            .Select(u => new ProfileViewModel()
            {
                FullName = $"{u.Employee!.FirstName} {u.Employee.LastName}",
                JobTitle = u.Employee.JobTitle,
                Salary = u.Employee.Salary,
                Department = u.Employee.Department.Name,
                Email = u.Email!,
                Role = u.UsersRoles.First().Role.Name!,
                IsPasswordChanged = u.IsPasswordChanged
            }).FirstAsync();
        }
        public async Task<ICollection<EmployeeViewModel>> AllAsync(EmployeeAllViewModel model, string id, bool isAdmin)
        {
            ICollection<EmployeeViewModel> employees = await context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .ThenInclude(d => d.Managers)
                .Include(e => e.Users)
                .ThenInclude(u => u.UsersRoles)
                .ThenInclude(ur => ur.Role)
                .Where(e => isAdmin || e.Department.Managers.Any(m => m.Manager.Users.First().Id == Guid.Parse(id)))
                .Select(e => new EmployeeViewModel
                {
                    Id = e.Id.ToString(),
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Role = e.Users.First().UsersRoles.First().Role.Name!,
                    Email = e.Users.First().Email!,
                })
                .ToListAsync();


            return employees;
        }
        public async Task<Guid> CreateAsync(EmployeeFormModel model)
        {
            var employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                JobTitle = model.JobTitle,
                Salary = model.Salary,
                DepartmentId = Guid.Parse(model.DepartmentId),
            };

            await context.AddAsync(employee);
            await context.SaveChangesAsync();

            return employee.Id;
        }
        public async Task DeleteAsync(string userId)
        {
            var user = await context.Users.FirstAsync(u => u.EmployeeId == Guid.Parse(userId));
            var employee = await context.Employees.FirstAsync(e => e.Id == user!.EmployeeId);

            context.Employees.Remove(employee);

            await userManager.DeleteAsync(user);
            await context.SaveChangesAsync();
        }
        public async Task<EmployeeFormModel> GetEditAsync(string userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) return null;

            var employee = await context.Employees.FirstOrDefaultAsync(e => e.Id == user.EmployeeId);
            if (employee == null) return null;

            return new EmployeeFormModel
            {
                Id = user.Id.ToString(),
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = user.Email,
                JobTitle = employee.JobTitle,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId.ToString(),
                RoleId = (await userManager.GetRolesAsync(user)).FirstOrDefault(),
                Password = "",
                ConfirmPassword = ""
            };
        }
        public async Task UpdateAsync(EmployeeFormModel model)
        {
            var user = await context.Users.FirstAsync(u => u.Id.ToString() == model.Id);
            var employee = await context.Employees.FirstAsync(e => e.Id == user.EmployeeId);
            
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.JobTitle = model.JobTitle;
            employee.Salary = model.Salary;
            employee.DepartmentId = Guid.Parse(model.DepartmentId);

            user.Email = model.Email;
            user.UserName = model.Email;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.ResetPasswordAsync(user, token, model.Password);
            }

            var currentRoles = await userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(model.RoleId))
            {
                await userManager.RemoveFromRolesAsync(user, currentRoles);
                await userManager.AddToRoleAsync(user, model.RoleId);
            }

            await context.SaveChangesAsync();

        }
    }
}
