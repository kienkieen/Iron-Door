using Nhom12_Website_Ban_Cua.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nhom12_Website_Ban_Cua.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        DBBanCuaEntities CSDL = new DBBanCuaEntities();


        // GET: Admin/Account
        public ActionResult DSTaiKhoan()
        {
            List<NguoiDung> DSTK = CSDL.NguoiDungs.ToList();
            return View(DSTK);
        }

        public ActionResult AdminLogin()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AdminLogin(string tentaikhoan, string matkhau)
        {
            if (string.IsNullOrEmpty(tentaikhoan) || string.IsNullOrEmpty(matkhau))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập tên đăng nhập admin và mật khẩu.";
                return View();
            }

            var taiKhoan = CSDL.NguoiDungs.FirstOrDefault(t => t.TenTaiKhoan == tentaikhoan && t.MaLoaiNguoiDung == "Admin");

            if (taiKhoan == null)
            {
                ViewBag.ErrorMessage = "Tên tài khoản admin trên không tồn tại!";
                return View();
            }

            if (taiKhoan.MatKhau == matkhau)
            {
                Session["TaiKhoanAdmin"] = tentaikhoan;
                return RedirectToAction("Index", "AdminHome");
            }
            else
            {
                ViewBag.ErrorMessage = "Mật khẩu không đúng!";
                return View();
            }
        }
    }
}