using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
    public class TaiKhoans63131236Controller : Controller
    {
        private FoodOrdering_63131236Entities db = new FoodOrdering_63131236Entities();

        // Quản lý user 
        public ActionResult UserManagement(int? page)
        {
            int pageNum = (page ?? 1);
            List<TAIKHOAN> taiKhoans = db.TAIKHOANs.ToList();
            return View(taiKhoans.ToPagedList(pageNum, 10));
        }
        public ActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(TAIKHOAN taiKhoan1,string ConfirmPassword, HttpPostedFileBase avatarFile)
        {
            if (ModelState.IsValid)
            {
                if (avatarFile != null && avatarFile.ContentLength > 0)
                {
                    var imagePath = Server.MapPath("/Asset/Image/profile/");
                    var fileName = Path.GetFileName(avatarFile.FileName);
                    var fullPath = Path.Combine(imagePath + fileName);
                    avatarFile.SaveAs(fullPath);

                    ANH newAvatar = new ANH() { Url = "/Asset/Image/profile/" + fileName };
                    taiKhoan1.Avatar = newAvatar.ID;
                    db.ANHs.Add(newAvatar);
                }

                if (!ConfirmPassword.Equals(taiKhoan1.MatKhau))
                {
                    ViewData["LoiMK"] = "Xác thực mật khẩu không chính xác";
                    return View("Login");
                }
                List<TAIKHOAN> kt = db.TAIKHOANs.Where(x => x.TenTK == taiKhoan1.TenTK).ToList();
                bool luu = true;
                if (kt.Count > 0)
                {
                    ModelState["TenTK"].Errors.Add("Tên tài khoản đã tồn tại");
                    luu = false;
                }

                kt = db.TAIKHOANs.Where(x => x.Email == taiKhoan1.Email).ToList();
                if (kt.Count > 0)
                {
                    ModelState["Email"].Errors.Add("Email đã tồn tại");
                    luu = false;
                }
                kt = db.TAIKHOANs.Where(x => x.SDT == taiKhoan1.SDT).ToList();
                if (kt.Count > 0)
                {
                    ModelState["SDT"].Errors.Add("Số điện thoại đã tồn tại");
                    luu = false;
                }
               /* kt = db.TAIKHOANs.Where(x => x.GioiTinh == taiKhoan1.GioiTinh).ToList();
                if (kt.Count > 0)
                {
                    ModelState["GioiTinh"].Errors.Add("Giới tính đã tồn tại =)))) ");
                    luu = false;
                }*/
                if (luu)
                {
                    try
                    {
                        taiKhoan1.MatKhau = Trash_63131236.MD5Hash(taiKhoan1.MatKhau);
                        taiKhoan1.DuocSD = true;
                        taiKhoan1.NgayCap = DateTime.Now;
                        db.TAIKHOANs.Add(taiKhoan1);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        ViewData["LoiMK"] = e.Message;
                    }

                }
                return RedirectToAction("UserManagement");
            }
            return RedirectToAction("UserManagement");
        }
        // cài đặt trạng thái người dùng
        public ActionResult SettingUser(int id, int type)
        {
            // type: 1 khóa tài khoản, 2 thăng làm admin, 3 xóa tài khoản
            TAIKHOAN taiKhoan = db.TAIKHOANs.Where(x => x.ID == id).First();
            if (taiKhoan.TenTK == "admin")
                return RedirectToAction("UserManagement");
            if (type == 1)
            {
                taiKhoan.DuocSD = taiKhoan.DuocSD ? false : true;
            }
            else if (type == 2)
            {
                taiKhoan.VaiTro = taiKhoan.VaiTro == "admin" ? "user" : "admin";
            }
            else if (type == 3)
            {
                db.TAIKHOANs.Remove(taiKhoan);
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction("UserManagement");
        }
        // thông tin tài khoản
        public ActionResult UserProfile(int id)
        {
            TAIKHOAN taiKhoan = db.TAIKHOANs.Where(x => x.ID == id).First();
            return View(taiKhoan);
        }
        // sửa thông tin
        [HttpPost]
        public ActionResult UserProfile(TAIKHOAN taiKhoan, bool CapLaiMK)
        {
            TAIKHOAN taiKhoan1 = db.TAIKHOANs.Where(x => x.ID == taiKhoan.ID).First();
            // tài khoản admin sẽ không được phép ai thay đổi trừ chính nó
            if (taiKhoan1.TenTK == "admin")
                return View(taiKhoan1);
            bool luu = true;
            List<TAIKHOAN> kt = db.TAIKHOANs.Where(x => x.Email == taiKhoan.Email && x.ID != taiKhoan.ID).ToList();
            if (kt.Count > 0)
            {
                ModelState["Email"].Errors.Add("Email đã tồn tại");
                luu = false;
            }
            kt = db.TAIKHOANs.Where(x => x.SDT == taiKhoan.SDT && x.ID != taiKhoan.ID).ToList();
            if (kt.Count > 0)
            {
                ModelState["SDT"].Errors.Add("Số điện thoại đã tồn tại");
                luu = false;
            }

            if (!luu)
                return View(taiKhoan1);
            taiKhoan1.HoTen = taiKhoan.HoTen;
            taiKhoan1.Email = taiKhoan.Email;
            taiKhoan1.SDT = taiKhoan.SDT;
            taiKhoan1.GhiChu = taiKhoan.GhiChu;
            try
            {
                if (CapLaiMK)
                    taiKhoan1.MatKhau = Trash_63131236.MD5Hash("1111");
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return View(taiKhoan1);
        }
        // thông tin admin (tài khoản admin cá nhân)
        public ActionResult ProfileAdmin()
        {
            User_63131236 user = (User_63131236)Session.Contents["Account"];
            return View(user.Account);
        }
        [HttpPost]
        public ActionResult ProfileAdmin(TAIKHOAN taiKhoan, string xnMK)
        {
            TAIKHOAN taiKhoan1 = db.TAIKHOANs.Where(x => x.ID == taiKhoan.ID).First();
            if (!taiKhoan.MatKhau.Equals(xnMK))
            {
                ViewBag.xnMK = "Xác nhận mật khẩu không chính xác";
                return View(taiKhoan1);
            }
            bool luu = true;
            List<TAIKHOAN> kt = db.TAIKHOANs.Where(x => x.Email == taiKhoan.Email && x.ID != taiKhoan.ID).ToList();
            if (kt.Count > 0)
            {
                ModelState["Email"].Errors.Add("Email đã tồn tại");
                luu = false;
            }
            kt = db.TAIKHOANs.Where(x => x.SDT == taiKhoan.SDT && x.ID != taiKhoan.ID).ToList();
            if (kt.Count > 0)
            {
                ModelState["SDT"].Errors.Add("Số điện thoại đã tồn tại");
                luu = false;
            }
            if (!luu)
                return View(taiKhoan1);
            taiKhoan1.HoTen = taiKhoan.HoTen;
            taiKhoan1.Email = taiKhoan.Email;
            taiKhoan1.SDT = taiKhoan.SDT;
            taiKhoan1.GhiChu = taiKhoan.GhiChu;
            taiKhoan1.MatKhau = Trash_63131236.MD5Hash(taiKhoan.MatKhau);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return View(taiKhoan1);
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
