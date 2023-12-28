using FoodOrdering63131236.Areas.Admin.Data;
using FoodOrdering63131236.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodOrdering63131236.Areas.Admin.Controllers
{
    [CustomAuthor(Roles = "admin")]
    public class Dashboards63131236Controller : Controller
    {
        // GET: Admin/Dashboards63131236
        private FoodOrdering_63131236Entities db = new FoodOrdering_63131236Entities();

        public ActionResult Index()
        {
            IEnumerable<TINHTHANH> ta = db.TINHTHANHs;
            ViewBag.tinh = from t in ta where t.ID == 1 select t;
            ViewBag.NewOrder = db.DONHANGs.Where(x => x.MaVanChuyen == null && x.ThanhCong == null).ToList().Count;
            ViewBag.DonThanhCong = db.DONHANGs.Where(x => x.ThanhCong == true).ToList().Count;
            ViewBag.SoNguoiDung = db.TAIKHOANs.ToList().Count;
            var donHangTrong7Ngay = db.GetTotalSumIn7days().ToList();
            ViewBag.DonHangTrongTuan = donHangTrong7Ngay;
            List<IGrouping<int, DONHANG>> donTheoThang = db.DONHANGs.ToList()
                                                                    .Where(dh => dh.NgayDatHang.Year == DateTime.Now.Year) // Chỉ lấy đơn hàng trong năm hiện tại
                                                                    .GroupBy(dh => dh.NgayDatHang.Month) // Nhóm theo tháng
                                                                    .OrderBy(g => g.Key) // Sắp xếp theo tháng
                                                                    .ToList();
            ViewBag.DonHangTheoThang = donTheoThang;
            return View();
        }
    }
}