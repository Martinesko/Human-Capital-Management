using Microsoft.AspNetCore.Mvc;
using static HCM.Common.HCMConstants.RoleConstants;

namespace HCM.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (User.IsInRole(EmployeeRoleName))
        {
            return RedirectToAction("Info", "Employee");
        }
        return View();
    }
}
