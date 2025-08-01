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
        public EmployeeInfoViewModel? GetEmployeeInfoByUserId(string userId)
        {
            var user = context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null) {
                return null;
            }
            var employeeId = user?.EmployeeId;
            var role = userManager.GetRolesAsync(user).Result.FirstOrDefault();
            

            if (employeeId != null)
            {
                var employeeInfoViewModel = context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == employeeId);
                return new EmployeeInfoViewModel
                {
                    FullName = $"{employeeInfoViewModel?.FirstName ?? string.Empty} {employeeInfoViewModel?.LastName ?? string.Empty}",
                    JobTitle = employeeInfoViewModel?.JobTitle ?? string.Empty,
                    Salary = employeeInfoViewModel?.Salary ?? 0,
                    Department = employeeInfoViewModel?.Department.Name ?? string.Empty,
                    Email = user?.Email ?? string.Empty,
                    UserRole = role,
                    IsPasswordChanged = user.IsPasswordChanged
                };
            }
            return null;
        } 
        public async Task<List<EmployeeUserViewModel>> GetAllEmployeesAync()
        {
            var employees = await context.Employees
                .Include(e => e.Department)
                .ToListAsync();

            var result = new List<EmployeeUserViewModel>();

            foreach (var e in employees)
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.EmployeeId == e.Id);
                IList<string> roles = new List<string>();
                string userId = string.Empty;
                string userPassword = string.Empty;
                string userEmail = string.Empty;

                if (user != null)
                {
                    roles = await userManager.GetRolesAsync(user);
                    userId = user.Id.ToString();
                    userPassword = user.PasswordHash ?? string.Empty;
                    userEmail = user.Email ?? string.Empty;
                }

                result.Add(new EmployeeUserViewModel
                {
                    Id = e.Id.GetHashCode(),
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    UserId = userId,
                    UserPassword = userPassword,
                    UserRole = roles[0],
                    UserEmail = userEmail
                });
            }

            return result;
        }
        public async Task<bool> Create(CreateEmployeeViewModel model)
        {
            var Employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                JobTitle = model.JobTitle,
                Salary = model.Salary,
                DepartmentId = model.Department,
            };
            context.Employees.Add(Employee);
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmployeeId = Employee.Id
            };
            var isCreated = await userManager.CreateAsync(user, model.Password);
            if (!isCreated.Succeeded)
            {
                return false;
            }
            await userManager.AddToRoleAsync(user, model.Role);

            await context.SaveChangesAsync();
            return true;
        }
        public async Task<List<DepartmentViewModel>> GetAllDepartmentsAsync()
        {
            return await context.Departments
                .Select(d => new DepartmentViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();
        }
        public async Task DeleteEmployeeAndUserAsync(string userId)
        {
            var user = context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user != null)
            {
                var employee = context.Employees.FirstOrDefault(e => e.Id == user.EmployeeId);
                if (employee != null)
                {
                    context.Employees.Remove(employee);
                }
                await userManager.DeleteAsync(user);
                await context.SaveChangesAsync();
            }
        }
        public async Task<CreateEmployeeViewModel> GetEmployeeForEditAsync(string userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) return null;

            var employee = await context.Employees.FirstOrDefaultAsync(e => e.Id == user.EmployeeId);
            if (employee == null) return null;

            return new CreateEmployeeViewModel
            {
                Id = user.Id.ToString(),
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = user.Email,
                JobTitle = employee.JobTitle,
                Salary = employee.Salary,
                Department = employee.DepartmentId,
                Role = (await userManager.GetRolesAsync(user)).FirstOrDefault(),
                Password = "", 
                ConfirmPassword = ""
            };
        }
        public async Task UpdateEmployeeAsync(CreateEmployeeViewModel model)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == model.Id);
            if (user == null) return;

            var employee = await context.Employees.FirstOrDefaultAsync(e => e.Id == user.EmployeeId);
            if (employee == null) return;

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.JobTitle = model.JobTitle;
            employee.Salary = model.Salary;
            employee.DepartmentId = model.Department;

            user.Email = model.Email;
            user.UserName = model.Email;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.ResetPasswordAsync(user, token, model.Password);
            }

            var currentRoles = await userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(model.Role))
            {
                await userManager.RemoveFromRolesAsync(user, currentRoles);
                await userManager.AddToRoleAsync(user, model.Role);
            }

            await context.SaveChangesAsync();

        }
    }
}
