using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Domain.Entities.Invoices;
using InventorySystem.Domain.Entities.Warehouses;
using InventorySystem.Domain.Enums;
using InventorySystem.Domain.Repositories;
using InventorySystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories
{
    public class WarehouseRepository : RepositoryBase<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Warehouse> GetCustomByIdAsync(int id, int pageNumber, int pageSize)
        {
            return _context.Warehouses
                .Include(w => w.WarehouseProducts)
                    .ThenInclude(wp => wp.Product)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task UpdateWhenCreateInvoiceAsync(Invoice invoice)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.WarehouseProducts)
                .FirstOrDefaultAsync(w => w.Id == invoice.WarehouseId);

            if(warehouse != null)
            {
                foreach(var invoiceProduct in invoice.InvoiceProducts)
                {
                    var warehouseProduct = warehouse.WarehouseProducts
                        .FirstOrDefault(wp => wp.ProductId == invoiceProduct.ProductId);

                    if(invoice.Type == InvoiceType.Incoming)
                    {
                        if (warehouseProduct != null)
                            warehouseProduct.Count += invoiceProduct.Count;
                        else
                        {
                            warehouse.WarehouseProducts.Add(new WarehouseProduct
                            {
                                ProductId = invoiceProduct.ProductId,
                                Count = invoiceProduct.Count,
                            });
                        }

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (warehouseProduct != null)
                            if (invoiceProduct.Count - warehouseProduct.Count < 0)
                                throw new IndexOutOfRangeException("Warehouse does not have enough items");
                            else
                            {
                                warehouseProduct.Count -= invoiceProduct.Count;
                                await _context.SaveChangesAsync();
                            }
                    }
                }
            }
        }

        public async Task UpdateWhenDeleteInvoiceAsync(Invoice invoice)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.WarehouseProducts)
                .FirstOrDefaultAsync(w => w.Id == invoice.WarehouseId);

            if (warehouse != null)
            {
                foreach (var invoiceProduct in invoice.InvoiceProducts)
                {
                    var warehouseProduct = warehouse.WarehouseProducts
                        .FirstOrDefault(wp => wp.ProductId == invoiceProduct.ProductId);

                    if (warehouseProduct != null)
                    {
                        if(invoice.Type == InvoiceType.Incoming)
                            warehouseProduct.Count -= invoiceProduct.Count;
                        else
                            warehouseProduct.Count += invoiceProduct.Count;

                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task UpdateWhenUpdateInvoiceAsync(Invoice oldInvoice, Invoice newInvoice)
        {
            await UpdateWhenDeleteInvoiceAsync(oldInvoice);
            await UpdateWhenCreateInvoiceAsync(newInvoice);
        }
    }
}
