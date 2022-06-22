using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using InventorySystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Application.Features.Invoices.Queries.ViewModels
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }

        [Display(Name = "رقم الوصل")]
        public string Number { get; set; }

        public InvoiceType Type { get; set; }

        [Display(Name = "المزود")]
        public string Supplier { get; set; }

        [Display(Name = "الوصف")]
        public string Description { get; set; }

        public WarehouseViewModel Warehouse { get; set; }
    }
}
