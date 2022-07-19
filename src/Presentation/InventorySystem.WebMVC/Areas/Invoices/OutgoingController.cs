using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using InventorySystem.Application.Features.Invoices.Commands.CreateInvoice;
using InventorySystem.Application.Features.Invoices.Commands.DeleteInvoice;
using InventorySystem.Application.Features.Invoices.Commands.UpdateInvoice;
using InventorySystem.Application.Features.Invoices.Models;
using InventorySystem.Application.Features.Invoices.Queries.GetInvoicesById;
using InventorySystem.Application.Features.Invoices.Queries.GetInvoicesList;
using InventorySystem.Application.Features.Products.Queries.GetProductsById;
using InventorySystem.Application.Features.Products.Queries.GetProductsList;
using InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesList;
using InventorySystem.Application.Models.Role;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InventorySystem.WebMVC.Areas.Invoices
{
    [Authorize]
    [Area("Invoices")]
    public class OutgoingController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateInvoiceCommand> _createValidator;
        private readonly IValidator<UpdateInvoiceCommand> _updateValidator;

        public OutgoingController(IWebHostEnvironment hostEnvironment, IMediator mediator, IMapper mapper, 
            IValidator<CreateInvoiceCommand> createValidator, IValidator<UpdateInvoiceCommand> updateValidator)
        {
            _hostEnvironment = hostEnvironment;
            _mediator = mediator;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(InvoiceSearchModel search, int page = 1, int pageSize = 20)
        {
            ViewBag.Current = "OutgoingInvoices";

            ViewData["SearchVM"] = search;

            var Invoices = await _mediator.Send(new GetInvoiceListQuery(page, pageSize, Domain.Enums.InvoiceType.Outgoing));

            return View(Invoices);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Invoice = await _mediator.Send(new GetInvoiceByIdQuery { Id = id.Value });

            if (Invoice == null)
            {
                return NotFound();
            }

            return View(Invoice);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["WarehouseId"] = new SelectList(await _mediator.Send(new GetWarehouseListQuery(1, int.MaxValue)), "Id", "Name");
            ViewData["Products"] = new SelectList(await _mediator.Send(new GetProductListQuery(1, int.MaxValue)), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInvoiceCommand invoice)
        {
            ValidationResult result = await _createValidator.ValidateAsync(invoice);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);

                ViewData["WarehouseId"] = new SelectList(await _mediator.Send(new GetWarehouseListQuery(1, int.MaxValue)), "Id", "Name");
                ViewData["Products"] = new SelectList(await _mediator.Send(new GetProductListQuery(1, int.MaxValue)), "Id", "Name");

                foreach (var invoiceProduct in invoice.InvoiceProducts)
                    invoiceProduct.Product = await _mediator.Send(new GetProductByIdQuery { Id = invoiceProduct.ProductId });

                return View(invoice);
            }

            invoice.Type = Domain.Enums.InvoiceType.Outgoing;
            await _mediator.Send(invoice);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _mediator.Send(new GetInvoiceByIdQuery { Id = id.Value });

            if (invoice == null)
            {
                return NotFound();
            }

            ViewData["WarehouseId"] = new SelectList(await _mediator.Send(new GetWarehouseListQuery(1, 20)), "Id", "Name", 
                invoice.Warehouse.Id);
            ViewData["Products"] = new SelectList(await _mediator.Send(new GetProductListQuery(1, int.MaxValue)), "Id", "Name");

            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InvoiceViewModel invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            var updateInvoice = _mapper.Map<UpdateInvoiceCommand>(invoice);

            ValidationResult result = await _updateValidator.ValidateAsync(updateInvoice);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);

                invoice = await _mediator.Send(new GetInvoiceByIdQuery { Id = id });

                if (invoice == null)
                {
                    return NotFound();
                }

                ViewData["WarehouseId"] = new SelectList(await _mediator.Send(new GetWarehouseListQuery(1, 20)), "Id", "Name",
                    invoice.Warehouse.Id);
                ViewData["Products"] = new SelectList(await _mediator.Send(new GetProductListQuery(1, int.MaxValue)), "Id", "Name");

                return View(invoice);
            }

            try
            {
                updateInvoice.Type = Domain.Enums.InvoiceType.Outgoing;
                await _mediator.Send(updateInvoice);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _mediator.Send(new GetInvoiceByIdQuery { Id = id.Value });

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteInvoiceCommand { Id = id });
            return RedirectToAction(nameof(Index));
        }
    }
}
