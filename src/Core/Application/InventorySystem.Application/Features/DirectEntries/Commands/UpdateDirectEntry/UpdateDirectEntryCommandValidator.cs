using FluentValidation;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.DirectEntries.Models;
using InventorySystem.Domain.Entities.DirectEntries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace InventorySystem.Application.Features.DirectEntries.Commands.UpdateDirectEntry
{
    public class UpdateDirectEntryCommandValidator : AbstractValidator<UpdateDirectEntryCommand>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IAsyncRepository<DirectEntry> _repository;

        public UpdateDirectEntryCommandValidator(IWarehouseRepository warehouseRepository, IAsyncRepository<DirectEntry> repository)
        {
            _warehouseRepository = warehouseRepository;
            _repository = repository;

            RuleForEach(invoice => invoice.DirectEntryProducts)
                .MustAsync(CheckProductCountAvailablityAsync)
                .WithMessage("Invoice product count cannot be greater than the storage count");
        }

        private async Task<bool> CheckProductCountAvailablityAsync(UpdateDirectEntryCommand directEntry, DirectEntryProductModel directEntryProduct,
            CancellationToken cancellationToken)
        {
            var oldDirectEntry = await _repository.GetByIdAsync(directEntry.Id, new List<Func<IQueryable<DirectEntry>, IIncludableQueryable<DirectEntry, object>>>
            {
                d => d.Include(e => e.Warehouse),
                d => d.Include(e => e.DirectEntryProducts).ThenInclude(e => e.Product)
            });

            if (oldDirectEntry != null)
            {
                int availableCount = await _warehouseRepository.ProductCountAsync(directEntry.WarehouseId, directEntryProduct.ProductId);

                int invoiceProductCount = oldDirectEntry.DirectEntryProducts
                    .FirstOrDefault(i => i.ProductId == directEntryProduct.ProductId)?.Count ?? 0;

                if ((availableCount + invoiceProductCount) >= directEntryProduct.Count)
                    return true;
            }

            return false;
        }
    }
}
