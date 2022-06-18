using InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesList.ViewModels;
using MediatR;
using X.PagedList;

namespace InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesList
{
    public class GetWarehousesListQuery : IRequest<IPagedList<WarehouseVM>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetWarehousesListQuery(int? pageNumber, int? pageSize)
        {
            PageNumber = pageNumber ?? 1;
            PageSize = pageSize ?? 20;
        }
    }
}
