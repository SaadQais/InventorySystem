using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Exceptions;
using InventorySystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventorySystem.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IAsyncRepository<Product> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(IAsyncRepository<Product> repository, IMapper mapper, 
            ILogger<UpdateProductCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var ProductToUpdate = await _repository.GetByIdAsync(request.Id);

            if (ProductToUpdate == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            _mapper.Map(request, ProductToUpdate, typeof(UpdateProductCommand), typeof(Product));

            await _repository.UpdateAsync(ProductToUpdate);

            _logger.LogInformation($"Product {ProductToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}
