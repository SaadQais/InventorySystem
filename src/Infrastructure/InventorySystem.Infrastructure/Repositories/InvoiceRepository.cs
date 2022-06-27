using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Domain.Entities.Invoices;
using InventorySystem.Domain.Repositories;
using InventorySystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories
{
    public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<Invoice> GetCustomByIdAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.Warehouse)
                .Include(i => i.InvoiceProducts).ThenInclude(ip => ip.Product)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
