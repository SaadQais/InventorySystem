using InventorySystem.Application.Features.Invoices.Commands.CreateInvoice;
using InventorySystem.Application.Features.Invoices.Commands.DeleteInvoice;
using InventorySystem.Application.Features.Invoices.Commands.UpdateInvoice;
using InventorySystem.Application.Features.Invoices.Queries.GetInvoicesById;
using InventorySystem.Application.Features.Invoices.Queries.GetInvoicesList;
using InventorySystem.Application.Features.Invoices.Queries.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.WebMVC.Controllers
{
    [Authorize]
    public class IncomingInvoicesController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMediator _mediator;

        public IncomingInvoicesController(IWebHostEnvironment hostEnvironment, IMediator mediator)
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInvoiceCommand Invoice)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(Invoice);
                return RedirectToAction(nameof(Index));
            }

            return View(Invoice);
        }

        public async Task<IActionResult> Edit(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateInvoiceCommand Invoice)
        {
            if (id != Invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(Invoice);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Invoice);
        }

        public async Task<IActionResult> Delete(int? id)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteInvoiceCommand { Id = id });
            return RedirectToAction(nameof(Index));
        }
    }
}
