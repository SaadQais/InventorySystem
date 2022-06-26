using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Exceptions;
using InventorySystem.Domain.Entities.Warehouses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventorySystem.Application.Features.Warehouses.Commands.DeleteWarehouse
{
    public class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand>
    {
        private readonly IAsyncRepository<Warehouse> _repository;
        private readonly ILogger<DeleteWarehouseCommandHandler> _logger;

        public DeleteWarehouseCommandHandler(IAsyncRepository<Warehouse> repository, ILogger<DeleteWarehouseCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouseToDelete = await _repository.GetByIdAsync(request.Id);
            if (warehouseToDelete == null)
            {
                throw new NotFoundException(nameof(Warehouse), request.Id);
            }

            await _repository.DeleteAsync(warehouseToDelete);
            _logger.LogInformation($"Warehouse {warehouseToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
