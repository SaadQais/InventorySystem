using InventorySystem.Application.Features.DirectEntries.Models;
using MediatR;

namespace InventorySystem.Application.Features.DirectEntries.Queries.GetDirectEntriesById
{
    public class GetDirectEntryByIdQuery : IRequest<DirectEntryViewModel>
    {
        public int Id { get; set; }
    }
}
