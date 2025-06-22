using Nhom12_Website_Ban_Cua.Models;
using Nhom12_Website_Ban_Cua.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Mvc;

namespace Nhom12_Website_Ban_Cua.Controllers
{
    public class UserAccountController : Controller
    {

        DBBanCuaEntities CSDL = new DBBanCuaEntities();

        // GET: RegisterAndLogin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string tentaikhoan, string matkhau)
        {
            // Kiểm tra trường bắt buộc
            if (string.IsNullOrWhiteSpace(tentaikhoan))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập tên tài khoản!";
                ViewBag.TenTaiKhoan = tentaikhoan; // Giữ lại dữ liệu đã nhập
                return View();
            }

            if (string.IsNullOrWhiteSpace(matkhau))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập mật khẩu!";
                ViewBag.TenTaiKhoan = tentaikhoan; // Giữ lại dữ liệu đã nhập
                return View();
            }

            var taiKhoan = CSDL.NguoiDungs.FirstOrDefault(t => t.TenTaiKhoan == tentaikhoan);
            if (taiKhoan == null)
            {
                ViewBag.ErrorMessage = "Tên tài khoản không tồn tại!";
                ViewBag.TenTaiKhoan = tentaikhoan; // Giữ lại dữ liệu đã nhập
                return View();
            }

