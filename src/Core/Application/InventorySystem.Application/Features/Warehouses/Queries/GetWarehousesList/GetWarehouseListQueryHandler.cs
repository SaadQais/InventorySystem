using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesList.ViewModels;
using InventorySystem.Domain.Entities;
using MediatR;
using X.PagedList;

namespace InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesList
{
    public class GetWarehouseListQueryHandler : IRequestHandler<GetWarehousesListQuery, IPagedList<WarehouseVM>>
    {
        private readonly IAsyncRepository<Warehouse> _repository;
        private readonly IMapper _mapper;

        public GetWarehouseListQueryHandler(IAsyncRepository<Warehouse> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<IPagedList<WarehouseVM>> Handle(GetWarehousesListQuery request, CancellationToken cancellationToken)
        {
            return await _repository
                .GetAll()
                .ProjectTo<WarehouseVM>(_mapper.ConfigurationProvider)
                .ToPagedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
