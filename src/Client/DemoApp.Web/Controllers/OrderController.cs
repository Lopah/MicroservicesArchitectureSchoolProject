using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View(new List<OrderViewModel>());
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderViewModel model)
        {
            try
            {

                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}