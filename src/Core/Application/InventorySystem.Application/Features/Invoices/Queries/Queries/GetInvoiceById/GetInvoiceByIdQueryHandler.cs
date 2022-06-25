﻿using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Invoices.Queries.ViewModels;
using InventorySystem.Domain.Entities.Invoices;
using MediatR;
using System.Linq.Expressions;

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
            var invoice = await _repository.GetByIdAsync(request.Id, new List<Expression<Func<Invoice, object>>>
            {
                x => x.Warehouse,
            });

            return _mapper.Map<InvoiceViewModel>(invoice);
        }
    }
}
