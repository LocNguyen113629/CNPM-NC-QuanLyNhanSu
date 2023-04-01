using HR_Manager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebGrease.Css.Extensions;

namespace HR_Manager.Controllers
{
    public class LoginController : Controller
    {
        //Khai báo Database
        HRManagerEntities4 db = new HRManagerEntities4();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            #region Check account
            var staff = "NV";
            var hr = "QL_NhanSu";
            var salary = "QL_Luong";
            #endregion

            var getdata = db.TaiKhoan.Where(tk => tk.Username.Equals(username) && tk.Password.Equals(password)).FirstOrDefault();
            var taikhoan = db.TaiKhoan.SingleOrDefault(nv => nv.Username.Equals(username) && nv.Password.Equals(password));
            
            if (username == "admin" || password == "admin")
            {
                return RedirectToAction("Admin_HomePage", "Admin");

            }

            if (getdata == null)
            {
                TempData["error"] = "Đăng nhập không thành công";
                return View("Login");
            }

            else if (getdata != null)
            {
                //add session
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["user"] = taikhoan;
                ViewBag.user = taikhoan;

                if (getdata.NhanVien.MaCV.ToString() == staff)
                {
                    return RedirectToAction("ST_HomePage", "Staff");
                }

                else if (getdata.NhanVien.MaCV.ToString() == salary)
                {
                    return RedirectToAction("SA_HomePage", "Salary");
                }

                else if (getdata.NhanVien.MaCV.ToString() == hr)
                {
                    return RedirectToAction("HR_HomePage", "HR");
                }
            }

            return View();
        }

        public ActionResult Logout() 
        {
            //Xoá session user hiện tại
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}