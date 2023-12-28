﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class FoodOrdering_63131236Entities : DbContext
    {
        public FoodOrdering_63131236Entities()
            : base("name=FoodOrdering_63131236Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ANH> ANHs { get; set; }
        public virtual DbSet<BAIVIET> BAIVIETs { get; set; }
        public virtual DbSet<BINHLUAN> BINHLUANs { get; set; }
        public virtual DbSet<CHITIETDH> CHITIETDHs { get; set; }
        public virtual DbSet<DANHMUC> DANHMUCs { get; set; }
        public virtual DbSet<DONHANG> DONHANGs { get; set; }
        public virtual DbSet<LOAIMON> LOAIMONs { get; set; }
        public virtual DbSet<MONAN> MONANs { get; set; }
        public virtual DbSet<PHUONGXA> PHUONGXAs { get; set; }
        public virtual DbSet<QUANHUYEN> QUANHUYENs { get; set; }
        public virtual DbSet<TAIKHOAN> TAIKHOANs { get; set; }
        public virtual DbSet<TINHTHANH> TINHTHANHs { get; set; }
    
        public virtual ObjectResult<GetTotalSumIn7days_Result> GetTotalSumIn7days()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetTotalSumIn7days_Result>("GetTotalSumIn7days");
        }
    
        public virtual int remov_file_anh(string file)
        {
            var fileParameter = file != null ?
                new ObjectParameter("file", file) :
                new ObjectParameter("file", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("remov_file_anh", fileParameter);
        }
    
        public virtual int rename_path_anh(string folderOldName, string folderNewName)
        {
            var folderOldNameParameter = folderOldName != null ?
                new ObjectParameter("folderOldName", folderOldName) :
                new ObjectParameter("folderOldName", typeof(string));
    
            var folderNewNameParameter = folderNewName != null ?
                new ObjectParameter("folderNewName", folderNewName) :
                new ObjectParameter("folderNewName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("rename_path_anh", folderOldNameParameter, folderNewNameParameter);
        }
    }
}