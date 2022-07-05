using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Domain.Entities.DirectEntries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventorySystem.Application.Features.DirectEntries.Commands.CreateDirectEntry
{
    public class CreateDirectEntryCommandHandler : IRequestHandler<CreateDirectEntryCommand>
    {
        private readonly IAsyncRepository<DirectEntry> _repository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateDirectEntryCommandHandler> _logger;

        public CreateDirectEntryCommandHandler(IAsyncRepository<DirectEntry> repository, IWarehouseRepository warehouseRepository, 
            IMapper mapper, ILogger<CreateDirectEntryCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _warehouseRepository = warehouseRepository ?? throw new ArgumentNullException(nameof(warehouseRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(CreateDirectEntryCommand request, CancellationToken cancellationToken)
        {
            var directEntry = _mapper.Map<DirectEntry>(request);

            await _repository.AddAsync(directEntry);

            _logger.LogInformation($"DirectEntry {directEntry.Id} is successfully added.");

            await _warehouseRepository.UpdateWhenCreateDirectEntryAsync(directEntry);

            return Unit.Value;
        }
    }
}
