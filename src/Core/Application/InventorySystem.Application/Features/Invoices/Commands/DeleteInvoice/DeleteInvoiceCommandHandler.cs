using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Exceptions;
using InventorySystem.Domain.Entities.Invoices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace InventorySystem.Application.Features.Invoices.Commands.DeleteInvoice
{
    public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand>
    {
        private readonly IInvoiceRepository _repository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ILogger<DeleteInvoiceCommandHandler> _logger;

        public DeleteInvoiceCommandHandler(IInvoiceRepository repository, IWarehouseRepository warehouseRepository,
            ILogger<DeleteInvoiceCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _warehouseRepository = warehouseRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoiceToDelete = await _repository.GetByIdAsync(request.Id, new List<Func<IQueryable<Invoice>, IIncludableQueryable<Invoice, object>>>
            {
                i => i.Include(e => e.InvoiceProducts)
            });

            if (invoiceToDelete == null)
            {
                throw new NotFoundException(nameof(Invoice), request.Id);
            }

            await _warehouseRepository.UpdateWhenDeleteInvoiceAsync(invoiceToDelete);

            await _repository.DeleteAsync(invoiceToDelete);
            _logger.LogInformation($"Invoice {invoiceToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
