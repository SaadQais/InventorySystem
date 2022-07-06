using InventorySystem.Application.Features.Invoices.Queries.GetInvoicesCount;
using InventorySystem.Application.Features.Products.Queries.GetProductsCount;
using InventorySystem.Application.Features.Warehouses.Queries.GetWarehousesCount;
using InventorySystem.Domain.Entities;
using InventorySystem.WebMVC.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InventorySystem.WebMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(IMediator mediator, ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager)
        {
            _mediator = mediator;
            _logger = logger;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Current = "Dashboard";

            ViewBag.InvoicesCount = await _mediator.Send(new GetInvoiceCountQuery());
            ViewBag.WarehousesCount = await _mediator.Send(new GetWarehouseCountQuery());
            ViewBag.ProductsCount = await _mediator.Send(new GetProductCountQuery());

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
{
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}