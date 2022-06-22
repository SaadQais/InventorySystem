using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Invoices.Queries.ViewModels;
using MediatR;

namespace InventorySystem.Application.Features.Invoices.Queries.GetInvoicesById
{
    public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceViewModel>
    {
        private readonly IInvoiceRepository _repository;
        private readonly IMapper _mapper;

        public GetInvoiceByIdQueryHandler(IInvoiceRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<InvoiceViewModel> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<InvoiceViewModel>(await _repository.GetByIdAsync(request.Id));
        }
    }
}
