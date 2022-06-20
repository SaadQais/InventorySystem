using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using MediatR;

namespace InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesById
{
    public class GetWarehouseByIdQuery : IRequest<WarehouseViewModel>
    {
        public int Id { get; set; }
    }
}
