using AutoMapper;
using InventorySystem.Application.Features.Invoices.Commands.CreateInvoice;
using InventorySystem.Application.Features.Invoices.Commands.UpdateInvoice;
using InventorySystem.Application.Features.Invoices.Queries.ViewModels;
using InventorySystem.Domain.Entities.Invoices;

namespace InventorySystem.Application.Mappings
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, InvoiceViewModel>().ReverseMap();
            CreateMap<Invoice, CreateInvoiceCommand>().ReverseMap();
            CreateMap<Invoice, UpdateInvoiceCommand>().ReverseMap();
        }
    }
}
