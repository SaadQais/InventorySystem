using InventorySystem.Domain.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace InventorySystem.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest
{
        public int Id { get; set; }

        [Display(Name = "الأسم")]
        public string Name { get; set; }

        [Display(Name = "الرمز")]
        public string Code { get; set; }

        [Display(Name = "الباركود")]
        public string Barcode { get; set; }

        [Display(Name = "التسلسل")]
        public string SerialNumber { get; set; }

        [Display(Name = "النوع")]
        public ProductType Type { get; set; }

        [Display(Name = "وحده القياس")]
        public UOM UOM { get; set; }

        [Display(Name = "الوصف")]
        public string Description { get; set; }
    }
}
