using MediatR;

namespace InventorySystem.Application.Features.Warehouses.Commands.DeleteWarehouse
{
    public class DeleteWarehouseCommand : IRequest
    {
        public int Id { get; set; }
    }
}
