// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using HCM.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static HCM.Common.ValidaitonConstants.Employee;

namespace HCM.Web.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ChangePasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [MinLength(PasswordMinLength)]
            [MaxLength(PasswordMaxLength)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [MinLength(PasswordMinLength)]
            [MaxLength(PasswordMaxLength)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            [MinLength(PasswordMinLength)]
            [MaxLength(PasswordMaxLength)]
            [Display(Name = "Confirm new password")]
            public string ConfirmPassword { get; set; } = string.Empty;

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (Input.OldPassword == Input.NewPassword)
            {
                ModelState.AddModelError("Input.NewPassword", "The new password cannot be the same as the current password.");
                return Page();
            }
            if (!Input.NewPassword.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                ModelState.AddModelError("Input.NewPassword", "The new password must contain at least one special character.");
                return Page();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your password has been changed.";

            await _userManager.UpdateAsync(user);

            return RedirectToPage("/", new { area = "" });
        }
    }
}
