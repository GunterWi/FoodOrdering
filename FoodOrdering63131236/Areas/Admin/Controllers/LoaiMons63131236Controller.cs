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
using PagedList;

namespace FoodOrdering63131236.Areas.Admin.Controllers
{
    [CustomAuthor(Roles = "admin")]
    public class LoaiMons63131236Controller : Controller
    {
        private FoodOrdering_63131236Entities db = new FoodOrdering_63131236Entities();

        // GET: Admin/LoaiMons63131236
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(LOAIMON loaiMon)
        {
            bool luu = true;
            if (String.IsNullOrEmpty(loaiMon.TenLoai))
            {
                ModelState["TenLoai"].Errors.Add("Tên thể loại không được để trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(loaiMon.TenDuongDan))
            {
                ModelState["TenDuongDan"].Errors.Add("Tên đường dẫn không được để trống");
                luu = false;
            }
            if (db.LOAIMONs.Where(x => x.TenDuongDan == loaiMon.TenDuongDan).ToList().Count > 0)
            {
                ModelState["TenDuongDan"].Errors.Add("Tên đường dẫn đã tồn tại");
                luu = false;
            }
            // lưu đữ liệu
            if (luu)
            {
                try
                {
                    db.LOAIMONs.Add(loaiMon);
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
        public ActionResult Edit(LOAIMON loaiMon)
        {
            LOAIMON loaiMon1 = db.LOAIMONs.Where(x => x.ID == loaiMon.ID).First();
            loaiMon1.TenLoai = loaiMon.TenLoai;
            loaiMon1.TenDuongDan = loaiMon.TenDuongDan;
            loaiMon1.ID_DanhMuc = loaiMon.ID_DanhMuc;
            loaiMon1.GhiChu = loaiMon.GhiChu;
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
            LOAIMON loai = db.LOAIMONs.Where(x => x.ID == id).First();
            try
            {
                db.LOAIMONs.Remove(loai);
                db.SaveChanges();
                // Lấy giá trị ID lớn nhất từ bảng LOAIMON
                var maxId = db.LOAIMONs.Max(p => (int?)p.ID) ?? 0;
                // Đặt lại giá trị IDENTITY sử dụng giá trị maxId
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('LOAIMON', RESEED, @maxId)", new SqlParameter("maxId", maxId));
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
            LOAIMON loaiMon = db.LOAIMONs.Where(x => x.ID == id).First();
            try
            {
                return Json(new
                {
                    TenLoai = loaiMon.TenLoai,
                    TenDuongDan = loaiMon.TenDuongDan,
                    ID_DanhMuc = loaiMon.ID_DanhMuc, 
                    GhiChu = loaiMon.GhiChu
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
