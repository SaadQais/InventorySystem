using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Features.Products.Queries.GetProductsCount
{
    public class GetProductCountQueryHandler : IRequestHandler<GetProductCountQuery, int>
    {
        private readonly IAsyncRepository<Product> _repository;

        public GetProductCountQueryHandler(IAsyncRepository<Product> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        public async Task<int> Handle(GetProductCountQuery request, CancellationToken cancellationToken)
        {
            return await _repository
                .GetAll()
                .CountAsync(cancellationToken: cancellationToken);
        }
    }
}
