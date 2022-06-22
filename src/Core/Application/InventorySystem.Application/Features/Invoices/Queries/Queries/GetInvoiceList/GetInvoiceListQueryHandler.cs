using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Invoices.Queries.ViewModels;
using MediatR;
using X.PagedList;

namespace InventorySystem.Application.Features.Invoices.Queries.GetInvoicesList
{
    public class GetInvoiceListQueryHandler : IRequestHandler<GetInvoiceListQuery, IPagedList<InvoiceViewModel>>
    {
        private readonly IInvoiceRepository _repository;
        private readonly IMapper _mapper;

        public GetInvoiceListQueryHandler(IInvoiceRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<IPagedList<InvoiceViewModel>> Handle(GetInvoiceListQuery request, CancellationToken cancellationToken)
        {
            return await _repository
                .GetAll()
                .ProjectTo<InvoiceViewModel>(_mapper.ConfigurationProvider)
                .ToPagedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
