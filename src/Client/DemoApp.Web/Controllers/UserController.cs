using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Infrastructure;
using DemoApp.Infrastructure.Events;
using DemoApp.Shared.Config;
using DemoApp.Shared.Events;
using DemoApp.Web.Models;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

namespace DemoApp.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ISendEndpointProvider _endpointProvider;
        private readonly RabbitMqSettings _settings;

        public UserController(ApplicationDbContext dbContext, IOptions<RabbitMqSettings> settings, ISendEndpointProvider endpointProvider)
        {
            _dbContext = dbContext;
            _endpointProvider = endpointProvider;
            _settings = settings?.Value;
        }

        // GET: Account
        public IActionResult Index()
        {
            var test = new List<(int, string)>()
            {
                (1, "Pepa"),
                (2, "Franta")
            };

            var model = new UsersViewModel(test);
            return View(model);
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            var model = new CreateUserViewModel();
            return View(model);
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add insert logic here
                var userEvent = new CreateUserEvent
                {
                    Name = model.Name,
                    Username = model.Username,
                    Password = model.Password
                };

                try
                {
                    await SendMessage(userEvent);
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error - Create User Event Publish" );
                }
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task SendMessage<T>(T message)
        {
            var endpoint = $"rabbitmq://{_settings.Hostname}:{_settings.Port}/{_settings.Endpoint}?durable=false";
            var finalEndpoint = await _endpointProvider.GetSendEndpoint(new Uri(endpoint));
            await finalEndpoint.Send(message);
        }
    }
}