using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using MediatR;
using X.PagedList;

namespace InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesList
{
    public class GetWarehouseListQuery : IRequest<IPagedList<WarehouseViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetWarehouseListQuery(int? pageNumber, int? pageSize)
        {
            PageNumber = pageNumber ?? 1;
            PageSize = pageSize ?? 20;
        }
    }
}
