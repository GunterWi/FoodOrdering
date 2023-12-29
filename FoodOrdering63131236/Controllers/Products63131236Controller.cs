using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoodOrdering63131236.Models;
using PagedList;

namespace FoodOrdering63131236.Controllers
{
    public class Products63131236Controller : Controller
    {
        private FoodOrdering_63131236Entities db = new FoodOrdering_63131236Entities();
        public ActionResult Index(int? page)
        {
            int pageNum = page ?? 1;
            List<MONAN> monAns = db.MONANs.Where(x => x.DangMA).ToList();
            return View(monAns.ToPagedList(pageNum, 12));
        }
        // Nội dung món ăn 
        public ActionResult Product(string id)
        {
            var monAns = db.MONANs;
            ViewBag.monAn = (from ma in monAns where ma.TenDuongDan.Equals(id) && ma.DangMA select ma).FirstOrDefault();
            ViewBag.khuyenMai = (from ma in monAns where ma.GiaKM < ma.GiaBan && ma.DangMA select ma).Take(4);
            int loaima = ViewBag.monAn.MaLoai;
            ViewBag.CungLoai = (from ma in monAns where ma.MaLoai == loaima && ma.DangMA select ma).Take(4);
            MONAN monAn = ViewBag.monAn;
            monAn.LuotXem++;
            db.SaveChanges();
            return View();
        }
        // Giở hàng 
        public ActionResult Cart()
        {
            List<GioHang> gioHangs = GioHang.GetGioHang();
            return View(gioHangs);
        }
        // Thanh toán 
        public ActionResult Checkout()
        {
            User_63131236 user = (User_63131236)Session.Contents["Account"];
            if (!user.daDangNhap)
            {
                TempData["AlertMessage"] = "Hãy đăng nhập vào tài khoản của bạn.";
                return Redirect(Url.Action("Login", "Users63131236"));
            }
            List<GioHang> gioHangs = GioHang.GetGioHang();
            if (gioHangs.Count <= 0)
                return RedirectToAction("Cart");
            return View();
        }
        // ajax sử dụng ajax để lấy dữ liệu tỉnh thành, quận huyện, phường xã
        [HttpPost]
        public ActionResult TinhThanh(int id, int type)
        {
            List<string> value = new List<string>();
            List<int> ID = new List<int>();
            if (type == 1)
            {
                foreach (QUANHUYEN phuongXa in db.QUANHUYENs.Where(x => x.TinhThanh == id).OrderBy(x => x.Name))
                {
                    value.Add(phuongXa.Name);
                    ID.Add(phuongXa.ID);
                }
            }
            else if (type == 2)
            {
                foreach (PHUONGXA phuongXa in db.PHUONGXAs.Where(x => x.QuanHuyen == id).OrderBy(x => x.Name))
                {
                    value.Add(phuongXa.Name);
                    ID.Add(phuongXa.ID);
                }
            }
            return Json(new
            {
                Value = value.ToArray(),
                ID = ID.ToArray()
            });
        }
        [HttpPost]
        public ActionResult Checkout(DONHANG donHang)
        {
            User_63131236 user = (User_63131236)Session.Contents["Account"];
            if (!user.daDangNhap)
                return Redirect(Url.Action("Login", "Users63131236"));
            List<GioHang> gioHangs = GioHang.GetGioHang();
            if (gioHangs.Count <= 0)
                return RedirectToAction("Cart");
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    donHang.NgayDatHang = DateTime.Now;
                    donHang.KhachHang = user.id ?? 0;
                    db.DONHANGs.Add(donHang);
                    db.SaveChanges();
                    // đếm số đơn hợp lệ được thêm vào
                    int count = 0;
                    foreach (GioHang gioHang in gioHangs)
                    {
                        // yêu cầu số lượng món ăn phải lớn hơn 0
                        if (gioHang.soLuon <= 0) continue;
                        CHITIETDH chiTietDH = new CHITIETDH()
                        {
                            MaDH = donHang.ID,
                            MaMA = gioHang.monAn.ID,
                            SoLuong = gioHang.soLuon,
                            GiaBan = gioHang.tongCong,
                        };
                        db.MONANs.Where(x => x.ID == gioHang.monAn.ID).First().SoLuong -= gioHang.soLuon;
                        db.MONANs.Where(x => x.ID == gioHang.monAn.ID).First().LuotMua += gioHang.soLuon;
                        db.CHITIETDHs.Add(chiTietDH);
                        count++;
                    }
                    // nếu trong chi tiết đơn hàng không có nào hợp lên thì xóa cả đơn hàng, tránh đơn hàng rỗng
                    if (count <= 0)
                    {
                        transaction.Rollback();
                        return Redirect("/");
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    //reset GioHang
                    Response.Cookies["GioHang"].Value = "";
                    Response.Cookies["GioHang"].Expires = DateTime.Now.AddDays(-1);
                    //update session
                    user.RefreshUserData();
                    Session.Contents["Account"] = user;
                    //TempData["AlertMessage"] = "Bạn đã đặt hàng thành công";
                    return Redirect(Url.Action("ProfileUser", "Users63131236", new { tab = "timeline" }));
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine(e.Message);
                    ViewBag.msg = "Lỗi không thể đặt món";
                    return View();
                }

            }

