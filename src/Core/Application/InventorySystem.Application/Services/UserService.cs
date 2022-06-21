using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventorySystem.Application.Contracts.Infrastructure;
using InventorySystem.Application.Models.Role;
using InventorySystem.Application.Models.User;
using InventorySystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;

namespace InventorySystem.Application.Services
{
    public class UserService : IUserService
    {
        //private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            //_contextAccessor = contextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        //public string UserId => _contextAccessor.HttpContext.User.Claims
        //    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        public async Task<IPagedList<UserViewModel>> GetAllAsync(int pageNumber = 1, int pageSize = 20)
        {
            IQueryable<ApplicationUser> users = _userManager.Users;

            return await users
                .OrderBy(u => u.UserName)
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<UserViewModel> GetByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return _mapper.Map<UserViewModel>(user);
        }

        public async Task<UserViewModel> GetAsync(string id)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            var userVM = _mapper.Map<UserViewModel>(user);
            userVM.UserRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return userVM;
        }

        public async Task UpdateAsync(UserViewModel userVM)
        {
            var user = await _userManager.FindByIdAsync(userVM.Id);

            await UpdateUserRoleAsync(user, userVM.UserRole);

            user.PhoneNumber = userVM.PhoneNumber;
            user.UserName = userVM.UserName;

            await _userManager.UpdateAsync(user);
        }

        public async Task DisableAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            await _userManager.UpdateAsync(user);

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(100));
        }

        public async Task EnableAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            await _userManager.UpdateAsync(user);

            await _userManager.SetLockoutEnabledAsync(user, false);
            await _userManager.SetLockoutEndDateAsync(user, null);
        }

        private async Task UpdateUserRoleAsync(ApplicationUser user, string role)
        {
            await _userManager.RemoveFromRolesAsync(user, RolesVM.GetAll());

            if (RolesVM.GetAll().Contains(role))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));

                await _userManager.AddToRoleAsync(user, role);
            }
            else { throw new Exception("Role does not exist"); }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordViewModel resetPasswordModel)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);

            return resetPassResult;
        }
    }
}
