using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Features.Products.Queries.ViewModels;
using InventorySystem.Domain.Entities;
using MediatR;
using X.PagedList;

namespace InventorySystem.Application.Features.Products.Queries.GetProductsList
{
    public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, IPagedList<ProductViewModel>>
    {
        private readonly IAsyncRepository<Product> _repository;
        private readonly IMapper _mapper;

        public GetProductListQueryHandler(IAsyncRepository<Product> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<IPagedList<ProductViewModel>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            return await _repository
                .GetAll()
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .ToPagedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
