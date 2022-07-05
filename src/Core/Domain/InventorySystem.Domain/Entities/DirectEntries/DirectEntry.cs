using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Warehouses;
using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.DirectEntries
{
    public class DirectEntry : EntityBase
    {
        public DirectEntryType Type { get; set; }
        public string Description { get; set; }
        public int? WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        public ICollection<DirectEntryProduct> DirectEntryProducts { get; set; } = new List<DirectEntryProduct>();
    }
}
