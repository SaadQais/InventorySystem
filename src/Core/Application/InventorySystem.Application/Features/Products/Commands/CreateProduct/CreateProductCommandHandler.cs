using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventorySystem.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
    {
        private readonly IAsyncRepository<Product> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(IAsyncRepository<Product> repository, IMapper mapper, 
            ILogger<CreateProductCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var Product = _mapper.Map<Product>(request);

            await _repository.AddAsync(Product);

            _logger.LogInformation($"Product {Product.Id} is successfully added.");

            return Unit.Value;
        }
    }
}
