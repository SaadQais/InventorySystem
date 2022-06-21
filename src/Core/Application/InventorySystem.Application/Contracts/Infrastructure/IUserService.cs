using InventorySystem.Application.Models.User;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using X.PagedList;

namespace InventorySystem.Application.Contracts.Infrastructure
{
    public interface IUserService
    {
        //string UserId { get; }

        Task<IPagedList<UserViewModel>> GetAllAsync(int pageNumber = 1, int pageSize = 20);
        Task<UserViewModel> GetByNameAsync(string userName);
        Task<UserViewModel> GetAsync(string id);
        Task UpdateAsync(UserViewModel userVM);
        Task DisableAsync(string id);
        Task EnableAsync(string id);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordViewModel resetPasswordModel);
        Task<string> GeneratePasswordResetTokenAsync(ClaimsPrincipal claimsPrincipal);
    }
}
