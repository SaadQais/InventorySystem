using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using InventorySystem.Domain.Entities.Warehouses;
using MediatR;
using X.PagedList;

namespace InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesList
{
    public class GetWarehouseListQueryHandler : IRequestHandler<GetWarehouseListQuery, IPagedList<WarehouseViewModel>>
    {
        private readonly IAsyncRepository<Warehouse> _repository;
        private readonly IMapper _mapper;

        public GetWarehouseListQueryHandler(IAsyncRepository<Warehouse> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<IPagedList<WarehouseViewModel>> Handle(GetWarehouseListQuery request, CancellationToken cancellationToken)
        {
            return await _repository
                .GetAll()
                .ProjectTo<WarehouseViewModel>(_mapper.ConfigurationProvider)
                .ToPagedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
