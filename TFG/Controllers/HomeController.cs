using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TFG.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Unete()
        {
            ViewData["Message"] = "Organiza tu centro de forma sencilla";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
