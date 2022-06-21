using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Products.Queries.ViewModels;
using InventorySystem.Domain.Entities;
using MediatR;

namespace InventorySystem.Application.Features.Products.Queries.GetProductsById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductViewModel>
    {
        private readonly IAsyncRepository<Product> _repository;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IAsyncRepository<Product> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<ProductViewModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ProductViewModel>(await _repository.GetByIdAsync(request.Id));
        }
    }
}
