using HCM.Services.Data.Contracts;
using HCM.Web.ViewModels.Employee;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static HCM.Common.HCMConstants.RoleConstants;

namespace HCM.Web.Controllers;

public class HomeController : Controller
{
    private IEmployeeService employeeService;

    public HomeController(IEmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }
    public IActionResult Index(bool showProfile = false)
    {
        EmployeeInfoViewModel? employeeInfo = null;
        bool isHrAdmin = false;
        bool isManager = false;

        if (User.Identity.IsAuthenticated && (User.IsInRole(EmployeeRoleName) || User.IsInRole(ManagerRoleName)))
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                employeeInfo = employeeService.GetEmployeeInfoByUserId(userId);
            }
            if (User.IsInRole(ManagerRoleName))
            {
                isManager = true;
            }
        }
        else if (User.Identity.IsAuthenticated && User.IsInRole(HRAdminRoleName)) {
            isHrAdmin = true;
        }

        ViewBag.IsHrAdmin = isHrAdmin;
        ViewBag.IsManager = isManager;
        ViewBag.ShowProfile = showProfile;
        return View(employeeInfo);
    }
}
