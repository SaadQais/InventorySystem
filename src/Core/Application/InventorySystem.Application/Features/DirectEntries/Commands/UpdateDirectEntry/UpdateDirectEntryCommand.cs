using InventorySystem.Application.Features.DirectEntries.Models;
using InventorySystem.Domain.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Application.Features.DirectEntries.Commands.UpdateDirectEntry
{
    public class UpdateDirectEntryCommand : IRequest
{
        public int Id { get; set; }

        public DirectEntryType Type { get; set; }

        [Display(Name = "الوصف")]
        public string Description { get; set; }

        [Display(Name = "المخزن")]
        public int WarehouseId { get; set; }

        [Display(Name = "مواد الوصل")]
        public List<DirectEntryProductModel> DirectEntryProducts { get; set; }
    }
}
