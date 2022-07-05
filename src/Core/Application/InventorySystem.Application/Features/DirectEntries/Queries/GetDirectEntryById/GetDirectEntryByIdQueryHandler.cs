using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.DirectEntries.Models;
using InventorySystem.Application.Features.DirectEntries.Queries.GetDirectEntriesById;
using InventorySystem.Domain.Entities.DirectEntries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace InventorySystem.Application.Features.DirectEntries.Queries.GetInvoicesById
{
    public class GetDirectEntryByIdQueryHandler : IRequestHandler<GetDirectEntryByIdQuery, DirectEntryViewModel>
    {
        private readonly IAsyncRepository<DirectEntry> _repository;
        private readonly IMapper _mapper;

        public GetDirectEntryByIdQueryHandler(IAsyncRepository<DirectEntry> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<DirectEntryViewModel> Handle(GetDirectEntryByIdQuery request, CancellationToken cancellationToken)
        {
            var directEntry = await _repository.GetByIdAsync(request.Id, new List<Func<IQueryable<DirectEntry>, IIncludableQueryable<DirectEntry, object>>>
            {
                d => d.Include(e => e.Warehouse),
                d => d.Include(e => e.DirectEntryProducts).ThenInclude(e => e.Product)
            });

            return _mapper.Map<DirectEntryViewModel>(directEntry);
        }
    }
}
