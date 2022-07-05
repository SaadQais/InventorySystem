using InventorySystem.Application.Features.DirectEntries.Commands.CreateDirectEntry;
using InventorySystem.Application.Features.DirectEntries.Commands.DeleteDirectEntry;
using InventorySystem.Application.Features.DirectEntries.Commands.UpdateDirectEntry;
using InventorySystem.Application.Features.DirectEntries.Models;
using InventorySystem.Application.Features.DirectEntries.Queries.GetDirectEntriesById;
using InventorySystem.Application.Features.DirectEntries.Queries.GetDirectEntriesList;
using InventorySystem.Application.Features.Products.Queries.GetProductsList;
using InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesList;
using InventorySystem.Application.Models.Role;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.WebMVC.Areas.DirectEntries
{
    [Authorize]
    [Area("DirectEntries")]
    public class OutgoingController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMediator _mediator;

        public OutgoingController(IWebHostEnvironment hostEnvironment, IMediator mediator)
        {
            _hostEnvironment = hostEnvironment;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DirectEntrySearchModel search, int page = 1, int pageSize = 20)
        {
            ViewBag.Current = "OutgoingDirectEntries";

            ViewData["SearchVM"] = search;

            var directEntries = await _mediator.Send(new GetDirectEntryListQuery(page, pageSize, Domain.Enums.DirectEntryType.Outgoing));

            return View(directEntries);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var directEntry = await _mediator.Send(new GetDirectEntryByIdQuery { Id = id.Value });

            if (directEntry == null)
            {
                return NotFound();
            }

            return View(directEntry);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["WarehouseId"] = new SelectList(await _mediator.Send(new GetWarehouseListQuery(1, int.MaxValue)), "Id", "Name");
            ViewData["Products"] = new SelectList(await _mediator.Send(new GetProductListQuery(1, int.MaxValue)), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDirectEntryCommand directEntry)
        {
            if (ModelState.IsValid)
            {
                directEntry.Type = Domain.Enums.DirectEntryType.Outgoing;
                await _mediator.Send(directEntry);
                return RedirectToAction(nameof(Index));
            }

            ViewData["WarehouseId"] = new SelectList(RolesVM.GetAll(), await _mediator.Send(new GetWarehouseListQuery(1, int.MaxValue)));
            ViewData["Products"] = new SelectList(await _mediator.Send(new GetProductListQuery(1, int.MaxValue)), "Id", "Name");

            return View(directEntry);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var directEntry = await _mediator.Send(new GetDirectEntryByIdQuery { Id = id.Value });

            if (directEntry == null)
            {
                return NotFound();
            }

            ViewData["WarehouseId"] = new SelectList(await _mediator.Send(new GetWarehouseListQuery(1, 20)), 
                "Id", "Name", directEntry.Warehouse.Id);

            return View(directEntry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateDirectEntryCommand directEntry)
        {
            if (id != directEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    directEntry.Type = Domain.Enums.DirectEntryType.Outgoing;
                    await _mediator.Send(directEntry);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["WarehouseId"] = new SelectList(await _mediator.Send(new GetWarehouseListQuery(1, 20)),
               "Id", "Name", directEntry.WarehouseId);

            return View(directEntry);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var directEntry = await _mediator.Send(new GetDirectEntryByIdQuery { Id = id.Value });

            if (directEntry == null)
            {
                return NotFound();
            }

            return View(directEntry);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteDirectEntryCommand { Id = id });
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            var products = await _mediator.Send(new GetProductListQuery(1, int.MaxValue));

            var data = products.Where(a => a.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                .ToList().AsReadOnly();

            return Ok(data);
        }
    }
}
