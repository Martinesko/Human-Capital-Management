using HCM.Services.Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using X.PagedList.Extensions;
using static HCM.Common.HCMConstants.RoleConstants;

namespace HCM.Web.Controllers
{
    [Authorize(Roles = $"{HRAdminRoleName},{ManagerRoleName}")]
    public class EmployeeController : Controller
    {
        private IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {

            int pageSize = 5;
            var employees = await employeeService.GetAllEmployeesAync();

            if (User.IsInRole(ManagerRoleName))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var managerInfo = employeeService.GetEmployeeInfoByUserId(userId);
                var departmentName = managerInfo?.Department;

                employees = employees.Where(e => e.Department == departmentName).ToList();

                ViewBag.DepartmentName = departmentName;
                ViewBag.IsManager = true;
            }
            else
            {
                ViewBag.DepartmentName = "All Employees";
                ViewBag.IsManager = false;
            }

            var pagedEmployees = employees.ToPagedList(page, pageSize);
            return View(pagedEmployees);
        }

        [Authorize(Roles = HRAdminRoleName)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeViewModel model)
        {
            ViewBag.Departments = await employeeService.GetAllDepartmentsAsync();
            ViewBag.Roles = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "Manager", Text = "Manager" },
                        new SelectListItem { Value = "Employee", Text = "Employee" }
                    };
            if (ModelState.IsValid)
            {
                var result = await employeeService.Create(model);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create employee. Please check your input or try again later.");
                    return View(model);
                }
                return RedirectToAction("Index", "Employee");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await employeeService.GetAllDepartmentsAsync();
            ViewBag.Departments = departments;
            ViewBag.Roles = new List<SelectListItem>
            {
            new SelectListItem { Value = "Manager", Text = "Manager" },
            new SelectListItem { Value = "Employee", Text = "Employee" }
            };
            return View();
        }


        [Authorize(Roles = HRAdminRoleName)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await employeeService.DeleteEmployeeAndUserAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await employeeService.GetEmployeeForEditAsync(id);
            ViewBag.Departments = await employeeService.GetAllDepartmentsAsync();
            ViewBag.Roles = new List<SelectListItem>
            {
              new SelectListItem { Value = "Manager", Text = "Manager" },
              new SelectListItem { Value = "Employee", Text = "Employee" }
            };
            return View("Create", model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(CreateEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await employeeService.GetAllDepartmentsAsync();
                ViewBag.Roles = new List<SelectListItem>
        {
            new SelectListItem { Value = "Manager", Text = "Manager" },
            new SelectListItem { Value = "Employee", Text = "Employee" }
        };
                return View("Create", model);
            }

            try
            {
                await employeeService.UpdateEmployeeAsync(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Failed to update employee: {ex.Message}");
                ViewBag.Departments = await employeeService.GetAllDepartmentsAsync();
                ViewBag.Roles = new List<SelectListItem>
        {
            new SelectListItem { Value = "Manager", Text = "Manager" },
            new SelectListItem { Value = "Employee", Text = "Employee" }
        };
                return View("Create", model);
            }

            return RedirectToAction("Index");
        }


    }
}
