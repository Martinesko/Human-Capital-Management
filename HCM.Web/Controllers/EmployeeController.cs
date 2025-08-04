using HCM.Services.Data.Contracts;
using HCM.Web.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Identity;
using HCM.Data.Models;
using static HCM.Common.HCMConstants.RoleConstants;
using static HCM.Common.HCMConstants.GeneralConstants;

namespace HCM.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IDepartmentService departmentService;
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;
        private readonly IUserEmailStore<ApplicationUser> emailStore;

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, IUserService userService)
        {
            this.employeeService = employeeService;
            this.departmentService = departmentService;
            this.userManager = userManager;
            this.userStore = userStore;
            emailStore = (IUserEmailStore<ApplicationUser>)userStore;
            this.userService = userService;
        }

        [Authorize(Roles = $"{EmployeeRoleName},{ManagerRoleName}")]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var model = await userService.GetProfileInfoAsync(userId);
            return View(model);
        }

        [Authorize(Roles = $"{ManagerRoleName},{HRAdminRoleName}")]
        public async Task<IActionResult> All([FromQuery] EmployeeAllViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var isAdmin = User.IsInRole(HRAdminRoleName);

            var employees = await employeeService.GetAllAsync(model, userId, isAdmin);

            model.Employees = employees.ToPagedList(model.CurrentPage, EntitiesPerPage);

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = HRAdminRoleName)]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await departmentService.GetAllAsync();
            ViewBag.Roles = new List<string>() { EmployeeRoleName, ManagerRoleName };
            return View();
        }

        [HttpPost]
        [Authorize(Roles = HRAdminRoleName)]
        public async Task<IActionResult> Create(EmployeeCreateFormModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await departmentService.GetAllAsync();
                ViewBag.Roles = new List<string>() { EmployeeRoleName, ManagerRoleName };
                return View(model);
            }

            var user = new ApplicationUser
            {
                Employee = new Employee()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    JobTitle = model.JobTitle,
                    Salary = model.Salary,
                    DepartmentId = Guid.Parse(model.DepartmentId)
                }
            };

            if (model.RoleName == ManagerRoleName)
            {
                user.Employee.ManagedDepartments.Add(new DepartmentManager() { DepartmentId = Guid.Parse(model.DepartmentId) });
            }

            await userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
            await userManager.CreateAsync(user, model.Password);
            await userManager.AddToRoleAsync(user, model.RoleName);

            return RedirectToAction("All");
        }

        [HttpPost]
        [Authorize(Roles = HRAdminRoleName)]
        public async Task<IActionResult> Delete(string userId, string employeeId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (userId == currentUserId)
            {
                return Unauthorized();
            }

            var user = await userManager.FindByIdAsync(userId);
            await userManager.DeleteAsync(user!);
            await employeeService.DeleteAsync(employeeId);
            return RedirectToAction("All");
        }

        [HttpGet]
        [Authorize(Roles = $"{ManagerRoleName},{HRAdminRoleName}")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await userService.GetByEmployeeIdAsync(id);

            if (await userManager.IsInRoleAsync(user, ManagerRoleName) && User.IsInRole(ManagerRoleName))
            {
                return Unauthorized();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var IsManagingEmployee = await userService.IsManagingEmployeeAsync(userId, id);

            if (!User.IsInRole(HRAdminRoleName) && !IsManagingEmployee)
            {
                return Unauthorized();
            }

            var model = await employeeService.GetEditAsync(id);

            ViewBag.Departments = await departmentService.GetAllAsync();
            ViewBag.Roles = new List<string>() { EmployeeRoleName, ManagerRoleName };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = $"{ManagerRoleName},{HRAdminRoleName}")]
        public async Task<IActionResult> Edit(EmployeeEditFormModel model)
        {
            var user = await userService.GetByEmployeeIdAsync(model.Id);

            if (await userManager.IsInRoleAsync(user, ManagerRoleName) && User.IsInRole(ManagerRoleName))
            {
                return Unauthorized();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var IsManagingEmployee = await userService.IsManagingEmployeeAsync(userId, model.Id);

            if (!User.IsInRole(HRAdminRoleName) && !IsManagingEmployee)
            {
                return Unauthorized();
            }

            if (!User.IsInRole(HRAdminRoleName))
            {
                ModelState.Remove(nameof(model.Email));
                ModelState.Remove(nameof(model.DepartmentId));
                ModelState.Remove(nameof(model.RoleName));
            }

            if (!ModelState.IsValid)
            {
                model = await employeeService.GetEditAsync(model.Id);

                ViewBag.Departments = await departmentService.GetAllAsync();
                ViewBag.Roles = new List<string>() { EmployeeRoleName, ManagerRoleName };

                return View(model);
            }

            user!.Employee!.FirstName = model.FirstName;
            user!.Employee!.LastName = model.LastName;
            user!.Employee!.JobTitle = model.JobTitle;
            user!.Employee!.Salary = model.Salary;

            if (User.IsInRole(HRAdminRoleName))
            {

                if (user!.Employee!.DepartmentId != Guid.Parse(model.DepartmentId))
                {
                    user.Employee.ManagedDepartments.Clear();
                }

                user!.Employee!.DepartmentId = Guid.Parse(model.DepartmentId);

                if (model.RoleName == ManagerRoleName && !await userManager.IsInRoleAsync(user, ManagerRoleName))
                {
                    user.Employee.ManagedDepartments.Add(new DepartmentManager() { DepartmentId = Guid.Parse(model.DepartmentId) });
                }

                if (model.RoleName == EmployeeRoleName)
                {
                    user.Employee.ManagedDepartments.Clear();
                }

                await userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                await emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    await userManager.ResetPasswordAsync(user, token, model.Password);
                }

                var currentRoles = await userManager.GetRolesAsync(user);

                if (!currentRoles.Contains(model.RoleName))
                {
                    await userManager.RemoveFromRolesAsync(user, currentRoles);
                    await userManager.AddToRoleAsync(user, model.RoleName);
                }
            }

            await userManager.UpdateAsync(user);

            return RedirectToAction("All", "Employee");
        }
    }
}
