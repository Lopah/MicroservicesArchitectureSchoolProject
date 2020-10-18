using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Core.Models.Products;
using DemoApp.Core.Services.Products;
using DemoApp.Infrastructure;
using DemoApp.Shared.Events.Products;
using DemoApp.Shared.Events.Users;
using DemoApp.Web.Models;
using DemoApp.Web.Models.Products;
using DemoApp.Web.Models.Users;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductController(IProductService productService, IPublishEndpoint publishEndpoint)
        {
            _productService = productService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            var model = new ProductsViewModel(products);
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new CreateProductViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var productEvent = new CreateProductEvent
                {
                    Name = model.Name,
                    Amount = model.Amount,
                    Price = model.Price
                };

                try
                {
                    await _publishEndpoint.Publish<CreateProductEvent>(productEvent);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error - Create Product Event Publish" );
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product is null)
                return NotFound();

            var model = new EditProductViewModel(product);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var productEvent = new EditProductEvent()
                {
                    Id = id,
                    Name = model.Name,
                    Amount = model.Amount,
                    Price = model.Price
                };

                try
                {
                    await _publishEndpoint.Publish<EditProductEvent>(productEvent);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error - Update User Event Publish" );
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var productEvent = new DeleteProductEvent()
            {
                Id = id
            };

            try
            {
                await _publishEndpoint.Publish<DeleteProductEvent>(productEvent);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}