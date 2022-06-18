using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventorySystem.Application.Features.Warehouses.Commands.CreateWarehouse
{
    public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand>
    {
        private readonly IAsyncRepository<Warehouse> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateWarehouseCommandHandler> _logger;

        public CreateWarehouseCommandHandler(IAsyncRepository<Warehouse> repository, IMapper mapper, 
            ILogger<CreateWarehouseCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = _mapper.Map<Warehouse>(request);

            await _repository.AddAsync(warehouse);

            _logger.LogInformation($"Warehouse {warehouse.Id} is successfully added.");

            return Unit.Value;
        }
    }
}
