using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.DirectEntries.Models;
using InventorySystem.Domain.Entities.DirectEntries;
using MediatR;
using X.PagedList;

namespace InventorySystem.Application.Features.DirectEntries.Queries.GetDirectEntriesList
{
    public class GetDirectEntryListQueryHandler : IRequestHandler<GetDirectEntryListQuery, IPagedList<DirectEntryViewModel>>
    {
        private readonly IAsyncRepository<DirectEntry> _repository;
        private readonly IMapper _mapper;

        public GetDirectEntryListQueryHandler(IAsyncRepository<DirectEntry> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<IPagedList<DirectEntryViewModel>> Handle(GetDirectEntryListQuery request, CancellationToken cancellationToken)
        {
            return await _repository
                .GetAll()
                .Where(i => i.Type == request.DirectEntryType)
                .ProjectTo<DirectEntryViewModel>(_mapper.ConfigurationProvider)
                .ToPagedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
