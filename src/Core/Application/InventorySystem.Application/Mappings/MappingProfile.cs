using AutoMapper;
using InventorySystem.Application.Features.Warehouses.Commands.CreateWarehouse;
using InventorySystem.Application.Features.Warehouses.Commands.UpdateWarehouse;
using InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesList.ViewModels;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Warehouse, WarehouseVM>().ReverseMap();
            CreateMap<Warehouse, CreateWarehouseCommand>().ReverseMap();
            CreateMap<Warehouse, UpdateWarehouseCommand>().ReverseMap();
        }
    }
}
