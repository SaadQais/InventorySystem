using AutoMapper;
using InventorySystem.Application.Extensions;
using InventorySystem.Application.Models.Role;
using InventorySystem.Application.Models.User;
using InventorySystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.WebMVC.Areas.Identity.Pages.Account
{
    [Authorize]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _logger = logger;
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; } = new RegisterViewModel();

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityResult result;

                var user = _mapper.Map<ApplicationUser>(Input);

                try
                {
                    result = await _userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync(RolesVM.Admin))
                            await _roleManager.CreateAsync(new IdentityRole(RolesVM.Admin));

                        await _userManager.AddToRoleAsync(user, RolesVM.Admin);

                        _logger.LogInformation("User created a new account with password.");

                        return RedirectToAction("Index", "Users");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ExceptionHelper.IsUniqueConstraintViolation(ex))
                    {
                        ModelState.AddModelError("Name", $"The Name '{user.UserName}' is already in use, please enter a different name.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
