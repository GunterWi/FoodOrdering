﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoodOrdering63131236.Models;

namespace FoodOrdering63131236.Controllers
{
    public class Users63131236Controller : Controller
    {
        private FoodOrdering_63131236Entities db = new FoodOrdering_63131236Entities();

        // GET: TaiKhoans63131236
        public ActionResult Login()
        {
            User_63131236 user = (User_63131236)Session.Contents["Account"];
            if (user.daDangNhap)
            {
                Response.Redirect(Url.Action("Index", "Homes63131236"));
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(string TaiKhoan, string MatKhau)
        {
            User_63131236 user = (User_63131236)Session.Contents["Account"];
            if (user == null)
            {
                user = new User_63131236();
                Session.Contents["Account"] = user;
            }
            if (user.daDangNhap)
            {
                return RedirectToAction("Index", "Homes63131236");
            }
            if (user.Login(TaiKhoan, Trash_63131236.MD5Hash(MatKhau)))
            {
                return RedirectToAction("Index", "Homes63131236");
            }
            TAIKHOAN taiKhoan = db.TAIKHOANs.FirstOrDefault(x => x.TenTK == TaiKhoan);
            if (taiKhoan != null && !taiKhoan.DuocSD)
            {
                ViewBag.msg = "Tài khoản này đã bị khóa";
                return View();
            }
            ViewBag.msg = "Tên tài khoản hoặc mật khẩu không chính xác";
            return View();
        }
        public void Logout()
        {
            User_63131236 user = (User_63131236)Session.Contents["Account"];
            user.Logout();
            Response.Redirect("/");
        }
        [HttpPost]
        public ActionResult Register(TAIKHOAN kh, string xnMatKhau)
        {
            if (!xnMatKhau.Equals(kh.MatKhau))
            {
                ViewData["LoiMK"] = "Xác thực mật khẩu không chính xác";
                return View("Login");
            }
            List<TAIKHOAN> kt = db.TAIKHOANs.Where(x => x.TenTK == kh.TenTK).ToList();
            bool luu = true;
            if (kt.Count > 0)
            {
                ModelState["TenTK"].Errors.Add("Tên tài khoản đã tồn tại");
                luu = false;
            }

            kt = db.TAIKHOANs.Where(x => x.Email == kh.Email).ToList();
            if (kt.Count > 0)
            {
                ModelState["Email"].Errors.Add("Email đã tồn tại");
                luu = false;
            }
            kt = db.TAIKHOANs.Where(x => x.SDT == kh.SDT).ToList();
            if (kt.Count > 0)
            {
                ModelState["SDT"].Errors.Add("Số điện thoại đã tồn tại");
                luu = false;
            }
            if (luu)
            {
                try
                {
                    kh.MatKhau = Trash_63131236.MD5Hash(kh.MatKhau);
                    kh.DuocSD = true;
                    kh.NgayCap = DateTime.Now;
                    kh.VaiTro = "user";
                    kh.Avatar = 59; // hiện tại cho là default, lười làm hàm tìm kiếm file ảnh default rồi gán ID, cho mặc định luôn =))
                    db.TAIKHOANs.Add(kh);
                    db.SaveChanges();
                    User_63131236 user = (User_63131236)Session.Contents["Account"];
                    user.Login(kh.TenTK, kh.MatKhau);
                    Response.Redirect(Url.Action("Index", "Homes63131236"));
                }
                catch (Exception e)
                {
                    ViewData["LoiMK"] = e.Message;
                }

            }
            return View("Login");
        }
        public ActionResult ProfileUser()
        {
            User_63131236 user = (User_63131236)Session.Contents["Account"];
            return View(user.Account);
        }
        [HttpPost]
        public ActionResult ProfileUser(int id, HttpPostedFileBase Avatar, string Hoten, string Email, string SDT)
        {
            TAIKHOAN taiKhoan1 = db.TAIKHOANs.FirstOrDefault(x => x.ID == id);
            User_63131236 user = (User_63131236)Session.Contents["Account"];
            if (taiKhoan1 == null)
            {
                Trace.WriteLine("thất bại.");
                return HttpNotFound();
            }

            string oldImagePath = null;

            if (Avatar != null)
            {
                // Nếu có ảnh cũ, lưu đường dẫn để xóa sau khi cập nhật cơ sở dữ liệu thành công
                if (!string.IsNullOrEmpty(taiKhoan1.ANH.Url) && !taiKhoan1.ANH.Url.Equals("/Asset/Image/profile/default.png"))
                {
                    oldImagePath = Server.MapPath(taiKhoan1.ANH.Url);
                }
                // Lưu tệp ảnh mới
                var imagePath = Server.MapPath("/Asset/Image/profile/");
                var fileName = Path.GetFileName(Avatar.FileName);
                var fullPath = Path.Combine(imagePath, fileName);
                Avatar.SaveAs(fullPath);

                // Tạo đối tượng ảnh mới và thêm vào cơ sở dữ liệu
                ANH newAvatar = new ANH() { Url = "/Asset/Image/profile/" + fileName };
                db.ANHs.Add(newAvatar);
                db.SaveChanges();

                taiKhoan1.Avatar = newAvatar.ID;
            }

            // Cập nhật thông tin khác của tài khoản
            taiKhoan1.HoTen = Hoten;
            taiKhoan1.Email = Email;
            taiKhoan1.SDT = SDT;

            try
            {
                // Cập nhật thông tin tài khoản trong cơ sở dữ liệu
                user.Account = taiKhoan1;
                db.SaveChanges();

                // Nếu có ảnh cũ và cập nhật cơ sở dữ liệu thành công, xóa ảnh cũ
                if (oldImagePath != null && System.IO.File.Exists(oldImagePath))
                {
                    Trace.WriteLine(oldImagePath);
                    db.remov_file_anh(oldImagePath);
                    db.SaveChanges();
                    System.IO.File.Delete(oldImagePath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return View(taiKhoan1);
            //return RedirectToAction("Index", "Homes63131236");
        }


        [HttpPost]
        public ActionResult ChangeMK(int id, string MatKhau, string xnMK)
        {
            TAIKHOAN taiKhoan1 = db.TAIKHOANs.Where(x => x.ID == id).First();
            if (!MatKhau.Equals(xnMK))
            {
                ViewBag.xnMK = "Xác nhận mật khẩu không chính xác";
                return View(taiKhoan1);
            }
            taiKhoan1.MatKhau = Trash_63131236.MD5Hash(MatKhau);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return View("ProfileUser", taiKhoan1);
        }
    }
}
