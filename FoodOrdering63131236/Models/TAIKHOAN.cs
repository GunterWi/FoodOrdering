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
    
    public partial class TAIKHOAN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TAIKHOAN()
        {
            this.BINHLUANs = new HashSet<BINHLUAN>();
            this.DONHANGs = new HashSet<DONHANG>();
        }
    
        public int ID { get; set; }
        public string HoTen { get; set; }
        public string TenTK { get; set; }
        public string MatKhau { get; set; }
        public System.DateTime NgayCap { get; set; }
        public bool GioiTinh { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public bool DuocSD { get; set; }
        public string GhiChu { get; set; }
        public Nullable<int> Avatar { get; set; }
        public string VaiTro { get; set; }
    
        public virtual ANH ANH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BINHLUAN> BINHLUANs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONHANG> DONHANGs { get; set; }
    }
}
