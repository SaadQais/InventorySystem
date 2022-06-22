using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Exceptions;
using InventorySystem.Domain.Entities.Invoices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventorySystem.Application.Features.Invoices.Commands.DeleteInvoice
{
    public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand>
    {
        private readonly IInvoiceRepository _repository;
        private readonly ILogger<DeleteInvoiceCommandHandler> _logger;

        public DeleteInvoiceCommandHandler(IInvoiceRepository repository, ILogger<DeleteInvoiceCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoiceToDelete = await _repository.GetByIdAsync(request.Id);
            if (invoiceToDelete == null)
            {
                throw new NotFoundException(nameof(Invoice), request.Id);
            }

            await _repository.DeleteAsync(invoiceToDelete);
            _logger.LogInformation($"Invoice {invoiceToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
