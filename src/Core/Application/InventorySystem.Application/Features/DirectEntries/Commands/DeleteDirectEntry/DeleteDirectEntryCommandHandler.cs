using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Exceptions;
using InventorySystem.Domain.Entities.DirectEntries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace InventorySystem.Application.Features.DirectEntries.Commands.DeleteDirectEntry
{
    public class DeleteDirectEntryCommandHandler : IRequestHandler<DeleteDirectEntryCommand>
    {
        private readonly IAsyncRepository<DirectEntry> _repository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ILogger<DeleteDirectEntryCommandHandler> _logger;

        public DeleteDirectEntryCommandHandler(IAsyncRepository<DirectEntry> repository, IWarehouseRepository warehouseRepository,
            ILogger<DeleteDirectEntryCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _warehouseRepository = warehouseRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteDirectEntryCommand request, CancellationToken cancellationToken)
        {
            var directEntryToDelete = await _repository.GetByIdAsync(request.Id, new List<Func<IQueryable<DirectEntry>, IIncludableQueryable<DirectEntry, object>>>
            {
                i => i.Include(e => e.DirectEntryProducts)
            });

            if (directEntryToDelete == null)
            {
                throw new NotFoundException(nameof(DirectEntry), request.Id);
            }

            await _warehouseRepository.UpdateWhenDeleteDirectEntryAsync(directEntryToDelete);

            await _repository.DeleteAsync(directEntryToDelete);
            _logger.LogInformation($"DirectEntry {directEntryToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
