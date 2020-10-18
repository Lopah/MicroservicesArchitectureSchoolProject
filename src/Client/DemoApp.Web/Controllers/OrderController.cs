using System;
using System.Threading.Tasks;
using DemoApp.Core.Services.Orders;
using DemoApp.Shared.Events.Orders;
using DemoApp.Web.Models.Orders;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderController(IOrderService orderService, IPublishEndpoint publishEndpoint)
        {
            _orderService = orderService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            //TODO: ViewModel

            return View();
        }

        public ActionResult Create()
        {
            var model = new CreateOrderViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userEvent = new CreateOrderEvent
                {
                    //TODO
                };

                try
                {
                    await _publishEndpoint.Publish<CreateOrderEvent>(userEvent);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error - Create Order Event Publish");
                }
            }

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
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}