using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace InventorySystem.Application.Features.Warehouses.Queries.ViewModels
{
    public class WarehouseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "الأسم")]
        public string Name { get; set; }

        [Display(Name = "الموقع")]
        public string Location { get; set; }

        [Display(Name = "الوصف")]
        public string Description { get; set; }
    }
}
