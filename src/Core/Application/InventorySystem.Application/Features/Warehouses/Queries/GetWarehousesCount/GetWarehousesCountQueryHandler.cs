using InventorySystem.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesCount
{
    public class GetWarehouseCountQueryHandler : IRequestHandler<GetWarehouseCountQuery, int>
    {
        private readonly IWarehouseRepository _repository;

        public GetWarehouseCountQueryHandler(IWarehouseRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        public async Task<int> Handle(GetWarehouseCountQuery request, CancellationToken cancellationToken)
        {
            return await _repository
                .GetAll()
                .CountAsync(cancellationToken: cancellationToken);
        }
    }
}
