//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    using System.Web;

    public partial class PhieuNghiPhep
    {
        public string SoPhieuNP { get; set; }
        public string MaNV { get; set; }
        public string MaLoaiNP { get; set; }
        public Nullable<System.DateTime> NgayLapPNP { get; set; }
        public Nullable<System.DateTime> NgayBDNghi { get; set; }
        public Nullable<System.DateTime> NgayDiLam { get; set; }
        public string AnhNghiPhepNV { get; set; }
        public string NoiDungNP { get; set; }
        public string TrangThai { get; set; }

        public HttpPostedFileBase ImageUploadPNP { get; set; }
    
        public virtual LoaiNghiPhep LoaiNghiPhep { get; set; }
        public virtual NhanVien NhanVien { get; set; }
    }
}
