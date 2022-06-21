using AutoMapper;
using InventorySystem.Application.Features.Products.Commands.CreateProduct;
using InventorySystem.Application.Features.Products.Commands.UpdateProduct;
using InventorySystem.Application.Features.Products.Queries.ViewModels;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Product, CreateProductCommand>().ReverseMap();
            CreateMap<Product, UpdateProductCommand>().ReverseMap();
        }
    }
}
