using InventorySystem.Domain.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace InventorySystem.Application.Features.Invoices.Commands.UpdateInvoice
{
    public class UpdateInvoiceCommand : IRequest
{
        public int Id { get; set; }

        [Display(Name = "رقم الوصل")]
        public string Number { get; set; }

        public InvoiceType Type { get; set; }

        [Display(Name = "المزود")]
        public string Supplier { get; set; }

        [Display(Name = "الوصف")]
        public string Description { get; set; }

        public int WarehouseId { get; set; }
    }
}
