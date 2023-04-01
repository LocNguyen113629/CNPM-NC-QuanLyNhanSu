using HR_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR_Manager.Controllers
{
    public class AdminController : Controller
    {
        //Khai báo database
        HRManagerEntities4 db = new HRManagerEntities4();

        // GET: Admin
        public ActionResult Admin_HomePage()
        {
            return View();
        }

        //Danh sách tài khoản
        public ActionResult ListAccount()
        {
            List<TaiKhoan> dsTaiKhoan = db.TaiKhoan.ToList();
            return View(dsTaiKhoan);
        }

        //Thêm tài khoản mới
        public ActionResult AddNewAccount()
        {
            List<NhanVien> dsNV = db.NhanVien.Where(st => st.TinhTrangTaiKhoan == false).ToList();
            List<SelectListItem> myNV = new List<SelectListItem>();
            foreach (NhanVien v in dsNV)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = v.MaNV,
                    Value = v.MaNV
                };

                myNV.Add(item);
            }

            ViewBag.dsNhanVien = myNV;

            return View();
        }
        
        [HttpPost]
        public ActionResult AddNewAccount(TaiKhoan model)
        {
            //Add new record
            Random rd = new Random();
            var matk = "NVPO_" + rd.Next(111, 999);
            model.MaTK = matk;

            db.TaiKhoan.Add(model);

            //Save changes in record
            db.SaveChanges();
            return RedirectToAction("ListAccount");
        }


        //Xoá tài khoản (ĐANG LỖI)
        public ActionResult DeleteAccount(int id)
        {
            // Find objects by ID
            TaiKhoan model = db.TaiKhoan.Find(id);
            //var updateModel = db.MayTinh.Find(id);
            db.TaiKhoan.Remove(model);

            // Save Change
            db.SaveChanges();

            return RedirectToAction("ListAccount");
        }

    }
}