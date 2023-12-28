using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodOrdering63131236.Controllers
{
    public class Errors63131236Controller : Controller
    {
        [HttpGet]
        public ActionResult PageNotFound()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Authorize()
        {
            return View();
        }
    }
}