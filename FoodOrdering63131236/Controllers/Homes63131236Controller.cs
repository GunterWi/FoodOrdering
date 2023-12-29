using FoodOrdering63131236.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodOrdering63131236.Controllers
{
    public class Homes63131236Controller : Controller
    {
        public ActionResult Index()
        {
            FoodOrdering_63131236Entities db = new FoodOrdering_63131236Entities();
            var monAns = db.MONANs;
            var loaiMons = db.LOAIMONs;
            ViewBag.monAns = (from sp in monAns where sp.DangMA == true orderby sp.NgayDangMA descending select sp).Take(6);
            ViewBag.loaiMons = loaiMons;
            return View();
        }
        public ActionResult Admin()
        {
            return RedirectToAction("Index", "Dashboards63131236", new { area = "Admin" });
        }

    }
}