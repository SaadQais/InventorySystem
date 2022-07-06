using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Features.Invoices.Queries.GetInvoicesCount
{
    public class GetInvoiceCountQueryHandler : IRequestHandler<GetInvoiceCountQuery, InvoicesCountViewModel>
    {
        private readonly IInvoiceRepository _repository;

        public GetInvoiceCountQueryHandler(IInvoiceRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        public async Task<InvoicesCountViewModel> Handle(GetInvoiceCountQuery request, CancellationToken cancellationToken)
        {
            return new InvoicesCountViewModel
            {
                TotalCount = await _repository.GetAll()
                    .CountAsync(cancellationToken: cancellationToken),
                IncomingCount = await _repository.GetAll()
                    .Where(i => i.Type == InvoiceType.Incoming)
                    .CountAsync(cancellationToken: cancellationToken),
                OutgoingCount = await _repository.GetAll()
                    .Where(i => i.Type == InvoiceType.Outgoing)
                    .CountAsync(cancellationToken: cancellationToken)
            };
        }
    }
}
