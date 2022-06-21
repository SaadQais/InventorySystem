using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Exceptions;
using InventorySystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventorySystem.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IAsyncRepository<Product> _repository;
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        public DeleteProductCommandHandler(IAsyncRepository<Product> repository, ILogger<DeleteProductCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var ProductToDelete = await _repository.GetByIdAsync(request.Id);
            if (ProductToDelete == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            await _repository.DeleteAsync(ProductToDelete);
            _logger.LogInformation($"Product {ProductToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
