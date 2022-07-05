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
        public async Task<IActionResult> Index(DirectEntrySearchModel search, int page = 1, int pageSize = 20)
        {
            ViewBag.Current = "IncomingDirectEntries";

            ViewData["SearchVM"] = search;

            var directEntries = await _mediator.Send(new GetDirectEntryListQuery(page, pageSize, Domain.Enums.DirectEntryType.Incoming));

            return View(directEntries);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var DirectEntry = await _mediator.Send(new GetDirectEntryByIdQuery { Id = id.Value });

            if (DirectEntry == null)
            {
                return NotFound();
            }

            return View(DirectEntry);
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
                directEntry.Type = Domain.Enums.DirectEntryType.Incoming;
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
                    directEntry.Type = Domain.Enums.DirectEntryType.Incoming;
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
    }
}
