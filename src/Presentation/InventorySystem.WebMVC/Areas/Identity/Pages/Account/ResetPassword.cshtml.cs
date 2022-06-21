using InventorySystem.Application.Models.User;
using InventorySystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventorySystem.WebMVC.Areas.Identity.Pages.Account
{
    [Authorize]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public ResetPasswordViewModel Input { get; set; }

        
        public async Task<IActionResult> OnGetAsync(string userId, string token = null)
        {
            ApplicationUser user;

            if(!string.IsNullOrEmpty(userId))
            {
                user = await _userManager.FindByIdAsync(userId);
                ViewData["UserId"] = userId;
            }
            else
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (token == null)
            {
                token = await _userManager.GeneratePasswordResetTokenAsync(user);
            }

            Input = new ResetPasswordViewModel
            {
                Token = token,
                Email = user.Email
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                ModelState.AddModelError(string.Empty, "Cannot find user with this email");
                return Page();
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Token, Input.Password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(userId))
                    return RedirectToAction("Index", "Users");

                return RedirectToPage("./Manage/Index", new { message = "Password was reset successfully" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
