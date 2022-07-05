using MediatR;

namespace InventorySystem.Application.Features.DirectEntries.Commands.DeleteDirectEntry
{
    public class DeleteDirectEntryCommand : IRequest
    {
        public int Id { get; set; }
    }
}
