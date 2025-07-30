using HCM.Data;
using HCM.Services.Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static HCM.Common.HCMConstants.RoleConstants;

namespace HCM.Web.Controllers
{   
    [Authorize(Roles = $"{EmployeeRoleName},{ManagerRoleName}")]
    public class EmployeeController : Controller
    {
        private IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public IActionResult Info()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found.");
            }

            var viewModel = employeeService.GetEmployeeInfoByUserId(userId);
            return View(viewModel);
        }
    }
}
