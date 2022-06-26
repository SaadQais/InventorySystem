using InventorySystem.Domain.Common;

namespace InventorySystem.Domain.Entities.Warehouses
{
    public class WarehouseProduct : EntityBase
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int Count { get; set; }

        public Product Product { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
