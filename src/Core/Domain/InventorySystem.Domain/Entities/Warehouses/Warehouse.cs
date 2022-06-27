using InventorySystem.Domain.Common;

namespace InventorySystem.Domain.Entities.Warehouses
{
    public class Warehouse : EntityBase
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public ICollection<WarehouseProduct> WarehouseProducts { get; set; } = new List<WarehouseProduct>();
    }
}
