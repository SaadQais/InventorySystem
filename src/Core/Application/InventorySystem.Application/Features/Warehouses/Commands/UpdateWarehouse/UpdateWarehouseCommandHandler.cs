using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Exceptions;
using InventorySystem.Domain.Entities.Warehouses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventorySystem.Application.Features.Warehouses.Commands.UpdateWarehouse
{
    public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand>
    {
        private readonly IAsyncRepository<Warehouse> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateWarehouseCommandHandler> _logger;

        public UpdateWarehouseCommandHandler(IAsyncRepository<Warehouse> repository, IMapper mapper, 
            ILogger<UpdateWarehouseCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouseToUpdate = await _repository.GetByIdAsync(request.Id);

            if (warehouseToUpdate == null)
            {
                throw new NotFoundException(nameof(Warehouse), request.Id);
            }

            _mapper.Map(request, warehouseToUpdate, typeof(UpdateWarehouseCommand), typeof(Warehouse));

            await _repository.UpdateAsync(warehouseToUpdate);

            _logger.LogInformation($"Warehouse {warehouseToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}
