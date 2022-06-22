using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Exceptions;
using InventorySystem.Domain.Entities.Invoices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventorySystem.Application.Features.Invoices.Commands.UpdateInvoice
{
    public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand>
    {
        private readonly IInvoiceRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateInvoiceCommandHandler> _logger;

        public UpdateInvoiceCommandHandler(IInvoiceRepository repository, IMapper mapper, 
            ILogger<UpdateInvoiceCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoiceToUpdate = await _repository.GetByIdAsync(request.Id);

            if (invoiceToUpdate == null)
            {
                throw new NotFoundException(nameof(Invoice), request.Id);
            }

            _mapper.Map(request, invoiceToUpdate, typeof(UpdateInvoiceCommand), typeof(Invoice));

            await _repository.UpdateAsync(invoiceToUpdate);

            _logger.LogInformation($"Invoice {invoiceToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}