            if (taiKhoan.MatKhau == matkhau)
            {
                Session["TaiKhoan"] = tentaikhoan;
                Session["MaNguoiDung"] = taiKhoan.MaNguoiDung;
                Session["MaLoaiNguoiDung"] = taiKhoan.MaLoaiNguoiDung;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Mật khẩu không đúng!";
                ViewBag.TenTaiKhoan = tentaikhoan; // Giữ lại dữ liệu đã nhập
                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection fc)
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(fc["hoten"]))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập tên đầy đủ!";
                return View("Register");
            }

            if (string.IsNullOrWhiteSpace(fc["email"]))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập email!";
                return View("Register");
            }

            if (string.IsNullOrWhiteSpace(fc["sdt"]))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập số điện thoại!";
                return View("Register");
            }

            if (string.IsNullOrWhiteSpace(fc["diachi"]))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập địa chỉ!";
                return View("Register");
            }

            if (string.IsNullOrWhiteSpace(fc["tentaikhoan"]))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập tên tài khoản!";
                return View("Register");
            }

            if (string.IsNullOrWhiteSpace(fc["matkhau_dk"]))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập mật khẩu!";
                return View("Register");
            }

            if (string.IsNullOrWhiteSpace(fc["xacnhan_matkhau"]))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập xác nhận mật khẩu!";
                return View("Register");
            }

            string tentaikhoan = fc["tentaikhoan"];
            NguoiDung KTtentk = CSDL.NguoiDungs.FirstOrDefault(t => t.TenTaiKhoan == tentaikhoan);
            if (KTtentk != null)
            {
                ViewBag.ErrorMessage = "Tên tài khoản đã tồn tại. Vui lòng nhập tên khác!";
                return View("Register");
            }

            // Kiểm tra email định dạng hợp lệ
            string email = fc["email"];
            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            if (!emailRegex.IsMatch(email))
            {
                ViewBag.ErrorMessage = "Email không hợp lệ!";
                return View("Register");
            }

            // Kiểm tra số điện thoại có đúng 10 chữ số
            string sdt = fc["sdt"];
            var phoneRegex = new System.Text.RegularExpressions.Regex(@"^\d{10}$");
            if (!phoneRegex.IsMatch(sdt))
            {
                ViewBag.ErrorMessage = "Số điện thoại phải có 10 chữ số!";
                return View("Register");
            }

            // Kiểm tra mật khẩu (bao gồm chữ và số, ít nhất 6 ký tự)
            string matkhau = fc["matkhau_dk"];
            var passwordRegex = new System.Text.RegularExpressions.Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$");
            if (!passwordRegex.IsMatch(matkhau))
            {
                ViewBag.ErrorMessage = "Mật khẩu phải bao gồm ít nhất một chữ cái, một chữ số và có ít nhất 6 ký tự!";
                return View("Register");
            }

            string xnmatkhau = fc["xacnhan_matkhau"];
            if (xnmatkhau != matkhau)
            {
                ViewBag.ErrorMessage = "Xác nhận mật khẩu khác với mật khẩu ban đầu!";
                return View("Register");
            }

            NguoiDung kh = new NguoiDung
            {
                TenNguoiDung = fc["hoten"],
                Email = email,
                SDT = sdt,
                DiaChi = fc["diachi"],
                TenTaiKhoan = tentaikhoan,
                MatKhau = matkhau,
                MaLoaiNguoiDung = "User"
            };

            try
            {
                CSDL.NguoiDungs.Add(kh);
                CSDL.SaveChanges();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi: " + ex.Message;
                return View("Register");
            }
        }


        public ActionResult Logout()
        {
            Session.Abandon();
           
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ThongTinNguoiDung()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Login", "UserAccount"); // Chuyển hướng đến trang đăng nhập
            }

            var tentk = Session["TaiKhoan"];
            NguoiDung ND = CSDL.NguoiDungs.FirstOrDefault(t => t.TenTaiKhoan == tentk.ToString());
            return View(ND);
        }

        public ActionResult Partial_HoSo()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Login", "UserAccount"); // Chuyển hướng đến trang đăng nhập
            }

            var tentk = Session["TaiKhoan"];
            NguoiDung ND = CSDL.NguoiDungs.FirstOrDefault(t => t.TenTaiKhoan == tentk.ToString());
            return PartialView(ND);
        }

        public ActionResult Partial_DoiMatKhau()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Login", "UserAccount"); // Chuyển hướng đến trang đăng nhập
            }
            var tentk = Session["TaiKhoan"];
            NguoiDung ND = CSDL.NguoiDungs.FirstOrDefault(t => t.TenTaiKhoan == tentk.ToString());
            return PartialView(ND);
        }

        public ActionResult Partial_LSGiaoHang()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Login", "UserAccount"); // Chuyển hướng đến trang đăng nhập
            }
            var tentk = Session["TaiKhoan"];
            NguoiDung ND = CSDL.NguoiDungs.FirstOrDefault(t => t.TenTaiKhoan == tentk.ToString());
            List<DatHang> DSDH = CSDL.DatHangs.Where(t => t.MaNguoiDung == ND.MaNguoiDung).ToList();

            // Tính lại tổng tiền cho từng đơn hàng với giá đã giảm
            foreach (var donHang in DSDH)
            {
                List<ChiTietDatHang> DSCTDH = CSDL.ChiTietDatHangs.Where(t => t.MaDonDatHang == donHang.MaDonDatHang).ToList();

                // Tính lại giá sau giảm giá cho từng chi tiết
                foreach (var item in DSCTDH)
                {
                    var sanPham = CSDL.SanPhams.FirstOrDefault(sp => sp.MaSP == item.MaSP);
                    if (sanPham != null && sanPham.GiamGia > 0)
                    {
                        item.Gia = sanPham.Gia - (sanPham.Gia * sanPham.GiamGia / 100);
                    }
                }

                // Tính lại tổng tiền cho đơn hàng
                donHang.TongTienThanhToan = DSCTDH.Sum(ct => ct.Gia * ct.SoLuong);
            }

            return PartialView(DSDH);
        }

        public ActionResult Partial_XemChiTietDH(int MaDH)
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Login", "UserAccount");
            }

            DatHang dh = CSDL.DatHangs.FirstOrDefault(t => t.MaDonDatHang == MaDH);

            List<ChiTietDatHang> DSCTDH = CSDL.ChiTietDatHangs.Where(t => t.MaDonDatHang == MaDH).ToList();

            // Tính lại giá sau giảm giá cho từng chi tiết
            foreach (var item in DSCTDH)
            {
                var sanPham = CSDL.SanPhams.FirstOrDefault(sp => sp.MaSP == item.MaSP);
                if (sanPham != null && sanPham.GiamGia > 0)
                {
                    item.Gia = sanPham.Gia - (sanPham.Gia * sanPham.GiamGia / 100);
                }
            }

            // Tính lại tổng tiền
            ViewBag.TongTien = DSCTDH.Sum(ct => ct.Gia * ct.SoLuong);

            return PartialView(DSCTDH);
        }

        public ActionResult Partial_DonHangDangGiao()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Login", "UserAccount"); // Chuyển hướng đến trang đăng nhập
            }
            var tentk = Session["TaiKhoan"];
            NguoiDung ND = CSDL.NguoiDungs.FirstOrDefault(t => t.TenTaiKhoan == tentk.ToString());

            List<DatHang> DSDH = CSDL.DatHangs.Where(t => t.MaNguoiDung == ND.MaNguoiDung && t.TrangThai.TenTrangThai == "Đang giao hàng").ToList();

            return PartialView(DSDH);
        }

        [HttpPost]
        public ActionResult DoiMatKhau(string matkhau, string matkhaumoi, string xacnhanmkmoi)
        {
            try
            {
                // Lấy thông tin người dùng hiện tại (giả sử sử dụng Session để lưu ID người dùng)
                var tentk = Session["TaiKhoan"];
                NguoiDung nguoiDung = CSDL.NguoiDungs.FirstOrDefault(t => t.TenTaiKhoan == tentk.ToString());

                if (nguoiDung == null)
                {
                    return Json(new { success = false, message = "Người dùng không tồn tại." });
                }

                // Kiểm tra mật khẩu hiện tại
                if (nguoiDung.MatKhau != matkhau)
                {
                    return Json(new { success = false, message = "Mật khẩu hiện tại không đúng." });
                }

                // Kiểm tra xác nhận mật khẩu
                if (matkhaumoi != xacnhanmkmoi)
                {
                    return Json(new { success = false, message = "Mật khẩu xác nhận không khớp với mật khẩu mới." });
                }

                // Kiểm tra độ dài hoặc các điều kiện khác của mật khẩu mới nếu cần
                if (matkhaumoi.Length < 6)
                {
                    return Json(new { success = false, message = "Mật khẩu mới phải có ít nhất 6 ký tự." });
                }
                if (matkhaumoi == nguoiDung.MatKhau)
                {
                    return Json(new { success = false, message = "Mật khẩu mới phải khác mật khẩu cũ." });
                }

                // Cập nhật mật khẩu mới
                nguoiDung.MatKhau = matkhaumoi;
                CSDL.SaveChanges();

                return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

        [HttpPost]
        public ActionResult CapNhatHoSo(string ten, string email, string sdt, string diachi)
        {
            try
            {
                // Lấy thông tin người dùng hiện tại (giả sử sử dụng Session để lưu ID người dùng)
                var tentk = Session["TaiKhoan"];
                NguoiDung nguoiDung = CSDL.NguoiDungs.FirstOrDefault(t => t.TenTaiKhoan == tentk.ToString());

                if (nguoiDung == null)
                {
                    return Json(new { success = false, message = "Người dùng không tồn tại." });
                }

                if (!email.EndsWith("@gmail.com"))
                {
                    return Json(new { success = false, message = "Email phải có định dạng @gmail.com." });
                }

                nguoiDung.TenNguoiDung = ten;
                nguoiDung.Email = email;
                nguoiDung.SDT = sdt;
                nguoiDung.DiaChi = diachi;

                CSDL.SaveChanges();

                return Json(new { success = true, message = "Cập nhật hồ sơ thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

    }
}