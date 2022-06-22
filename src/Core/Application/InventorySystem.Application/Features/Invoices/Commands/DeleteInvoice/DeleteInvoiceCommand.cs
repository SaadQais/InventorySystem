using MediatR;

namespace InventorySystem.Application.Features.Invoices.Commands.DeleteInvoice
{
    public class DeleteInvoiceCommand : IRequest
    {
        public int Id { get; set; }
    }
}
