using AutoMapper;
using InventorySystem.Application.Models.User;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
            CreateMap<ApplicationUser, RegisterViewModel>().ReverseMap();
        }
    }
}
