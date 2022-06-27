using InventorySystem.Application.Features.Invoices.Models;
using MediatR;

namespace InventorySystem.Application.Features.Invoices.Queries.GetInvoicesById
{
    public class GetInvoiceByIdQuery : IRequest<InvoiceViewModel>
    {
        public int Id { get; set; }
    }
}
