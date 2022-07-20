using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using MediatR;

namespace InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesDetails
{
    public class GetWarehouseDetailsQuery : IRequest<IReadOnlyList<WarehouseDetailsViewModel>>
    {
        
    }
}
