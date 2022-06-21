using InventorySystem.Application.Features.Products.Queries.ViewModels;
using MediatR;

namespace InventorySystem.Application.Features.Products.Queries.GetProductsById
{
    public class GetProductByIdQuery : IRequest<ProductViewModel>
    {
        public int Id { get; set; }
    }
}
