using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Core.Services.Orders;
using DemoApp.Core.Services.Products;
using DemoApp.Core.Services.Users;
using DemoApp.Shared.Events.Orders;
using DemoApp.Web.Models.Orders;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderController(IOrderService orderService, IUserService userService, IProductService productService, IPublishEndpoint publishEndpoint)
        {
            _orderService = orderService;
            _userService = userService;
            _productService = productService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IActionResult> Index(bool success = false)
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var model = new OrdersViewModel(orders);
            model.ShowSuccess = success;
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var users = await _userService.GetAllUsersAsync();
            var products = await _productService.GetAllProductsAsync();
            var model = new CreateOrderViewModel(users, products);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var filteredProducts = model.Products.Where(p => p.Amount > 0).ToList();
                var userEvent = new CreateOrderEvent
                {
                    UserId = model.UserId,
                    Products = filteredProducts.Select(p => new CreateOrderEvent.CreateOrderEventProductDto(p.Id, p.Amount)).ToList()
                };

                try
                {
                    await _publishEndpoint.Publish<CreateOrderEvent>(userEvent);
                    return RedirectToAction(nameof(Index), new { success = true });
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error - Create Order Event Publish");
                }
            }

            model.SetUserList(await _userService.GetAllUsersAsync());
            model.SetProductList(await _productService.GetAllProductsAsync());
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var userEvent = new DeleteOrderEvent()
            {
                Id = id
            };

            try
            {
                await _publishEndpoint.Publish<DeleteOrderEvent>(userEvent);
                return RedirectToAction(nameof(Index), new { success = true });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}