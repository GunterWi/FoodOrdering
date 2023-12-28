using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoodOrdering63131236.Areas.Admin.Data;
using FoodOrdering63131236.Models;

namespace FoodOrdering63131236.Areas.Admin.Controllers
{
    [CustomAuthor(Roles = "admin")]
    public class DanhMucs63131236Controller : Controller
    {
        private FoodOrdering_63131236Entities db = new FoodOrdering_63131236Entities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DANHMUC dm)
        {
            bool luu = true;
            if (String.IsNullOrEmpty(dm.TenDanhMuc))
            {
                ModelState["TenDanhMuc"].Errors.Add("Tên danh mục không được để trống");
                luu = false;
            }
           /* if (String.IsNullOrEmpty(dm.TenDuongDan))
            {
                ModelState["TenDuongDan"].Errors.Add("Tên đường dẫn không được để trống");
                luu = false;
            }*/
           /* if (db.LOAIMONs.Where(x => x.TenDuongDan == dm.TenDuongDan).ToList().Count > 0)
            {
                ModelState["TenDuongDan"].Errors.Add("Tên đường dẫn đã tồn tại");
                luu = false;
            }*/
            // lưu đữ liệu
            if (luu)
            {
                try
                {
                    db.DANHMUCs.Add(dm);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/LoaiMons63131236/Edit/5
        public ActionResult Edit(DANHMUC dm)
        {
            DANHMUC danhmuc = db.DANHMUCs.Where(x => x.ID == dm.ID).First();
            danhmuc.TenDanhMuc = dm.TenDanhMuc;
            /*danhmuc.TenDuongDan = dm.TenDuongDan;*/
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            DANHMUC dm = db.DANHMUCs.Where(x => x.ID == id).First();
            try
            {
                db.DANHMUCs.Remove(dm);
                db.SaveChanges();
                // Lấy giá trị ID lớn nhất từ bảng LOAIMON
                var maxId = db.DANHMUCs.Max(p => (int?)p.ID) ?? 0;
                // Đặt lại giá trị IDENTITY sử dụng giá trị maxId
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('DANHMUC', RESEED, @maxId)", new SqlParameter("maxId", maxId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction("Index");
        }
        // Thực hiện ajax để lấy dữ liệu cho chỉnh sửa thể loại 
        [HttpPost]
        public ActionResult Ajax_Category(int id)
        {
            DANHMUC danhmuc = db.DANHMUCs.Where(x => x.ID == id).First();
            try
            {
                return Json(new
                {
                    TenDanhMuc = danhmuc.TenDanhMuc,
                    /*TenDuongDan = danhmuc.TenDuongDan*/
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public ActionResult Search_Category(string search, int? page)
        {
            int pageNum = (page ?? 1);
            List<LOAIMON> lOAIMAs = db.LOAIMONs.Where(x => DbFunctions.Like(x.TenLoai, "%" + search + "%")).ToList();
            return View("Index", lOAIMAs.ToList());
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
