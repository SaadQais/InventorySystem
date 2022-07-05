using AutoMapper;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Exceptions;
using InventorySystem.Domain.Entities.DirectEntries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace InventorySystem.Application.Features.DirectEntries.Commands.UpdateDirectEntry
{
    public class UpdateDirectEntryCommandHandler : IRequestHandler<UpdateDirectEntryCommand>
    {
        private readonly IAsyncRepository<DirectEntry> _repository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateDirectEntryCommandHandler> _logger;

        public UpdateDirectEntryCommandHandler(IAsyncRepository<DirectEntry> repository, IWarehouseRepository warehouseRepository, IMapper mapper, 
            ILogger<UpdateDirectEntryCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _warehouseRepository = warehouseRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(UpdateDirectEntryCommand request, CancellationToken cancellationToken)
        {
            var directEntryToUpdate = await _repository.GetByIdAsync(request.Id, new List<Func<IQueryable<DirectEntry>, IIncludableQueryable<DirectEntry, object>>>
            {
                i => i.Include(e => e.DirectEntryProducts)
            });

            if (directEntryToUpdate == null)
            {
                throw new NotFoundException(nameof(DirectEntry), request.Id);
            }

            await _warehouseRepository.UpdateWhenUpdateDirectEntryAsync(directEntryToUpdate, _mapper.Map<DirectEntry>(request));

            _mapper.Map(request, directEntryToUpdate, typeof(UpdateDirectEntryCommand), typeof(DirectEntry));

            await _repository.UpdateAsync(directEntryToUpdate);

            _logger.LogInformation($"DirectEntry {directEntryToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}
