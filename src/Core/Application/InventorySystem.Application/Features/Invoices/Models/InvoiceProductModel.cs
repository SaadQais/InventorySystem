using InventorySystem.Application.Features.Products.Queries.ViewModels;

namespace InventorySystem.Application.Features.Invoices.Models
{
    public class InvoiceProductModel
    {
        public int ProductId { get; set; }
        public int InvoiceId { get; set; }
        public int Count { get; set; }

        public ProductViewModel Product { get; set; }
    }
}
