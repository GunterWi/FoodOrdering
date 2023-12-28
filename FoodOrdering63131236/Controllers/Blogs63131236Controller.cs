using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using FoodOrdering63131236.Models;
using PagedList;

namespace FoodOrdering63131236.Controllers
{
    public class Blogs63131236Controller : Controller
    {
        private FoodOrdering_63131236Entities db = new FoodOrdering_63131236Entities();

        // GET: Blogs63131236
        public ActionResult Index(int? page)
        {
            int pageNum = page ?? 1;
            List<BAIVIET> baiViets = db.BAIVIETs.Where(x => x.HienThi).OrderByDescending(x => x.NgayDang).ToList();
            return View(baiViets.ToPagedList(pageNum, 3));
        }
        // Thông tin bài viết 
        public ActionResult Single(string id)
        {
            var baiViets = db.BAIVIETs;
            BAIVIET baiViet = (from bv in baiViets where bv.TenDuongDan == id && bv.HienThi select bv).First();
            baiViet.SoLanDoc++;
            db.SaveChanges();
            return View(baiViet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
