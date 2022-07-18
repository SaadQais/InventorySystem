using FluentValidation;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Invoices.Models;

namespace InventorySystem.Application.Features.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public CreateInvoiceCommandValidator(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;

            RuleFor(p => p.Number)
                .NotEmpty().WithMessage("{Number} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Number} must not exceed 50 characters.");

            RuleForEach(invoice => invoice.InvoiceProducts)
                .MustAsync(CheckProductCountAvailablityAsync)
                .WithMessage("Invoice product count cannot be greater than the storage count");
        }

        private async Task<bool> CheckProductCountAvailablityAsync(CreateInvoiceCommand invoice, InvoiceProductModel invoiceProduct, 
            CancellationToken cancellationToken)
        {
            if (invoice.Type == Domain.Enums.InvoiceType.Incoming)
                return true;

            int availableCount = await _warehouseRepository.ProductCountAsync(invoice.WarehouseId, invoiceProduct.ProductId);

            if (availableCount >= invoiceProduct.Count)
                return true;

            return false;
        }
    }
}
