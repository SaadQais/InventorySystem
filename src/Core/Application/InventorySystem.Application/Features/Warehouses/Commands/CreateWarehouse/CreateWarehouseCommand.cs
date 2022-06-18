using MediatR;

namespace InventorySystem.Application.Features.Warehouses.Commands.CreateWarehouse
{
    public class CreateWarehouseCommand : IRequest
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}
