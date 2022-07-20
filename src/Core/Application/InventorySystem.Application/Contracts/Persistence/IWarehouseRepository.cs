using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using InventorySystem.Domain.Entities.DirectEntries;
using InventorySystem.Domain.Entities.Invoices;
using InventorySystem.Domain.Entities.Warehouses;

namespace InventorySystem.Application.Contracts.Persistence
{
    public interface IWarehouseRepository : IAsyncRepository<Warehouse>
    {
        Task UpdateWhenCreateInvoiceAsync(Invoice invoice);
        Task UpdateWhenCreateDirectEntryAsync(DirectEntry directEntry);
        Task UpdateWhenDeleteInvoiceAsync(Invoice invoice);
        Task UpdateWhenDeleteDirectEntryAsync(DirectEntry directEntry);
        Task UpdateWhenUpdateInvoiceAsync(Invoice oldInvoice, Invoice newInvoice);
        Task UpdateWhenUpdateDirectEntryAsync(DirectEntry olddirectEntry, DirectEntry newdirectEntry);
        Task<int> ProductCountAsync(int warehouseId, int productId);
        Task<IReadOnlyList<WarehouseDetailsViewModel>> GetDashboardDetailsAsync();
    }
}
