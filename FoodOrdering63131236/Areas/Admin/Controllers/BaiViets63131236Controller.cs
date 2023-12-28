using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoodOrdering63131236.Areas.Admin.Data;
using FoodOrdering63131236.Models;
using PagedList;

namespace FoodOrdering63131236.Areas.Admin.Controllers
{
    [CustomAuthor(Roles = "admin")]
    public class BaiViets63131236Controller : Controller
    {
        private FoodOrdering_63131236Entities db = new FoodOrdering_63131236Entities();

        // Danh sách bài viết
        public ActionResult Blog_List(int? page)
        {
            int pageNum = (page ?? 1);
            List<BAIVIET> baiViets = db.BAIVIETs.ToList();
            return View(baiViets.ToPagedList(pageNum, 10));
        }
        // thêm bài viết 
        public ActionResult Create_Blog()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create_Blog(BAIVIET baiViet, bool preview = false)
        {
            List<BAIVIET> baiViets = db.BAIVIETs.Where(x => x.TenDuongDan == baiViet.TenDuongDan).ToList();
            bool luu = true;
            if (baiViets.Count > 0)
            {
                ModelState["TenDuongDan"].Errors.Add("Tên đường dẫn đã tồn tại");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.TenBV) || baiViet.TenBV.Equals(""))
            {
                ModelState["TenBV"].Errors.Add("Tên bài viết không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.NoiDungBV) || baiViet.NoiDungBV.Equals(""))
            {
                ModelState["NoiDungBV"].Errors.Add("Nội dung bài viết không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.TenDuongDan) || baiViet.TenDuongDan.Equals(""))
            {
                ModelState["TenDuongDan"].Errors.Add("Tên đường dẫn không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.TomTat) || baiViet.TomTat.Equals(""))
            {
                ModelState["TomTat"].Errors.Add("Tóm tắt không được bỏ trống");
                luu = false;
            }
            if (baiViet.AnhBia.Equals(null))
            {
                ModelState["TomTat"].Errors.Add("Ảnh bìa không được bỏ trống");
                luu = false;
            }
            if (luu)
            {
                // xóa xuống dòng
                baiViet.NoiDungBV = baiViet.NoiDungBV.Replace("\n", "").Replace("\r", "");
                baiViet.TomTat = baiViet.TomTat.Replace("\n", "").Replace("\r", "");
                baiViet.NgayDang = DateTime.Now;
                baiViet.NgayCapNhat = DateTime.Now;
                baiViet.SoLanDoc = 0;
                db.BAIVIETs.Add(baiViet);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (preview)
                return Preview(baiViet);
            return RedirectToAction("Blog_List");
        }
        public ActionResult Edit_Blog(int id)
        {
            BAIVIET baiViets = db.BAIVIETs.Where(x => x.ID == id).First();
            return View(baiViets);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit_Blog(BAIVIET baiViet, bool preview = false)
        {
            List<BAIVIET> baiViets = db.BAIVIETs.Where(x => x.TenDuongDan == baiViet.TenDuongDan && x.ID != baiViet.ID).ToList();
            bool luu = true;
            if (baiViets.Count > 0)
            {
                ModelState["TenDuongDan"].Errors.Add("Tên đường dẫn đã tồn tại");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.TenBV) || baiViet.TenBV.Equals(""))
            {
                ModelState["TenBV"].Errors.Add("Tên bài viết không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.NoiDungBV) || baiViet.NoiDungBV.Equals(""))
            {
                ModelState["NoiDungBV"].Errors.Add("Nội dung bài viết không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.TenDuongDan) || baiViet.TenDuongDan.Equals(""))
            {
                ModelState["TenDuongDan"].Errors.Add("Tên đường dẫn không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.TomTat) || baiViet.TomTat.Equals(""))
            {
                ModelState["TomTat"].Errors.Add("Tóm tắt không được bỏ trống");
                luu = false;
            }
            if (baiViet.AnhBia.Equals(null))
            {
                ModelState["TomTat"].Errors.Add("Ảnh bìa không được bỏ trống");
                luu = false;
            }
            if (luu)
            {
                // xóa xuống dòng
                BAIVIET baiViet2 = db.BAIVIETs.Where(x => x.ID == baiViet.ID).First();
                baiViet2.TenBV = baiViet.TenBV;
                baiViet2.TenDuongDan = baiViet.TenDuongDan;
                baiViet2.NoiDungBV = baiViet.NoiDungBV.Replace("\n", "").Replace("\r", "");
                baiViet2.TomTat = baiViet.TomTat.Replace("\n", "").Replace("\r", "");
                baiViet2.AnhBia = baiViet.AnhBia;
                baiViet2.HienThi = baiViet.HienThi;
                baiViet2.NgayCapNhat = DateTime.Now;
                try
                {
                    db.SaveChanges();
                    if (preview)
                        return Preview(baiViet);
                    return RedirectToAction("Blog_List");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return View();
        }

        // xóa bài viết
        public ActionResult Remove_Blog(int id)
        {
            BAIVIET baiViet = db.BAIVIETs.Where(x => x.ID == id).First();
            db.BAIVIETs.Remove(baiViet);
            db.SaveChanges();
            return RedirectToAction("Blog_List");
        }
        // Search Blog
        public ActionResult Search_Blog(string search, int? page)
        {
            int pageNum = (page ?? 1);
            List<BAIVIET> baiViets = db.BAIVIETs.Where(x => DbFunctions.Like(x.TenBV, "%" + search + "%")).ToList();
            ViewBag.search = search;
            return View("Blog_List", baiViets.ToPagedList(pageNum, 10));
        }
        private ActionResult Preview(BAIVIET baiViet)
        {
            return View("~/Views/Blogs63131236/Single.cshtml", baiViet);
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
