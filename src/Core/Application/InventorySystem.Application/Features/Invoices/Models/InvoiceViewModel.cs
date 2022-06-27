using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using InventorySystem.Domain.Entities.Invoices;
using InventorySystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Application.Features.Invoices.Models
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }

        [Display(Name = "رقم الوصل")]
        public string Number { get; set; }

        [Display(Name = "نوع الوصل")]
        public InvoiceType Type { get; set; }

        [Display(Name = "المزود")]
        public string Supplier { get; set; }

        [Display(Name = "الوصف")]
        public string Description { get; set; }

        public WarehouseViewModel Warehouse { get; set; }

        [Display(Name = "المخزن")]
        public int WarehouseId { get; set; }

        [Display(Name = "مواد الوصل")]
        public ICollection<InvoiceProduct> InvoiceProducts { get; set; }
    }
}
