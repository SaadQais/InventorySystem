﻿using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Domain.Entities.Invoices;
using InventorySystem.Domain.Entities.Warehouses;
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

        public async Task UpdateWarehouseProductsAsync(Invoice invoice)
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
            }
        }
    }
}