            return Redirect("/");
        }
        // món ăn theo thể loại 
        public ActionResult Category(string id, int? page)
        {
            int pageNum = page ?? 1;
            List<MONAN> monAns = db.MONANs.Where(x => x.DangMA && x.LOAIMON.TenDuongDan == id).ToList();
            return View("Index", monAns.ToPagedList(pageNum, 12));
        }
        // Tìm kiếm món ăn
        public ActionResult Search(string search, int? page)
        {
            if (page.Equals(null))
            {
                page = 1;
            }
            // loại bỏ dấu tiếng việt
            search = Trash_63131236.convertToUnSign3(search);
            string[] key = search.Split(' ');
            List<MONAN> monAns = new List<MONAN>();
            // lọc từng key ra theo dấu cách
            foreach (String k in key)
            {   
                foreach (MONAN s in db.MONANs)
                {
                    if (Trash_63131236.convertToUnSign3(s.TenMA).IndexOf(k) != -1) // nếu có
                        monAns.Add(s);
                }
            }
            ViewBag.search = search;
            int pageNum = page ?? 1;
            return View("Index", monAns.ToPagedList(pageNum, 12));

        }
        // Quản lý đơn hàng của khách
        public ActionResult OrderManagement(int id)
        {
            DONHANG donHang = db.DONHANGs.Where(x => x.ID == id).First();
            return View(donHang);
        }
        [HttpPost]
        public ActionResult OrderManagement(int sao, int maMA, int maDH)
        {
            User_63131236 user = (User_63131236)Session.Contents["Account"];
            if (!user.daDangNhap) return null;
            db.CHITIETDHs.Where(x => x.MaMA == maMA && x.MaDH == maDH).First().DanhGia = sao;
            db.SaveChanges();
            return Json(new { msg = "Đánh giá thành công" });
        }
        public ActionResult Comment(BINHLUAN binhLuan, int maMA)
        {
            User_63131236 user = (User_63131236)Session.Contents["Account"];
            if (!user.daDangNhap)
            {
                TempData["AlertMessage"] = "Hãy đăng nhập vào tài khoản của bạn.";
                return Redirect(Url.Action("Login", "Users63131236"));
            }
            binhLuan.MaTK = user.Account.ID;
            binhLuan.MaMA = maMA;
            if (binhLuan.BinhLuan1.IndexOf('@') == -1)
                binhLuan.PhanHoi = null;

            if (binhLuan.PhanHoi != null && binhLuan.BinhLuan1.IndexOf('@') != -1)
            {
                string ten = binhLuan.BinhLuan1.Split('‍')[1]; // ký tự ẩn
                string tenPhanHoi = db.BINHLUANs.Where(x => x.ID == binhLuan.PhanHoi).First().TAIKHOAN.HoTen;
                if (!ten.Equals(tenPhanHoi))
                {
                    binhLuan.PhanHoi = null;
                }
                else
                {
                    ten = "<a href = ''><strong>" + ten + "</strong></a>";
                    binhLuan.BinhLuan1 = ten + binhLuan.BinhLuan1.Split('‍')[2]; // ký tự ẩn
                }
            }
            db.BINHLUANs.Add(binhLuan);
            db.SaveChanges();
            string tenDuongDan = db.MONANs.Where(x => x.ID == binhLuan.MaMA).First().TenDuongDan;
            return Redirect(Url.Action("Product", "Products63131236", routeValues: new { id = tenDuongDan }));
        }
        // lọc giá món ăn
        public ActionResult price_filter(string price, int? page)
        {
            string[] price1 = price.Split(',');
            int min = Convert.ToInt32(price1[0]);
            int max = Convert.ToInt32(price1[1]);
            int pageNum = page ?? 1;
            List<MONAN> monAns = db.MONANs.Where(x => x.DangMA && x.GiaBan >= min && x.GiaBan <= max).ToList();
            ViewBag.price = price;
            return View("Index", monAns.ToPagedList(pageNum, 12));
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
