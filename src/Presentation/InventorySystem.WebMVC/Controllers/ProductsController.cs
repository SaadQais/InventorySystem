using InventorySystem.Application.Features.Products.Commands.CreateProduct;
using InventorySystem.Application.Features.Products.Commands.DeleteProduct;
using InventorySystem.Application.Features.Products.Commands.UpdateProduct;
using InventorySystem.Application.Features.Products.Queries.GetProductsById;
using InventorySystem.Application.Features.Products.Queries.GetProductsList;
using InventorySystem.Application.Features.Products.Queries.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.WebMVC.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMediator _mediator;

        public ProductsController(IWebHostEnvironment hostEnvironment, IMediator mediator)
        {
            _hostEnvironment = hostEnvironment;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ProductSearchModel search, int page = 1, int pageSize = 20)
        {
            ViewBag.Current = "Products";

            ViewData["SearchVM"] = search;

            var Products = await _mediator.Send(new GetProductListQuery(page, pageSize));

            return View(Products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _mediator.Send(new GetProductByIdQuery { Id = id.Value });

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductCommand Product)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(Product);
                return RedirectToAction(nameof(Index));
            }

            return View(Product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _mediator.Send(new GetProductByIdQuery { Id = id.Value });

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateProductCommand Product)
        {
            if (id != Product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(Product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _mediator.Send(new GetProductByIdQuery { Id = id.Value });

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteProductCommand { Id = id });
            return RedirectToAction(nameof(Index));
        }
    }
}
