using HCM.Data.Models;
using HCM.Services.Data.Contracts;
using HCM.Web.ViewModels.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;
using static HCM.Common.HCMConstants.GeneralConstants;
using static HCM.Common.HCMConstants.RoleConstants;

namespace HCM.Web.Controllers
{
    [Authorize(Roles = HRAdminRoleName)]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService departmentService;
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;
        public DepartmentController(IDepartmentService departmentService, IUserService userService, UserManager<ApplicationUser> userManager)
        {
            this.departmentService = departmentService;
            this.userService = userService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All(DepartmentAllViewModel model)
        {
            var departments = await departmentService.GetAllAsync();
            model.Departments = departments.ToPagedList(model.CurrentPage, EntitiesPerPage);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await departmentService.CreateAsync(model);

            return RedirectToAction("All");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var users = await userService.GetAllByDepartmentAsync(id);

            foreach (var user in users)
            {
                await userManager.DeleteAsync(user!);
            }

            await departmentService.DeleteAsync(id);
            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await departmentService.GetByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await departmentService.GetByIdAsync(model.Id);
                return View(model);
            }

            await departmentService.EditAsync(model);

            return RedirectToAction("All");
        }
    }
}
