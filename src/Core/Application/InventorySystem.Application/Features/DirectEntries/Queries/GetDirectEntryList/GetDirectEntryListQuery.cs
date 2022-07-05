using InventorySystem.Application.Features.DirectEntries.Models;
using InventorySystem.Domain.Enums;
using MediatR;
using X.PagedList;

namespace InventorySystem.Application.Features.DirectEntries.Queries.GetDirectEntriesList
{
    public class GetDirectEntryListQuery : IRequest<IPagedList<DirectEntryViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DirectEntryType DirectEntryType { get; set; }

        public GetDirectEntryListQuery(int? pageNumber, int? pageSize, DirectEntryType directEntryType)
        {
            PageNumber = pageNumber ?? 1;
            PageSize = pageSize ?? 20;
            DirectEntryType = directEntryType;
        }
    }
}
