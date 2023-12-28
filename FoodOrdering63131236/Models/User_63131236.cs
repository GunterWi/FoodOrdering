using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace FoodOrdering63131236.Models
{
    public class User_63131236
    {
        public Nullable<int> id { get; set; }
        public bool daDangNhap { get; set; }
        public TAIKHOAN Account { get; set; }
        private FoodOrdering_63131236Entities db;
        public User_63131236()
        {
            id = null;
            daDangNhap = false;
            db = new FoodOrdering_63131236Entities();
            Account = null;
        }

        public bool Login(string tk, string mk)
        {
            List<TAIKHOAN> kt = db.TAIKHOANs.Where(x => x.DuocSD && x.TenTK.Equals(tk) && x.MatKhau.Equals(mk)).ToList();
            if (kt.Count <= 0) return false;
            else
            {
                this.id = kt.First().ID;
                this.Account = kt.First();
                daDangNhap = true;
                return true;
            }
        }
        public void RefreshUserData()
        {
            if (this.id.HasValue)
            {
                // Tải tài khoản và toàn bộ thông tin liên quan
                TAIKHOAN updatedAccount = db.TAIKHOANs
                    .Include("DONHANGs")
                    .Include("BINHLUANs")
                    .FirstOrDefault(x => x.ID == this.id);

                if (updatedAccount != null)
                {
                    this.Account = updatedAccount;

                    // Đoạn này chỉ để gỡ lỗi và kiểm tra
                    Trace.WriteLine("Đơn hàng:");
                    foreach (var donHang in updatedAccount.DONHANGs)
                    {
                        Trace.WriteLine($"ID: {donHang.ID}, Ngày Đặt: {donHang.NgayDatHang}");
                    }

                    Trace.WriteLine("Bình luận:");
                    foreach (var binhLuan in updatedAccount.BINHLUANs)
                    {
                        Trace.WriteLine($"ID: {binhLuan.ID}, Nội dung: {binhLuan.BinhLuan1}");
                    }
                }
            }
        }
        public void Logout()
        {
            daDangNhap = false;
            Account = null;
        }
    }
}