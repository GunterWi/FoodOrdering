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
    
    public partial class BINHLUAN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BINHLUAN()
        {
            this.BINHLUAN11 = new HashSet<BINHLUAN>();
        }
    
        public int ID { get; set; }
        public int MaMA { get; set; }
        public int MaTK { get; set; }
        public string BinhLuan1 { get; set; }
        public Nullable<int> PhanHoi { get; set; }
    
        public virtual MONAN MONAN { get; set; }
        public virtual TAIKHOAN TAIKHOAN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BINHLUAN> BINHLUAN11 { get; set; }
        public virtual BINHLUAN BINHLUAN2 { get; set; }
    }
}