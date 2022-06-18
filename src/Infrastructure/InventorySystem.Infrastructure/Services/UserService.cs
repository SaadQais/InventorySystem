using InventorySystem.Application.Contracts.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace InventorySystem.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string UserId
        {
            get
            {
                var nameId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                return nameId != null ? int.Parse(nameId.Value) : -1;
            }
        }
    }
}
