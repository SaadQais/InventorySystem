using InventorySystem.Domain.Common;

namespace InventorySystem.Domain.Entities
{
    public class Warehouse : EntityBase
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}
