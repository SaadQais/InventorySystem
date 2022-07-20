using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesDetails
{
    public class GetWarehouseDetailsQueryHandler : IRequestHandler<GetWarehouseDetailsQuery, IReadOnlyList<WarehouseDetailsViewModel>>
    {
        private readonly IWarehouseRepository _repository;

        public GetWarehouseDetailsQueryHandler(IWarehouseRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        public async Task<IReadOnlyList<WarehouseDetailsViewModel>> Handle(GetWarehouseDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _repository
                .GetDashboardDetailsAsync();
        }
    }
}
