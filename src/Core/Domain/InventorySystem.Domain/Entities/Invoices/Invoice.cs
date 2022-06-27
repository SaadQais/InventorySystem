using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Warehouses;
using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.Invoices
{
    public class Invoice : EntityBase
    {
        public string Number { get; set; }
        public InvoiceType Type { get; set; }
        public string Supplier { get; set; }
        public string Client { get; set; }
        public string Description { get; set; }

        public int? WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        public ICollection<InvoiceProduct> InvoiceProducts { get; set; } = new List<InvoiceProduct>();
    }
}
