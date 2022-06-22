using InventorySystem.Domain.Common;

namespace InventorySystem.Domain.Entities.Invoices
{
    public class InvoiceProduct : EntityBase
    {
        public int ProductId { get; set; }
        public int InvoiceId { get; set; }

        public Product Product { get; set; }
        public Invoice Invoice { get; set; }
    }
}
