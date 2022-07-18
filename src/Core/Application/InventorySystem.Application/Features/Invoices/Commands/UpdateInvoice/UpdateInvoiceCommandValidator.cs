using FluentValidation;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Invoices.Commands.CreateInvoice;
using InventorySystem.Application.Features.Invoices.Models;
using InventorySystem.Domain.Entities.Invoices;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Features.Invoices.Commands.UpdateInvoice
{
    public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public UpdateInvoiceCommandValidator(IWarehouseRepository warehouseRepository, IInvoiceRepository invoiceRepository)
        {
            _warehouseRepository = warehouseRepository;
            _invoiceRepository = invoiceRepository;

            RuleFor(p => p.Number)
                .NotEmpty().WithMessage("{Name} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Name} must not exceed 50 characters.");

            RuleForEach(invoice => invoice.InvoiceProducts)
                .MustAsync(CheckProductCountAvailablityAsync)
                .WithMessage("Invoice product count cannot be greater than the storage count");
        }

        private async Task<bool> CheckProductCountAvailablityAsync(UpdateInvoiceCommand invoice, InvoiceProductModel invoiceProduct,
            CancellationToken cancellationToken)
        {
            var oldInvoice = await _invoiceRepository.GetByIdAsync(invoice.Id, new List<Func<IQueryable<Invoice>, IIncludableQueryable<Invoice, object>>>
            {
                d => d.Include(i => i.Warehouse),
                d => d.Include(i => i.InvoiceProducts).ThenInclude(ip => ip.Product)
            });

            if (oldInvoice != null)
            {
                int availableCount = await _warehouseRepository.ProductCountAsync(invoice.WarehouseId, invoiceProduct.ProductId);

                int invoiceProductCount = oldInvoice.InvoiceProducts
                    .FirstOrDefault(i => i.ProductId == invoiceProduct.ProductId)?.Count ?? 0;

                if ((availableCount + invoiceProductCount) >= invoiceProduct.Count)
                    return true;
            }

            return false; 
        }
    }
}
