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
    
    public partial class ChamCong
    {
        public string MaCC { get; set; }
        public string MaNV { get; set; }
        public string MaPL { get; set; }
        public Nullable<System.DateTime> ThoiGianVaoLam { get; set; }
        public Nullable<System.DateTime> ThoiGianKetThucLV { get; set; }
        public Nullable<System.TimeSpan> SoGioLamViec { get; set; }
    
        public virtual NhanVien NhanVien { get; set; }
    }
}
