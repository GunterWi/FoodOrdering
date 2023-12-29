using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using FoodOrdering63131236.Models;
using FoodOrdering63131236.Areas.Admin.Data;
using System.Diagnostics;
using System.Data.SqlClient;

namespace FoodOrdering63131236.Areas.Admin.Controllers
{
    [CustomAuthor(Roles = "admin")]
    public class MonAns63131236Controller : Controller
    {
        private FoodOrdering_63131236Entities db = new FoodOrdering_63131236Entities();

        // GET: Admin/MonAns63131236
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            var monAn = db.MONANs;
            List<MONAN> monAns = (from ma in monAn orderby ma.ID ascending select ma).ToList();
            return View(monAns.ToPagedList(pageNum, 10));
        }
        // Create Product
        public ActionResult Create()
        {
            ViewBag.MONAN = new List<MONAN>();
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(MONAN ma, string tva, bool preview = false)
        {
            bool luu = true;
            if (String.IsNullOrEmpty(ma.TenMA) || ma.TenMA.Equals(""))
            {
                ModelState["TenMA"].Errors.Add("Tên món ăn không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(ma.TenDuongDan) || ma.TenDuongDan.Equals(""))
            {
                ModelState["TenDuongDan"].Errors.Add("Tên đường dẫn không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(ma.NdMon) || ma.NdMon.Equals(""))
            {
                ModelState["NdMon"].Errors.Add("Nội dung món ăn không được bỏ trống");
                luu = false;
            }
            List<MONAN> monAns = db.MONANs.Where(x => x.TenDuongDan == ma.TenDuongDan).ToList();
            if (monAns.Count > 0)
            {
                ModelState["TenDuongDan"].Errors.Add("Tên đường dẫn đã tồn tại");
                luu = false;
            }
            if (ma.GiaBan < ma.GiaKM)
            {
                ModelState["GiaBan"].Errors.Add("Giá khuyển mãi không được lớn hơn giá bán");
                luu = false;
            }
            if (ma.SoLuong < 0 || ma.SoLuong == null)
            {
                ModelState["SoLuong"].Errors.Add("Số lượng không nhỏ hơn 0 hoặc không được bỏ trống");
                luu = false;
            }
            if (ma.AnhBia == null)
            {
                ModelState["AnhBia"].Errors.Add("Ảnh bìa không được để trống");
                luu = false;
            }
            if (!luu)
            {
                ViewBag.MONAN = new List<MONAN>();
                return View("Create");
            }
            Trace.WriteLine("Test");
            if (String.IsNullOrEmpty(ma.Dvt) || ma.Dvt.Equals(""))
                ma.Dvt = "Cái";
            ma.NgayDangMA = DateTime.Now;
            ma.NdMon = String.IsNullOrEmpty(ma.NdMon) ? null : ma.NdMon.Replace("\n", "").Replace("\r", "");
            ma.TomTat = String.IsNullOrEmpty(ma.TomTat) ? null : ma.TomTat.Replace("\n", "").Replace("\r", "");
            ma.LuotMua = 0;
            ma.LuotXem = 0;
            db.MONANs.Add(ma);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Create");
            }
            if (tva != null && tva != "")
            {
                string[] tva2 = tva.Split(',');
                foreach (string t in tva2)
                {
                    int idAnh = Convert.ToInt32(t);
                    db.Database.ExecuteSqlCommand("Insert into THUVIENANHMA (MaAnh,MaMA) values (" + idAnh + "," + ma.ID + ")");
                }
            }
            if (preview)
                return Preview(ma.ID);
            return RedirectToAction("Index");
        }
        //preview Món
        private ActionResult Preview(int id)
        {
            FoodOrdering_63131236Entities db = new FoodOrdering_63131236Entities();
            Trace.WriteLine(id);
            var monAns = db.MONANs;
            ViewBag.monAn = monAns.Where(x => x.ID == id).FirstOrDefault();
            Trace.WriteLine(monAns.Where(x => x.ID == id).FirstOrDefault());
            ViewBag.khuyenMai = (from sp in monAns where sp.GiaKM < sp.GiaBan && sp.DangMA select sp).Take(4);
            Trace.WriteLine((from sp in monAns where sp.GiaKM < sp.GiaBan && sp.DangMA select sp).Take(4));
            int loaisp = ViewBag.monAn.MaLoai;
            ViewBag.CungLoai = (from sp in monAns where sp.MaLoai == loaisp && sp.DangMA select sp).Take(4);
            Trace.WriteLine((from sp in monAns where sp.MaLoai == loaisp && sp.DangMA select sp).Take(4));
            return View("~/Views/Products63131236/Product.cshtml");
        }
        public ActionResult Edit(int id)
        {
            // KHÔNG DÙNG ĐƯỢC MODEL LIST
            /*MONAN monAns = db.MONANs.Where(x => x.ID == id).First();
            return View(monAns);*/
            var monAns = db.MONANs;
            ViewBag.MONAN = (from ma in monAns where ma.ID == id select ma).ToList();
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(MONAN ma, string tva, bool preview = false)
        {
            bool luu = true;
            if (String.IsNullOrEmpty(ma.TenMA) || ma.TenMA.Equals(""))
            {
                ModelState["TenMA"].Errors.Add("Tên món ăn không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(ma.TenDuongDan) || ma.TenDuongDan.Equals(""))
            {
                ModelState["TenDuongDan"].Errors.Add("Tên đường dẫn không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(ma.NdMon) || ma.NdMon.Equals(""))
            {
                ModelState["NdMon"].Errors.Add("Nội dung món ăn không được bỏ trống");
                luu = false;
            }
            List<MONAN> monAns = db.MONANs.Where(x => x.TenDuongDan == ma.TenDuongDan && x.ID != ma.ID).ToList();
            if (monAns.Count > 0)
            {
                ModelState["TenDuongDan"].Errors.Add("Tên đường dẫn đã tồn tại");
                luu = false;
            }
            if (ma.GiaBan < ma.GiaKM)
            {
                ModelState["GiaBan"].Errors.Add("Giá khuyển mãi không được lớn hơn giá bán");
                luu = false;
            }
            if (ma.SoLuong < 0 || ma.SoLuong == null)
            {
                ModelState["SoLuong"].Errors.Add("Số lượng không nhỏ hơn 0 hoặc không được bỏ trống");
                luu = false;
            }
            if (!luu)
            {
                ViewBag.MONAN = new List<MONAN>();
                return View("Create");
            }
            Trace.WriteLine("đang chạy.");
            MONAN ma2 = db.MONANs.Where(x => x.ID == ma.ID).First();
            ma2.MaLoai = ma.MaLoai;
            ma2.TenMA = ma.TenMA;
            ma2.TenDuongDan = ma.TenDuongDan;
            ma2.GiaBan = ma.GiaBan;
            ma2.GiaKM = ma.GiaKM;
            ma2.Dvt = ma.Dvt;
            ma2.SoLuong = ma.SoLuong;
            ma2.AnhBia = ma.AnhBia;
            ma2.DangMA = ma.DangMA;
            ma2.NdMon = String.IsNullOrEmpty(ma.NdMon) ? null : ma.NdMon.Replace("\n", "").Replace("\r", "");
            ma2.TomTat = String.IsNullOrEmpty(ma.TomTat) ? null : ma.TomTat.Replace("\n", "").Replace("\r", "");
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            // xóa thư viện ảnh
            db.Database.ExecuteSqlCommand("delete THUVIENANHMA where maMA = " + ma.ID);
            // thêm lại thư viện ảnh
            if (tva != null && tva != "")
            {
                string[] tva2 = tva.Split(',');
                foreach (string t in tva2)
                {
                    int idAnh = Convert.ToInt32(t);
                    db.Database.ExecuteSqlCommand("Insert into THUVIENANHMA (maAnh,maMA) values (" + idAnh + "," + ma2.ID + ")");
                }
            }
            if (preview)
                return Preview(ma.ID);
            return RedirectToAction("Index");
        }

        // GET: Admin/MonAns63131236/Delete/5
        public ActionResult Delete(int id)
        {
            MONAN ma = db.MONANs.Where(x => x.ID == id).First();
            try
            {
                db.MONANs.Remove(ma);
                db.SaveChanges();
                var maxId = db.MONANs.Max(p => (int?)p.ID) ?? 0;
                // Đặt lại giá trị IDENTITY sử dụng giá trị maxId
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('MONAN', RESEED, @maxId)", new SqlParameter("maxId", maxId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Redirect(Url.Action("Index", "MonAns63131236"));
        }
        // Search Product
        public ActionResult Search_Product(string search, int? page)
        {
            int pageNum = (page ?? 1);
            List<MONAN> monAns = db.MONANs.Where(x => DbFunctions.Like(x.TenMA, "%" + search + "%")).ToList();
            ViewBag.search = search;
            return View("Index", monAns.ToPagedList(pageNum, 10));
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
