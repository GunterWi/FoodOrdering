//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FoodOrdering63131236.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BAIVIET
    {
        public int ID { get; set; }
        public string TenBV { get; set; }
        public string TenDuongDan { get; set; }
        public System.DateTime NgayDang { get; set; }
        public System.DateTime NgayCapNhat { get; set; }
        public string TomTat { get; set; }
        public Nullable<int> AnhBia { get; set; }
        public string NoiDungBV { get; set; }
        public bool HienThi { get; set; }
        public Nullable<int> SoLanDoc { get; set; }
    
        public virtual ANH ANH { get; set; }
    }
}