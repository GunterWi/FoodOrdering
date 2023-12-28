using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoodOrdering63131236.Areas.Admin.Data;
using FoodOrdering63131236.Models;

namespace FoodOrdering63131236.Areas.Admin.Controllers
{
    [CustomAuthor(Roles = "admin")]
    public class DonHangs63131236Controller : Controller
    {
        private FoodOrdering_63131236Entities db = new FoodOrdering_63131236Entities();
        // Quản lý đơn hàng
        public ActionResult OrderManagement()
        {
            List<DONHANG> donHangs = db.DONHANGs.ToList();
            return View(donHangs);
        }
        // Cài đặt trạng thái vận chuyển
        public ActionResult SettingOrder(int id, int type, string DvVanChuyen, string MaVanChuyen, int? PhiShip, string GhiChu)
        {
            // type: 1 hủy đơn, 2 vận chuyển, 3 chuyển trạng thái giao thành công, 4 ghi chú
            DONHANG donHang = db.DONHANGs.Where(x => x.ID == id).First();
            if (type == 1)
            {
                donHang.ThanhCong = false;
                donHang.GhiChuShop = GhiChu;
            }
            else if (type == 2)
            {
                donHang.DvVanChuyen = DvVanChuyen;
                donHang.MaVanChuyen = MaVanChuyen;
                donHang.PhiShip = PhiShip;
            }
            else if (type == 3)
            {
                donHang.NgayGiaoHang = DateTime.Now;
                donHang.ThanhCong = true;
            }
            else if (type == 4)
            {
                donHang.GhiChuShop = GhiChu;
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e.EntityValidationErrors);
            }

            return RedirectToAction("OrderManagement");
        }
        // Chi tiết đơn hàng
        public ActionResult DetailOrder(int id)
        {
            List<CHITIETDH> chiTietDHs = db.CHITIETDHs.Where(x => x.MaDH == id).ToList();
            ViewBag.donHang = db.DONHANGs.Where(x => x.ID == id).First();
            return View(chiTietDHs);
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
