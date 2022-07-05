using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Exceptions;
using InventorySystem.Domain.Entities.Invoices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace InventorySystem.Application.Features.Invoices.Commands.UpdateInvoice
{
    public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand>
    {
        private readonly IInvoiceRepository _repository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateInvoiceCommandHandler> _logger;

        public UpdateInvoiceCommandHandler(IInvoiceRepository repository, IWarehouseRepository warehouseRepository, IMapper mapper, 
            ILogger<UpdateInvoiceCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _warehouseRepository = warehouseRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoiceToUpdate = await _repository.GetByIdAsync(request.Id, new List<Func<IQueryable<Invoice>, IIncludableQueryable<Invoice, object>>>
            {
                i => i.Include(e => e.InvoiceProducts)
            });

            if (invoiceToUpdate == null)
            {
                throw new NotFoundException(nameof(Invoice), request.Id);
            }

            await _warehouseRepository.UpdateWhenUpdateInvoiceAsync(invoiceToUpdate, _mapper.Map<Invoice>(request));

            _mapper.Map(request, invoiceToUpdate, typeof(UpdateInvoiceCommand), typeof(Invoice));

            await _repository.UpdateAsync(invoiceToUpdate);

            _logger.LogInformation($"Invoice {invoiceToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}
