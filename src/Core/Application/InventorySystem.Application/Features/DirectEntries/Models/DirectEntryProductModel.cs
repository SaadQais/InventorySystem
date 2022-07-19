using InventorySystem.Application.Features.Products.Queries.ViewModels;

namespace InventorySystem.Application.Features.DirectEntries.Models
{
    public class DirectEntryProductModel
    {
        public int ProductId { get; set; }
        public int DirectEntryId { get; set; }
        public int Count { get; set; }

        public ProductViewModel Product { get; set; }
    }
}
