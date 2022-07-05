using AutoMapper;
using InventorySystem.Application.Features.Invoices.Commands.CreateInvoice;
using InventorySystem.Application.Features.Invoices.Commands.UpdateInvoice;
using InventorySystem.Application.Features.Invoices.Models;
using InventorySystem.Domain.Entities.Invoices;

namespace InventorySystem.Application.Mappings
{
    public class InvoicesProfile : Profile
    {
        public InvoicesProfile()
        {
            CreateMap<Invoice, InvoiceViewModel>().ReverseMap();
            CreateMap<Invoice, CreateInvoiceCommand>().ReverseMap();
            CreateMap<Invoice, UpdateInvoiceCommand>().ReverseMap();
            CreateMap<InvoiceProduct, InvoiceProductModel>().ReverseMap();
        }
    }
}
