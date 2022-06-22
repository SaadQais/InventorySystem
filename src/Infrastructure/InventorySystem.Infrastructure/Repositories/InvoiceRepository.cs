using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Domain.Entities.Invoices;
using InventorySystem.Domain.Repositories;
using InventorySystem.Infrastructure.Persistence;

namespace InventorySystem.Infrastructure.Repositories
{
    public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
