using HCM.Services.Data.Contracts;
using HCM.Web.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using X.PagedList.Extensions;
using static HCM.Common.HCMConstants.RoleConstants;
using static HCM.Common.HCMConstants.GeneralConstants;
using Microsoft.AspNetCore.Identity;
using HCM.Data.Models;
using static HCM.Common.ValidaitonConstants;
using HCM.Web.ViewModels.DepartmentManagerService;

namespace HCM.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IDepartmentService departmentService;
        private readonly IDepartmentManagerService departmentManagerService;
        private readonly IRoleService roleService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;
        private readonly IUserEmailStore<ApplicationUser> emailStore;

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService, IRoleService roleService, UserManager<ApplicationUser> userManager,IUserStore<ApplicationUser> userStore, IDepartmentManagerService departmentManagerService)
        {
            this.employeeService = employeeService;
            this.departmentService = departmentService;
            this.roleService = roleService;
            this.userManager = userManager;
            this.userStore = userStore;
            emailStore = (IUserEmailStore<ApplicationUser>)userStore;
            this.departmentManagerService = departmentManagerService;
        }

        [Authorize(Roles = $"{EmployeeRoleName},{ManagerRoleName}")]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await employeeService.GetByIdAsync(userId!);
            return View(model);
        }

        [Authorize(Roles = $"{ManagerRoleName},{HRAdminRoleName}")]
        public async Task<IActionResult> All([FromQuery] EmployeeAllViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var isAdmin = User.IsInRole(HRAdminRoleName);

            var employees = await employeeService.AllAsync(model, userId, isAdmin);

            model.Employees = employees.ToPagedList(model.CurrentPage, EntitiesPerPage);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await departmentService.AllAsync(); 
            ViewBag.Roles = await roleService.AllAsync();
            return View();
        }

        [Authorize(Roles = HRAdminRoleName)]
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeFormModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await departmentService.AllAsync();
                ViewBag.Roles = await roleService.AllAsync();
                return View(model);
            }

            var user = new ApplicationUser();

            var employeeId = await employeeService.CreateAsync(model);

            await departmentManagerService.CreateAsync(new DepartmentManagerViewModel
            {
                DepartmentId = model.DepartmentId,
                ManagerId = employeeId.ToString()
            });

            user.UsersRoles.Add(new ApplicationUserRole
            {
                RoleId = Guid.Parse(model.RoleId),
            });

            user.EmployeeId = employeeId;
          
            await userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
            await userManager.CreateAsync(user, model.Password);

            return RedirectToAction("All");
        }


        [Authorize(Roles = HRAdminRoleName)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await departmentManagerService.DeleteAsync(id);
            await employeeService.DeleteAsync(id);
            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await employeeService.GetEditAsync(id);
            ViewBag.Departments = await departmentService.AllAsync();
            ViewBag.Roles = await roleService.AllAsync();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeFormModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await departmentService.AllAsync();
                ViewBag.Roles = await roleService.AllAsync();
                return View(model);
            }

            try
            {
                await employeeService.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Failed to update employee: {ex.Message}");
                ViewBag.Departments = await departmentService.AllAsync();
                ViewBag.Roles = await roleService.AllAsync();
                return View(model);
            }

            return RedirectToAction("All");
        }


    }
}
