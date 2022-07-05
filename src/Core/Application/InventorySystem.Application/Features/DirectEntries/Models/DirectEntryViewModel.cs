using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using InventorySystem.Domain.Entities.DirectEntries;
using InventorySystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Application.Features.DirectEntries.Models
{
    public class DirectEntryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "نوع الأدخال")]
        public DirectEntryType Type { get; set; }

        [Display(Name = "الوصف")]
        public string Description { get; set; }

        public WarehouseViewModel Warehouse { get; set; }

        [Display(Name = "المخزن")]
        public int WarehouseId { get; set; }

        [Display(Name = "مواد الوصل")]
        public ICollection<DirectEntryProduct> DirectEntryProducts { get; set; }
    }
}
