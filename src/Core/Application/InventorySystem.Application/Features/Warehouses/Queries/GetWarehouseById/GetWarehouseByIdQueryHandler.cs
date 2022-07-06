using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using InventorySystem.Domain.Entities.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesById
{
    public class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQuery, WarehouseViewModel>
    {
        private readonly IWarehouseRepository _repository;
        private readonly IMapper _mapper;

        public GetWarehouseByIdQueryHandler(IWarehouseRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<WarehouseViewModel> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<WarehouseViewModel>(await _repository.GetByIdAsync(request.Id, new List<Func<IQueryable<Warehouse>, IIncludableQueryable<Domain.Entities.Warehouses.Warehouse, object>>>
            {
                d => d.Include(w => w.WarehouseProducts).ThenInclude(wp => wp.Product)
            }));
        }
    }
}
