using InventorySystem.Application.Features.Products.Queries.ViewModels;
using MediatR;
using X.PagedList;

namespace InventorySystem.Application.Features.Products.Queries.GetProductsList
{
    public class GetProductListQuery : IRequest<IPagedList<ProductViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetProductListQuery(int? pageNumber, int? pageSize)
        {
            PageNumber = pageNumber ?? 1;
            PageSize = pageSize ?? 20;
        }
    }
}
