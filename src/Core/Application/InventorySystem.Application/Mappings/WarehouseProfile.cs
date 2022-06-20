using AutoMapper;
using InventorySystem.Application.Features.Warehouses.Commands.CreateWarehouse;
using InventorySystem.Application.Features.Warehouses.Commands.UpdateWarehouse;
using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Mappings
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<Warehouse, WarehouseViewModel>().ReverseMap();
            CreateMap<Warehouse, CreateWarehouseCommand>().ReverseMap();
            CreateMap<Warehouse, UpdateWarehouseCommand>().ReverseMap();
        }
    }
}
