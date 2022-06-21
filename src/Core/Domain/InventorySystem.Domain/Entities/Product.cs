using InventorySystem.Domain.Common;
using InventorySystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace InventorySystem.Domain.Entities
{
    public class Product : EntityBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string SerialNumber { get; set; }
        public ProductType Type { get; set; }
        public UOM UOM { get; set; }
        public string Description { get; set; }
    }
}
