using HR_Manager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace HR_Manager.Controllers
{
    public class StaffController : Controller
    {
        //Khai báo database
        HRManagerEntities4 db = new HRManagerEntities4();

        // GET: Staff
        public ActionResult ST_Homepage()
        {
            //Hiển thị DropdownList danh sách loại nghỉ phép
            var listLNP = db.LoaiNghiPhep.ToList();
            ViewBag.MaLoaiNP = new SelectList(listLNP, "MaLoaiNP", "TenLoaiNP");

            return View();
        }

        [HttpPost]
        public ActionResult ST_Homepage(PhieuNghiPhep pnp_model, ChamCong cc_model)
        {
            #region Tạo phiếu nghỉ phép
            if (pnp_model.ImageUploadPNP != null)
            {
                string fileName = Path.GetFileName(pnp_model.ImageUploadPNP.FileName);
                pnp_model.AnhNghiPhepNV = "/Images/ST_ApplicationForLeave/" + fileName;
                pnp_model.ImageUploadPNP.SaveAs(Path.Combine(Server.MapPath("~/Images/ST_ApplicationForLeave/"), fileName));
            }

            //Add new record
            Random rd = new Random();
            var sophieunp = "PN" + rd.Next(111, 999);
            pnp_model.SoPhieuNP = sophieunp;
            pnp_model.NgayLapPNP = DateTime.Now;

            var trangthainp = "Chưa phê duyệt";
            pnp_model.TrangThai = trangthainp;

            ViewBag.MaLoaiNP = new SelectList(db.LoaiNghiPhep, "MaLoaiNP", "TenLoaiNP", pnp_model.MaLoaiNP);

            db.PhieuNghiPhep.Add(pnp_model);
            db.SaveChanges();
            #endregion

            

            return View();
        }
    }
}