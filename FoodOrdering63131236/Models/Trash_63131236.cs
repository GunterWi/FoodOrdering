using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;

namespace FoodOrdering63131236.Models
{
    public class Trash_63131236
    {
        private FoodOrdering_63131236Entities db;
        public Trash_63131236()
        {
            this.db = new FoodOrdering_63131236Entities();
        }
        public IEnumerable<DANHMUC> GetDanhMuc()
        {
            var DANHMUC = db.DANHMUCs.Include("LOAIMONs").ToList();
            return DANHMUC;
        }
        // hàm xóa dấu tiếng việt
        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
        }
        // hàm mã hóa MD5
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}