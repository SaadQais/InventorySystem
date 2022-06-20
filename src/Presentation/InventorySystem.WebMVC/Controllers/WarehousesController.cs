using InventorySystem.Application.Features.Warehouses.Commands.CreateWarehouse;
using InventorySystem.Application.Features.Warehouses.Commands.DeleteWarehouse;
using InventorySystem.Application.Features.Warehouses.Commands.UpdateWarehouse;
using InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesById;
using InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesList;
using InventorySystem.Application.Features.Warehouses.Queries.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagingLands.WebMVC.Controllers
{
    //[Authorize]
    public class WarehousesController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMediator _mediator;

        public WarehousesController(IWebHostEnvironment hostEnvironment, IMediator mediator)
        {
            _hostEnvironment = hostEnvironment;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(SearchViewModel search, int page = 1, int pageSize = 20)
        {
            ViewBag.Current = "Warehouses";

            ViewData["SearchVM"] = search;

            var warehouses = await _mediator.Send(new GetWarehousesListQuery(page, pageSize));

            return View(warehouses);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _mediator.Send(new GetWarehouseByIdQuery { Id = id.Value });

            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateWarehouseCommand warehouse)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(warehouse);
                return RedirectToAction(nameof(Index));
            }

            return View(warehouse);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _mediator.Send(new GetWarehouseByIdQuery { Id = id.Value });

            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateWarehouseCommand warehouse)
        {
            if (id != warehouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(warehouse);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _mediator.Send(new GetWarehouseByIdQuery { Id = id.Value });

            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteWarehouseCommand { Id = id });
            return RedirectToAction(nameof(Index));
        }
    }
}
