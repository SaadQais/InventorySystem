using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using InventorySystem.Domain.Entities.Invoices;

namespace InventorySystem.Application.Features.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand>
    {
        private readonly IInvoiceRepository _repository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateInvoiceCommandHandler> _logger;

        public CreateInvoiceCommandHandler(IInvoiceRepository repository, IWarehouseRepository warehouseRepository, IMapper mapper, 
            ILogger<CreateInvoiceCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _warehouseRepository = warehouseRepository ?? throw new ArgumentNullException(nameof(warehouseRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = _mapper.Map<Invoice>(request);

            await _repository.AddAsync(invoice);

            _logger.LogInformation($"Invoice {invoice.Id} is successfully added.");

            await _warehouseRepository.UpdateWhenCreateInvoiceAsync(invoice);

            return Unit.Value;
        }
    }
}
