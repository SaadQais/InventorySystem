using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using InventorySystem.Domain.Entities.DirectEntries;
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
                        {
                            warehouseProduct.Count -= invoiceProduct.Count;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        public async Task UpdateWhenCreateDirectEntryAsync(DirectEntry directEntry)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.WarehouseProducts)
                .FirstOrDefaultAsync(w => w.Id == directEntry.WarehouseId);

            if (warehouse != null)
            {
                foreach (var directEntryProduct in directEntry.DirectEntryProducts)
                {
                    var warehouseProduct = warehouse.WarehouseProducts
                        .FirstOrDefault(wp => wp.ProductId == directEntryProduct.ProductId);

                    if (directEntry.Type == DirectEntryType.Incoming)
                    {
                        if (warehouseProduct != null)
                            warehouseProduct.Count += directEntryProduct.Count;
                        else
                        {
                            warehouse.WarehouseProducts.Add(new WarehouseProduct
                            {
                                ProductId = directEntryProduct.ProductId,
                                Count = directEntryProduct.Count,
                            });
                        }

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (warehouseProduct != null)
                            if ((warehouseProduct.Count - directEntryProduct.Count) < 0)
                                throw new IndexOutOfRangeException("Warehouse does not have enough items");
                            else
                            {
                                warehouseProduct.Count -= directEntryProduct.Count;
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

        public async Task UpdateWhenDeleteDirectEntryAsync(DirectEntry directEntry)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.WarehouseProducts)
                .FirstOrDefaultAsync(w => w.Id == directEntry.WarehouseId);

            if (warehouse != null)
            {
                foreach (var directEntryProduct in directEntry.DirectEntryProducts)
                {
                    var warehouseProduct = warehouse.WarehouseProducts
                        .FirstOrDefault(wp => wp.ProductId == directEntryProduct.ProductId);

                    if (warehouseProduct != null)
                    {
                        if (directEntry.Type == DirectEntryType.Incoming)
                            warehouseProduct.Count -= directEntryProduct.Count;
                        else
                            warehouseProduct.Count += directEntryProduct.Count;

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

        public async Task UpdateWhenUpdateDirectEntryAsync(DirectEntry olddirectEntry, DirectEntry newdirectEntry)
        {
            await UpdateWhenDeleteDirectEntryAsync(olddirectEntry);
            await UpdateWhenCreateDirectEntryAsync(newdirectEntry);
        }

        public async Task<int> ProductCountAsync(int warehouseId, int productId)
        {
            return await _context.WarehouseProducts
                .Where(wp => wp.ProductId == productId)
                .Where(wp => wp.WarehouseId == warehouseId)
                .SumAsync(wp => wp.Count);
        }

        public async Task<IReadOnlyList<WarehouseDetailsViewModel>> GetDashboardDetailsAsync()
        {
            var details = await _context.WarehouseProducts
                .Include(w => w.Warehouse)
                .Include(w => w.Product)
                .OrderByDescending(wp => wp.Count)
                .Take(4)
                .Select(wp => new WarehouseDetailsViewModel
                {
                    Warehouse = wp.Warehouse.Name,
                    Product = wp.Product.Name,
                    Count = wp.Count
                })
                .ToListAsync();

            return details;
        }
    }
}
