using AutoMapper;
using InventorySystem.Application.Features.DirectEntries.Commands.CreateDirectEntry;
using InventorySystem.Application.Features.DirectEntries.Commands.UpdateDirectEntry;
using InventorySystem.Application.Features.DirectEntries.Models;
using InventorySystem.Domain.Entities.DirectEntries;

namespace InventorySystem.Application.Mappings
{
    public class DirectEntriesProfile : Profile
    {
        public DirectEntriesProfile()
        {
            CreateMap<DirectEntry, DirectEntryViewModel>().ReverseMap();
            CreateMap<DirectEntry, CreateDirectEntryCommand>().ReverseMap();
            CreateMap<DirectEntry, UpdateDirectEntryCommand>().ReverseMap();
            CreateMap<DirectEntryProduct, DirectEntryProductModel>().ReverseMap();
        }
    }
}
