using MediatR;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Application.Features.Warehouses.Commands.CreateWarehouse
{
    public class CreateWarehouseCommand : IRequest
    {
        [Display(Name = "الأسم")]
        public string Name { get; set; }

        [Display(Name = "الموقع")]
        public string Location { get; set; }

        [Display(Name = "الوصف")]
        public string Description { get; set; }
    }
}
