using FluentValidation;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.DirectEntries.Models;
using InventorySystem.Domain.Entities.Invoices;

namespace InventorySystem.Application.Features.DirectEntries.Commands.CreateDirectEntry
{
    public class CreateDirectEntryCommandValidator : AbstractValidator<CreateDirectEntryCommand>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public CreateDirectEntryCommandValidator(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;

            RuleForEach(invoice => invoice.DirectEntryProducts)
                .MustAsync(CheckProductCountAvailablityAsync)
                .WithMessage("Direct Entry product count cannot be greater than the storage count");
        }

        private async Task<bool> CheckProductCountAvailablityAsync(CreateDirectEntryCommand directEntry, 
            DirectEntryProductModel directEntryProduct, CancellationToken cancellationToken)
        {
            if (directEntry.Type == Domain.Enums.DirectEntryType.Incoming)
                return true;

            int availableCount = await _warehouseRepository.ProductCountAsync(directEntry.WarehouseId, directEntryProduct.ProductId);

            int directEntryProductCount = directEntry.DirectEntryProducts
                .Where(i => i.ProductId == directEntryProduct.ProductId)
                .Sum(i => i.Count);

            if (availableCount >= directEntryProductCount)
                return true;

            return false;
        }
    }
}
