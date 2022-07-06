using MediatR;

namespace InventorySystem.Application.Features.Invoices.Queries.GetInvoicesCount
{
    public class GetInvoiceCountQuery : IRequest<InvoicesCountViewModel>
    {
    }
}
