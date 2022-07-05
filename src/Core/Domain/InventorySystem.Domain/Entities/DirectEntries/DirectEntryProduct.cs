using InventorySystem.Domain.Common;

namespace InventorySystem.Domain.Entities.DirectEntries
{
    public class DirectEntryProduct : EntityBase
    {
        public int ProductId { get; set; }
        public int DirectEntryId { get; set; }
        public int Count { get; set; }

        public Product Product { get; set; }
        public DirectEntry DirectEntry { get; set; }
    }
}
