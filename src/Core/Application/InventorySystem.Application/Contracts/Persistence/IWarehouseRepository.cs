using InventorySystem.Domain.Entities.Invoices;
using InventorySystem.Domain.Entities.Warehouses;

namespace InventorySystem.Application.Contracts.Persistence
{
    public interface IWarehouseRepository : IAsyncRepository<Warehouse>
    {
        Task UpdateWhenCreateInvoiceAsync(Invoice invoice);
        Task UpdateWhenDeleteInvoiceAsync(Invoice invoice);
        Task UpdateWhenUpdateInvoiceAsync(Invoice oldInvoice, Invoice newInvoice);
    }
}
