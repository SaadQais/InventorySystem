using FluentValidation;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Invoices.Commands.CreateInvoice;
using InventorySystem.Application.Features.Invoices.Models;

namespace InventorySystem.Application.Features.Invoices.Commands.UpdateInvoice
{
    public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public UpdateInvoiceCommandValidator(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;

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
            int availableCount = await _warehouseRepository.ProductCountAsync(invoice.WarehouseId, invoiceProduct.ProductId);

            if (availableCount >= invoiceProduct.Count)
                return true;

            return false;
        }
    }
}
