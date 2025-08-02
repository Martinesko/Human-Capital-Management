using Microsoft.AspNetCore.Mvc;
using static HCM.Common.HCMConstants.RoleConstants;

namespace HCM.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (!User!.Identity!.IsAuthenticated)
        {
            return View("Index");
        }

        if (User.IsInRole(ManagerRoleName) || User.IsInRole(HRAdminRoleName))
        {
            return View("ActionPanel");
        }

        return RedirectToAction("Profile", "Employee");
    }
}
