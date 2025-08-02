using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace HCM.Web.Pages.Home
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (User.IsInRole("Employee"))
            {
                return RedirectToPage("/Account/Info");
            }

            return Page();
        }
    }
}
