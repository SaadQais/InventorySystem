using InventorySystem.Application.Features.Products.Queries.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Application.Features.Warehouses.Queries.ViewModels
{
    public class WarehouseProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "العدد")]
        public int Count { get; set; }

        public ProductViewModel Product { get; set; }
        public WarehouseViewModel Warehouse { get; set; }
    }
}
