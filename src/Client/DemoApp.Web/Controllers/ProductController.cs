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
            var products = new List<ProductDto>(); //await _productService.GetAllUsersAsync();
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
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error - Create Product Event Publish" );
                }
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public ActionResult Edit()
        {
            var model = new EditProductViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                // var userEvent = new Edit()
                // {
                //     Id = model.Id,
                //     Name = model.Name,
                //     Username = model.Username,
                //     Password = model.Password
                // };
                //
                // try
                // {
                //     await _publishEndpoint.Publish<EditUserViewModel>(userEvent);
                // }
                // catch
                // {
                //     ModelState.AddModelError(string.Empty, "Error - Create User Event Publish" );
                // }
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}