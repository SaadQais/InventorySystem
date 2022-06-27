using InventorySystem.Domain.Entities.Invoices;

namespace InventorySystem.Application.Contracts.Persistence
{
    public interface IInvoiceRepository : IAsyncRepository<Invoice>
    {
        Task UpdateWarehouseProductsAsync(Invoice invoice);
    }
}
