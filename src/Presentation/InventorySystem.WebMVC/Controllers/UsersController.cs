using AutoMapper;
using InventorySystem.Application.Contracts.Infrastructure;
using InventorySystem.Application.Models.Role;
using InventorySystem.Application.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventorySystem.WebMVC.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _usersService;
        private readonly IMapper _mapper;

        [BindProperty]
        public UserViewModel UserVM { get; set; }


        public UsersController(IUserService usersService, 
            IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
            
            UserVM = new UserViewModel();
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.Current = "Users";

            int pageSize = 15;

            var users = await _usersService.GetAllAsync(page, pageSize);

            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _usersService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _usersService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            ViewData["Roles"] = new SelectList(RolesVM.GetAll(), user.UserRole);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserViewModel user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _usersService.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
            }

            ViewData["Roles"] = new SelectList(RolesVM.GetAll(), user.UserRole);

            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _usersService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var user = await _usersService.GetAsync(id);

            if (user != null)
            {
                await _usersService.DisableAsync(id);

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult RestPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);

            var user = await _usersService.GetByNameAsync(User.Identity.Name);

            resetPasswordModel.Token = await _usersService.GeneratePasswordResetTokenAsync(User);
            resetPasswordModel.Email = user.Email;

            var result = await _usersService.ResetPasswordAsync(resetPasswordModel);
            
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return View();
            }

            return RedirectToPage("/Identity/Account/Manage/Index");
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
