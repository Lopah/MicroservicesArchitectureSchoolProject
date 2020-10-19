using System;
using System.Threading.Tasks;
using DemoApp.Core.Services;
using DemoApp.Core.Services.Orders;
using DemoApp.Core.Services.Users;
using DemoApp.Shared.Events.Users;
using DemoApp.Web.Models.Users;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using CreateUserEvent = DemoApp.Shared.Events.Users.CreateUserEvent;

namespace DemoApp.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IPublishEndpoint _publishEndpoint;

        public UserController(IUserService userService, IOrderService orderService, IPublishEndpoint publishEndpoint)
        {
            _userService = userService;
            _orderService = orderService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IActionResult> Index(bool success = false)
        {
            var users = await _userService.GetAllUsersAsync();
            var model = new UsersViewModel(users);
            model.ShowSuccess = success;
            return View(model);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user is null)
                return NotFound();

            var orders = await _orderService.GetOrdersForUserAsync(id);
            var model = new UserDetailViewModel(user, orders);
            return View(model);
        }


        public ActionResult Create()
        {
            var model = new CreateUserViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userEvent = new CreateUserEvent
                {
                    Name = model.Name,
                    Username = model.Username,
                    Password = model.Password
                };

                try
                {
                    await _publishEndpoint.Publish<CreateUserEvent>(userEvent);
                    return RedirectToAction(nameof(Index), new { success = true });
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error - Create User Event Publish" );
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user is null)
                return NotFound();

            var model = new EditUserViewModel(user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userEvent = new EditUserEvent()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Username = model.Username,
                    Password = model.Password
                };

                try
                {
                    await _publishEndpoint.Publish<EditUserEvent>(userEvent);
                    return RedirectToAction(nameof(Index), new { success = true });
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error - Edit User Event Publish" );
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var userEvent = new DeleteUserEvent()
            {
                Id = id
            };

            try
            {
                await _publishEndpoint.Publish<DeleteUserEvent>(userEvent);
                return RedirectToAction(nameof(Index), new { success = true });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}