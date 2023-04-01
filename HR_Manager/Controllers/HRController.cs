using HR_Manager.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace HR_Manager.Controllers
{
    public class HRController : Controller
    {
        //Khai báo database
        HRManagerEntities4 db = new HRManagerEntities4();
        // GET: HR
        public ActionResult HR_HomePage()
        {
            return View();
        }

        #region Quản Lý Nhân Viên
        // Xem danh sách nhân viên
        public ActionResult ListEmployee()
        {
            List<NhanVien> dsNhanVien = db.NhanVien.ToList();

            return View(dsNhanVien);
        }

        // Thêm nhân viên
        public ActionResult AddNewEmployee()
        {
            var listCV = db.ChucVu.ToList();
            ViewBag.MaCV = new SelectList(listCV, "MaCV", "TenCV");

            var listPB = db.PhongBan.ToList();
            ViewBag.MaPB = new SelectList(listPB, "MaPB", "TenPB");
            return View();
        }

        [HttpPost] //Check tên file nếu trùng thì ko cho upload, khi upload đổi tên file thành mã nhân viên (SẼ LÀM)
        public ActionResult AddNewEmployee(NhanVien model)
        {
            if (model.ImageUpload != null)
            {
                string fileName = Path.GetFileName(model.ImageUpload.FileName);
                model.AnhDaiDienNV = "/Images/" + fileName;
                model.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/"), fileName));
            }

            //Dropdown List
            var listCV = db.ChucVu.ToList();
            ViewBag.MaCV = new SelectList(listCV, "MaCV", "TenCV");

            var listPB = db.PhongBan.ToList();
            ViewBag.MaPB = new SelectList(listPB, "MaPB", "TenPB");

            //Add new record
            Random rd = new Random();
            var manv = "NV" + rd.Next(111, 999);
            model.MaNV = manv;

            db.NhanVien.Add(model);

            //Save changes in record
            db.SaveChanges();
            return RedirectToAction("ListEmployee");
        }

        //Cập nhật thông tin nhân viên
        public ActionResult UpdateEmployee(string id)
        {
            NhanVien nv_model = db.NhanVien.Find(id);

            ViewBag.MaCV = new SelectList(db.ChucVu, "MaCV", "TenCV", nv_model.MaCV);
            ViewBag.MaPB = new SelectList(db.PhongBan, "MaPB", "TenPB", nv_model.MaPB);

            return View(nv_model);
            
        }

        [HttpPost]
        public ActionResult UpdateEmployee(NhanVien nv_model)
        {
            var updateModel = db.NhanVien.Find(nv_model.MaNV);

            updateModel.MaCV = nv_model.MaCV;
            updateModel.MaPB = nv_model.MaPB;
            updateModel.CCCD = nv_model.CCCD;
            updateModel.HoTen = nv_model.HoTen;
            updateModel.NgaySinh = nv_model.NgaySinh;
            updateModel.DanToc = nv_model.DanToc;
            updateModel.TonGiao = nv_model.TonGiao;
            updateModel.QueQuan = nv_model.QueQuan;
            updateModel.NoiOHienTai = nv_model.NoiOHienTai;
            updateModel.DienThoai = nv_model.DienThoai;
            updateModel.Email = nv_model.Email;
            updateModel.TrinhDoHocVan = nv_model.TrinhDoHocVan;
            updateModel.SoNgayNPConLai = nv_model.SoNgayNPConLai;

            if (nv_model.ImageUpload != null)
            {
                string fileName = Path.GetFileName(nv_model.ImageUpload.FileName);
                nv_model.AnhDaiDienNV = "/Images/" + fileName;
                nv_model.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/"), fileName));
                updateModel.AnhDaiDienNV = nv_model.AnhDaiDienNV;

            }

            ViewBag.MaCV = new SelectList(db.ChucVu, "MaCV", "TenCV", nv_model.MaCV);
            ViewBag.MaPB = new SelectList(db.PhongBan, "MaPB", "TenPB", nv_model.MaPB);

            db.SaveChanges();
            return RedirectToAction("ListEmployee");

        }

        //Thông tin chi tiết nhân viên
        public ActionResult DetailsEmployee(string id)
        {
            //Lấy nhân viên với mã nhân viên tương ứng
            var nv = db.NhanVien.FirstOrDefault(s => s.MaNV == id);
            return View(nv);
        }

        //Xoá nhân viên 
        public ActionResult DeleteEmployee(string id)
        {
            NhanVien model = db.NhanVien.Find(id);
            db.NhanVien.Remove(model);

        //Save Change
            db.SaveChanges();
            return RedirectToAction("ListEmployee");
        }
        #endregion

        #region Quản Lý Phòng Ban

        //Xem danh sách nhân viên với phòng ban tương ứng (filter data)
        public ActionResult ListDepartment_Employee(string maPB)
        {
            //Dropdown list PhongBan
            var listPB = db.PhongBan.ToList();
            ViewBag.MaPB = new SelectList(listPB, "MaPB", "TenPB");

            //List ChucVu
            var listCV = db.ChucVu.ToList();
            ViewBag.MaCV = new SelectList(listCV, "MaCV", "TenCV");

            //Lấy danh sách nhân viên trong phòng ban theo MaPB
            List<NhanVien> dsNVPB = new List<NhanVien>();
            if (maPB == null)
            {
                dsNVPB = db.NhanVien.ToList();
            }
            else
            {
                dsNVPB = db.NhanVien.Where(pb => pb.MaPB == maPB).ToList();
            }
            return View(dsNVPB);            
        }

        //Thêm phiếu điều chuyển 
        public ActionResult AddTransferingDepartment(string id)
        {
            //Lấy MaNV
            ViewBag.MaNVDC = id;

            //Lấy Hình NV
            ViewBag.hinhNV = db.NhanVien.FirstOrDefault(nv => nv.MaNV == id).AnhDaiDienNV;

            //Lấy Phòng Ban Cũ
            ViewBag.phongbancu = db.NhanVien.FirstOrDefault(nv => nv.MaNV == id).PhongBan.TenPB;

            //Dropdown list PhongBan Mới
            var listPB = db.PhongBan.ToList();
            ViewBag.MaPB = new SelectList(listPB, "MaPB", "TenPB");


            return View();
        }

        [HttpPost]
        public ActionResult AddTransferingDepartment(PhieuDieuChuyen model, string id)
        {
            //Add new record
            Random rd = new Random();
            var sophieudc = "DC" + rd.Next(111, 999);
            model.SoPhieuDC = sophieudc;
            model.MaNV = id;

            //model.NhanVien.PhongBan.TenPB = db.NhanViens.FirstOrDefault(nv => nv.MaNV == id).PhongBan.TenPB;
            ViewBag.phongbancu = db.NhanVien.FirstOrDefault(nv => nv.MaNV == id).PhongBan.TenPB;
            model.PBDi = ViewBag.phongbancu;

            //Dropdown list lưu PhongBan Đến
            ViewBag.MaPB = new SelectList(db.PhongBan, "MaPB", "TenPB", model.MaPB);

            //Stored Procedure Update PhongBan bảng Nhân Viên
            var updatePB = db.prUpdateChuyenPB(model.MaPB, id).ToString();

            db.PhieuDieuChuyen.Add(model);

            //Save changes in record
            db.SaveChanges();
            return RedirectToAction("ListDepartment_Employee");
        }

        #endregion

        #region Quản Lý Hợp Đồng (Nếu loại hợp đồng là "Không thời hạn" => Ẩn ngày kết thúc [SẼ LÀM])
        //Thêm hợp đồng cho nhân viên
        public ActionResult AddEmployeeAgreement(string id)
        {
            var listHD = db.LoaiHopDong.ToList();
            ViewBag.MaLoaiHD = new SelectList(listHD, "MaLoaiHD", "TenLoaiHD");

            ViewBag.MaNV = id;
            ViewBag.hinhNV = db.NhanVien.FirstOrDefault(nv => nv.MaNV == id).AnhDaiDienNV;

            return View();
        }

        [HttpPost]
        public ActionResult AddEmployeeAgreement(HopDongLaoDong model, string id)
        {
            //Add new record
            Random rd = new Random();
            var mahd = "HD" + rd.Next(111, 999);
            model.MaHD = mahd;
            model.MaNV = id;

            db.HopDongLaoDong.Add(model);

            //Save changes in record
            db.SaveChanges();
            return RedirectToAction("AgreementList");
        }

        //Danh sách hợp đồng
        public ActionResult AgreementList(string MaLoaiHD)
        {
            //Dropdown list LoaiHopDong
            var listLHD = db.LoaiHopDong.ToList();
            ViewBag.MaLoaiHD = new SelectList(listLHD, "MaLoaiHD", "TenLoaiHD");

            //Filter data LoaiHopDong
            List<HopDongLaoDong> dsHopDong = new List<HopDongLaoDong>();
            if (MaLoaiHD == null)
            {
                 dsHopDong = db.HopDongLaoDong.ToList();
            }
            else
            {
                 dsHopDong = db.HopDongLaoDong.Where(st => st.MaLoaiHD == MaLoaiHD).ToList();
            }

            return View(dsHopDong);
        }

        //Cập nhật loại hợp đồng cho nhân viên
        public ActionResult UpdateEmployeeAgreement(string id, string idMaNV)
        {
            HopDongLaoDong hd_model = db.HopDongLaoDong.Find(id);
            //Lấy Hình NV
            ViewBag.hinhNV = db.NhanVien.FirstOrDefault(nv => nv.MaNV == idMaNV).AnhDaiDienNV;

            ViewBag.MaLoaiHD = new SelectList(db.LoaiHopDong, "MaLoaiHD", "TenLoaiHD", hd_model.MaLoaiHD);


            return View(hd_model);

        }

        [HttpPost]
        public ActionResult UpdateEmployeeAgreement(HopDongLaoDong hd_model)
        {
            var updateModel = db.HopDongLaoDong.Find(hd_model.MaHD);

            ViewBag.MaLoaiHD = new SelectList(db.LoaiHopDong, "MaLoaiHD", "TenLoaiHD", hd_model.MaLoaiHD);

            updateModel.NgayBDHD = hd_model.NgayBDHD;
            updateModel.NgayKTHD = hd_model.NgayKTHD;

            db.SaveChanges();
            return RedirectToAction("AgreementList");

        }


        #endregion

        #region Quản Lý Công Tác
        //Thêm phiếu công tác
        public ActionResult AddEmployeeBusiness(string id)
        {
            ////Lấy hình nhân viên
            ViewBag.hinhNV = db.NhanVien.FirstOrDefault(nv => nv.MaNV == id).AnhDaiDienNV;

            //TextBox MaNV
            ViewBag.MaNVCu = id;

            return View();
        }

        [HttpPost]
        public ActionResult AddEmployeeBusiness(PhieuCongTac model, string id)
        {
            model.MaNV = id;

            //Add new record
            Random rd = new Random();
            var sophieuct = "CT" + rd.Next(111, 999);
            model.SoPhieuCT = sophieuct;

            //Set tình trạng công tác là "Đang công tác" khi mới thêm phiếu ct
            var tinhtrangct = "Đang công tác";
            model.TinhTrangCongTac = tinhtrangct;

            db.PhieuCongTac.Add(model);

            //Save changes in record
            db.SaveChanges();
            return RedirectToAction("ListEmployeeBusiness");
        }

        //Danh sách nhân viên công tác
        public ActionResult ListEmployeeBusiness()
        {
            List<PhieuCongTac> dsPhieuCongTac = db.PhieuCongTac.ToList();

            return View(dsPhieuCongTac);
        }

        //Cập nhật quá trình công tác
        public ActionResult UpdateEmployeeBusiness(string id, string idMaNV)
        {
            PhieuCongTac model = db.PhieuCongTac.Find(id);

            //Lấy ảnh đại diện nhân viên
            ViewBag.hinhNV = db.NhanVien.FirstOrDefault(nv => nv.MaNV == idMaNV).AnhDaiDienNV;

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateEmployeeBusiness(PhieuCongTac model)
        {
            // Find objects
            var updateModel = db.PhieuCongTac.Find(model.SoPhieuCT);

            // Assign value for objects
            updateModel.NoiCT = model.NoiCT;
            updateModel.NoiDungCongTac = model.NoiDungCongTac;
            updateModel.TuNgay = model.TuNgay;
            updateModel.DenNgay = model.DenNgay;
            updateModel.TinhTrangCongTac = model.TinhTrangCongTac;

            //Save Changes
            db.SaveChanges();

            return RedirectToAction("ListEmployeeBusiness");
        }

        
        //Xoá phiếu công tác
        public ActionResult DeleteEmployeeBusiness(string id)
        {
            // Find objects by ID
            PhieuCongTac model = db.PhieuCongTac.Find(id);
            db.PhieuCongTac.Remove(model);

            // Save Change
            db.SaveChanges();

            return RedirectToAction("ListEmployeeBusiness");
        }
        #endregion

        #region Quản Lý Nghỉ Phép
        public ActionResult ListApplicationForLeave()
        {
            List<PhieuNghiPhep> dsPNP = db.PhieuNghiPhep.ToList();

            return View(dsPNP);
        }

        public ActionResult DetailsApplication(string id)
        {
            var sopnp = db.PhieuNghiPhep.FirstOrDefault(s => s.SoPhieuNP == id);
            return View(sopnp);
        }

        #endregion

        #region Quản Lý KLKT

        //Thêm phiếu KLKT
        public ActionResult AddRewardDiscipline()
        {
            //Dropdown List MaNV
            var listMaNV = db.NhanVien.ToList();
            ViewBag.MaNV = new SelectList(listMaNV, "MaNV", "HoTen");

            //Dropdown List MaPL
            var listMaPL = db.PhieuLuong.ToList();
            ViewBag.MaPL = new SelectList(listMaPL, "MaPL", "MaPL");

            //Dropdown List LoaiKLKT
            var listKLKT = db.LoaiKLKT.ToList();
            ViewBag.MaKLKT = new SelectList(listKLKT, "MaKLKT", "TenKLKT");

            return View();
        }

        [HttpPost]
        public ActionResult AddRewardDiscipline(PhieuKLKT model)
        {
            //Dropdown List MaNV
            var listMaNV = db.NhanVien.ToList();
            ViewBag.MaNV = new SelectList(listMaNV, "MaNV", "HoTen");

            //Dropdown List MaPL
            var listMaPL = db.PhieuLuong.ToList();
            ViewBag.MaPL = new SelectList(listMaPL, "MaPL", "MaPL");

            //Dropdown List LoaiKLKT
            var listKLKT = db.LoaiKLKT.ToList();
            ViewBag.MaKLKT = new SelectList(listKLKT, "MaKLKT", "TenKLKT");

            //Add new record
            Random rd = new Random();
            var sophieuKLKT = "RD" + rd.Next(111, 999);
            model.SoPhieuKLKT = sophieuKLKT;

            db.PhieuKLKT.Add(model);

            //Save changes in record
            db.SaveChanges();
            return RedirectToAction("ListRewardDiscipline");
        }

        //Danh sách phiếu KLKT
        public ActionResult ListRewardDiscipline()
        {
            List<PhieuKLKT> dsPhieuKLKT = db.PhieuKLKT.ToList();

            var kt = db.LoaiKLKT.Where(lkt => lkt.MaKLKT.StartsWith("KT")).ToList();
            ViewBag.LKT = kt.ToString();

            var kl = db.LoaiKLKT.Where(lkl => lkl.MaKLKT.StartsWith("KL")).ToList();
            ViewBag.LKL = kl.ToString();

            return View(dsPhieuKLKT);
        }

        //Cập nhật phiếu KLKT
        public ActionResult UpdateRewardsDisciplines(string id, string idNV)
        {
            PhieuKLKT model = db.PhieuKLKT.Find(id);

            //Dropdown List MaPL
            ViewBag.MaPL = new SelectList(db.PhieuLuong, "MaPL", "MaPL", model.MaPL);

            //Dropdown List LoaiKLKT
            ViewBag.MaKLKT = new SelectList(db.LoaiKLKT, "MaKLKT", "TenKLKT", model.MaKLKT);

            //Lấy ảnh đại diện nhân viên
            ViewBag.hinhNV = db.NhanVien.FirstOrDefault(nv => nv.MaNV == idNV).AnhDaiDienNV;

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateRewardsDisciplines(PhieuKLKT model)
        {
            // Find objects
            var updateModel = db.PhieuKLKT.Find(model.SoPhieuKLKT);

            //Dropdown List MaPL
            var listMaPL = db.PhieuLuong.ToList();
            ViewBag.MaPL = new SelectList(listMaPL, "MaPL", "MaPL");

            //Dropdown List LoaiKLKT
            var listKLKT = db.LoaiKLKT.ToList();
            ViewBag.MaKLKT = new SelectList(listKLKT, "MaKLKT", "TenKLKT");

            // Assign value for objects
            updateModel.MaPL = model.MaPL;
            updateModel.MaKLKT = model.MaKLKT;
            updateModel.ThoiGian = model.ThoiGian;

            //Save Changes
            db.SaveChanges();

            return RedirectToAction("ListRewardDiscipline");
        }

        #endregion
    }
}
    