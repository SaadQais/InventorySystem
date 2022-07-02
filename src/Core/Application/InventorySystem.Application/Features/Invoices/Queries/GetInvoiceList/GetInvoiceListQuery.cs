using InventorySystem.Application.Features.Invoices.Models;
using InventorySystem.Domain.Enums;
using MediatR;
using X.PagedList;

namespace InventorySystem.Application.Features.Invoices.Queries.GetInvoicesList
{
    public class GetInvoiceListQuery : IRequest<IPagedList<InvoiceViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public InvoiceType InvoiceType { get; set; }

        public GetInvoiceListQuery(int? pageNumber, int? pageSize, InvoiceType invoiceType)
        {
            PageNumber = pageNumber ?? 1;
            PageSize = pageSize ?? 20;
            InvoiceType = invoiceType;
        }
    }
}
