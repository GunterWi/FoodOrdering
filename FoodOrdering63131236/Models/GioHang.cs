using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodOrdering63131236.Models
{
    public class GioHang
    {
        public MONAN monAn { get; set; }
        public int soLuon { get; set; }
        public GioHang(MONAN monAn, int soLuon)
        {
            this.monAn = monAn;
            this.soLuon = soLuon;
        }
        public int tongCong
        {
            get
            {
                return ((int)(soLuon * monAn.GiaKM ?? monAn.GiaBan));
            }
        }
        public static List<GioHang> GetGioHang()
        {
            List<GioHang> gioHangs = new List<GioHang>();
            if (!HttpContext.Current.Request.Cookies.AllKeys.Contains("GioHang")) return gioHangs;
            string cookie = HttpContext.Current.Request.Cookies["GioHang"].Value;
            if (String.IsNullOrEmpty(cookie) || cookie.Equals("")) return gioHangs;
            string[] id_sl = cookie.Split(',');
            foreach (string i in id_sl)
            {
                string[] gh = i.Split('|');
                FoodOrdering_63131236Entities db = new FoodOrdering_63131236Entities();
                try
                {
                    MONAN monAn = db.MONANs.ToList().Where(x => x.ID == Convert.ToInt32(gh[0])).First();
                    GioHang gioHang = new GioHang(monAn, Convert.ToInt32(gh[1]));
                    gioHangs.Add(gioHang);
                }
                catch
                {
                    continue;
                }

            }
            return gioHangs;
        }

        public static int TongHang()
        {
            List<GioHang> gioHangs = GioHang.GetGioHang();
            int tc = 0;
            foreach (GioHang gh in gioHangs)
            {
                tc += gh.soLuon * (gh.monAn.GiaKM ?? gh.monAn.GiaBan);
            }
            return tc;
        }
    }
}