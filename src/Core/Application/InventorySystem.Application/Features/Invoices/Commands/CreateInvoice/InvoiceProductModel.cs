namespace InventorySystem.Application.Features.Invoices.Commands.CreateInvoice
{
    public class InvoiceProductModel
    {
        public int ProductId { get; set; }
        public int InvoiceId { get; set; }
        public int Count { get; set; }
    }
}
