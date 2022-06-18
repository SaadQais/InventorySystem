using MediatR;

namespace InventorySystem.Application.Features.Warehouses.Commands.UpdateWarehouse
{
    public class UpdateWarehouseCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}
