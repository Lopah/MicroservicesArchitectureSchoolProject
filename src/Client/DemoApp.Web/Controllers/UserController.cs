﻿using System.Threading.Tasks;
using DemoApp.Core.Events;
using DemoApp.Core.Services;
using DemoApp.Web.Models;
using DemoApp.Web.Models.Users;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using CreateUserEvent = DemoApp.Shared.Events.Users.CreateUserEvent;

namespace DemoApp.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPublishEndpoint _publishEndpoint;

        public UserController(IUserService userService, IPublishEndpoint publishEndpoint)
        {
            _userService = userService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            var model = new UsersViewModel(users);
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
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error - Create User Event Publish" );
                }
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public ActionResult Edit()
        {
            var model = new EditUserViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, EditUserViewModel model)
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
                    await _publishEndpoint.Publish<EditUserViewModel>(userEvent);
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error - Create User Event Publish" );
                }
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}