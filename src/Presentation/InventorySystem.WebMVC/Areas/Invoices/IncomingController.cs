using InventorySystem.Application.Features.Invoices.Commands.CreateInvoice;
using InventorySystem.Application.Features.Invoices.Commands.DeleteInvoice;
using InventorySystem.Application.Features.Invoices.Commands.UpdateInvoice;
using InventorySystem.Application.Features.Invoices.Queries.GetInvoicesById;
using InventorySystem.Application.Features.Invoices.Queries.GetInvoicesList;
using InventorySystem.Application.Features.Invoices.Queries.ViewModels;
using InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesList;
using InventorySystem.Application.Models.Role;
using InventorySystem.Domain.Entities.Invoices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.WebMVC.Areas.Invoices
{
    [Authorize]
    [Area("Invoices")]
    public class IncomingController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMediator _mediator;

        public IncomingController(IWebHostEnvironment hostEnvironment, IMediator mediator)
        {
            _hostEnvironment = hostEnvironment;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(InvoiceSearchModel search, int page = 1, int pageSize = 20)
        {
            ViewBag.Current = "Invoices";

            ViewData["SearchVM"] = search;

            var Invoices = await _mediator.Send(new GetInvoiceListQuery(page, pageSize));

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
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["WarehouseId"] = new SelectList(await _mediator.Send(new GetWarehouseListQuery(1, 20)), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInvoiceCommand invoice)
        {
            if (ModelState.IsValid)
            {
                invoice.Type = Domain.Enums.InvoiceType.Incoming;
                await _mediator.Send(invoice);
                return RedirectToAction(nameof(Index));
            }

            ViewData["WarehouseId"] = new SelectList(RolesVM.GetAll(), await _mediator.Send(new GetWarehouseListQuery(1, 20)));

            return View(invoice);
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

            ViewData["WarehouseId"] = new SelectList(await _mediator.Send(new GetWarehouseListQuery(1, 20)), 
                "Id", "Name", invoice.Warehouse.Id);
            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateInvoiceCommand invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    invoice.Type = Domain.Enums.InvoiceType.Incoming;
                    await _mediator.Send(invoice);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["WarehouseId"] = new SelectList(await _mediator.Send(new GetWarehouseListQuery(1, 20)),
               "Id", "Name", invoice.WarehouseId);

            return View(invoice);
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
