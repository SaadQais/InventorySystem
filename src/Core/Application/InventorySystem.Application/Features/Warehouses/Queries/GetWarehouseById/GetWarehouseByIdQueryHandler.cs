using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using InventorySystem.Domain.Entities;
using MediatR;

namespace InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesById
{
    public class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQuery, WarehouseViewModel>
    {
        private readonly IAsyncRepository<Warehouse> _repository;
        private readonly IMapper _mapper;

        public GetWarehouseByIdQueryHandler(IAsyncRepository<Warehouse> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<WarehouseViewModel> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<WarehouseViewModel>(await _repository.GetByIdAsync(request.Id));
        }
    }
}
