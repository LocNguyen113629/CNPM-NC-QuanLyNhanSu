﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HR_Manager.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class HRManagerEntities4 : DbContext
    {
        public HRManagerEntities4()
            : base("name=HRManagerEntities4")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ChamCong> ChamCong { get; set; }
        public virtual DbSet<ChucVu> ChucVu { get; set; }
        public virtual DbSet<HopDongLaoDong> HopDongLaoDong { get; set; }
        public virtual DbSet<LoaiHopDong> LoaiHopDong { get; set; }
        public virtual DbSet<LoaiKLKT> LoaiKLKT { get; set; }
        public virtual DbSet<LoaiNghiPhep> LoaiNghiPhep { get; set; }
        public virtual DbSet<NhanVien> NhanVien { get; set; }
        public virtual DbSet<PhieuCongTac> PhieuCongTac { get; set; }
        public virtual DbSet<PhieuDieuChuyen> PhieuDieuChuyen { get; set; }
        public virtual DbSet<PhieuKLKT> PhieuKLKT { get; set; }
        public virtual DbSet<PhieuLuong> PhieuLuong { get; set; }
        public virtual DbSet<PhieuNghiPhep> PhieuNghiPhep { get; set; }
        public virtual DbSet<PhongBan> PhongBan { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }
    
        public virtual int prUpdateChuyenPB(string maPBden, string maNV)
        {
            var maPBdenParameter = maPBden != null ?
                new ObjectParameter("MaPBden", maPBden) :
                new ObjectParameter("MaPBden", typeof(string));
    
            var maNVParameter = maNV != null ?
                new ObjectParameter("MaNV", maNV) :
                new ObjectParameter("MaNV", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prUpdateChuyenPB", maPBdenParameter, maNVParameter);
        }
    }
}
