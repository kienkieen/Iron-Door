using Nhom12_Website_Ban_Cua.Models;
using Nhom12_Website_Ban_Cua.Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Nhom12_Website_Ban_Cua.Controllers
{
    public class ShoppingCartController : Controller
    {
        DBBanCuaEntities CSDL = new DBBanCuaEntities();

        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ThemHang(int maSP)
        {
            // Kiểm tra khách hàng đã đăng nhập chưa
            if (Session["TaiKhoan"] == null)
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng!";
                return Json(new
                {
                    success = false,
                    msg = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng!",
                    redirect = Url.Action("Index", "Account")
                }, JsonRequestBehavior.AllowGet);
            }

            // Lấy giỏ hàng từ Session
            GioHang gh = Session["gh"] as GioHang;
            if (gh == null)
            {
                gh = new GioHang();
                Session["SoLuongHangGH"] = 0;
            }

            // Thêm sản phẩm vào giỏ hàng
            int kq = gh.Them(maSP);
            Session["gh"] = gh;

            // Cập nhật số lượng sản phẩm trong giỏ hàng
            Session["SoLuongHangGH"] = gh.SoMatHang();

            // Kiểm tra kết quả thêm sản phẩm
            bool isSuccess = kq != -1;

            if (isSuccess)
            {
                TempData["SuccessMessage"] = "Thêm sản phẩm vào giỏ hàng thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm sản phẩm!";
            }

            return Json(new
            {
                success = isSuccess,
                msg = isSuccess ? "Thêm sản phẩm thành công!" : "Có lỗi xảy ra!",
                count = gh.SoMatHang()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult XemGioHang()
        {
            if (Session["TaiKhoan"] == null)
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để xem giỏ hàng!";
                return RedirectToAction("Index", "Account");
            }

            GioHang gh = (GioHang)Session["gh"];
            double tongTien = 0;

            if (gh != null)
            {
                tongTien = gh.TongTien();
                Session["SoLuongHangGH"] = gh.SoMatHang(); // Đồng bộ số lượng sản phẩm
            }
            else
            {
                Session["SoLuongHangGH"] = 0; // Giỏ hàng rỗng
                TempData["InfoMessage"] = "Giỏ hàng của bạn đang trống.";
            }

            ViewBag.tongTien = tongTien;
            return View(gh);
        }

        public ActionResult XoaHang(int maSP)
        {
            // Lấy giỏ hàng từ session
            GioHang gh = Session["gh"] as GioHang;

            if (gh != null && gh.lst != null)
            {
                // Tìm sản phẩm trong giỏ hàng theo mã sản phẩm
                var itemXoa = gh.lst.FirstOrDefault(s => s.maSP == maSP);
                if (itemXoa != null)
                {
                    gh.lst.Remove(itemXoa);  // Xóa sản phẩm khỏi giỏ hàng
                    Session["SoLuongHangGH"] = gh.SoMatHang();  // Cập nhật lại số lượng sản phẩm
                    TempData["SuccessMessage"] = "Đã xóa sản phẩm khỏi giỏ hàng thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Sản phẩm không tồn tại trong giỏ hàng.";
                }
            }
            else
            {
                TempData["InfoMessage"] = "Giỏ hàng của bạn đang trống.";
            }

            return RedirectToAction("XemGioHang", "ShoppingCart");
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int maSP, int soLuong)
        {
            // Kiểm tra xem giỏ hàng có tồn tại trong Session không
            if (Session["gh"] == null)
            {
                TempData["ErrorMessage"] = "Giỏ hàng rỗng hoặc không hợp lệ.";
                return Json(new { success = false, message = "Giỏ hàng rỗng hoặc không hợp lệ." });
            }

            GioHang gh = Session["gh"] as GioHang;

            // Tìm sản phẩm theo maSP
            var item = gh.lst.FirstOrDefault(c => c.maSP == maSP);

            if (item != null)
            {
                item.soLuong = soLuong;
                TempData["SuccessMessage"] = "Đã cập nhật số lượng sản phẩm thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại trong giỏ hàng.";
                return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng." });
            }

            // Cập nhật lại Session
            Session["gh"] = gh;
            return Json(new { success = true });
        }

        public ActionResult ThanhToan()
        {
            GioHang ghang = (GioHang)Session["gh"];
            if (ghang == null || ghang.lst == null || ghang.lst.Count == 0)
            {
                TempData["ErrorMessage"] = "Giỏ hàng rỗng! Vui lòng thêm sản phẩm trước khi thanh toán.";
                return RedirectToAction("Index", "Products");
            }
            return View(ghang);
        }

        [HttpPost]
        public ActionResult XacNhanThanhToan(FormCollection fc)
        {
            try
            {
                GioHang gioHang = Session["gh"] as GioHang;

                if (gioHang == null || gioHang.lst.Count == 0)
                {
                    TempData["ErrorMessage"] = "Giỏ hàng trống hoặc có lỗi xảy ra!";
                    return RedirectToAction("ThongBaoThanhToan", "ShoppingCart");
                }

                string nd = Session["TaiKhoan"] as string;
                NguoiDung ND = CSDL.NguoiDungs.FirstOrDefault(t => t.TenTaiKhoan == nd);

                if (ND == null)
                {
                    TempData["ErrorMessage"] = "Người dùng không hợp lệ!";
                    return RedirectToAction("ThongBaoThanhToan", "ShoppingCart");
                }

                // Lấy danh sách các mã người vận chuyển
                var dsNguoiVanChuyen = CSDL.NguoiVanChuyens.Select(nvc => nvc.MaNguoiVanChuyen).ToList();

                if (dsNguoiVanChuyen.Count == 0)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy người vận chuyển!";
                    return RedirectToAction("ThongBaoThanhToan", "ShoppingCart");
                }

                // Lấy ngẫu nhiên một mã người vận chuyển
                Random rand = new Random();
                int maNguoiVanChuyenNgauNhien = dsNguoiVanChuyen[rand.Next(dsNguoiVanChuyen.Count)];

                int pttt_int = int.Parse(fc["PTTT"]);
                string PTTT;
                switch (pttt_int)
                {
                    case 1:
                        PTTT = "Tiền Mặt";
                        break;
                    case 2:
                        PTTT = "Thẻ Tín Dụng";
                        break;
                    case 3:
                        PTTT = "Thẻ Ghi Nợ";
                        break;
                    case 4:
                        PTTT = "PayPal";
                        break;
                    default:
                        PTTT = "Tiền Mặt"; // Mặc định
                        break;
                }

                string DiaChi = fc["diachi"];

                // Kiểm tra địa chỉ giao hàng
                if (string.IsNullOrWhiteSpace(DiaChi))
                {
                    TempData["ErrorMessage"] = "Vui lòng nhập địa chỉ giao hàng!";
                    return RedirectToAction("ThanhToan", "ShoppingCart");
                }

                // Tạo đơn hàng mới
                DatHang dh = new DatHang
                {
                    MaNguoiDung = ND.MaNguoiDung,
                    MaNguoiVanChuyen = maNguoiVanChuyenNgauNhien,
                    NgayDatHang = DateTime.Now,
                    DiaChiVanChuyen = DiaChi,
                    TongTienThanhToan = (decimal)gioHang.TongTien(), // Tổng tiền theo giá đã giảm
                    PhuongThucThanhToan = PTTT,
                    TrangThaiThanhToan = "Chưa Thanh Toán",
                    NgayGiaoHang = DateTime.Now.AddDays(3),
                    MaTrangThai = 1
                };

                CSDL.DatHangs.Add(dh);
                CSDL.SaveChanges();

                // Lưu chi tiết đơn hàng với giá đã giảm
                foreach (var item in gioHang.lst)
                {
                    ChiTietDatHang ctdh = new ChiTietDatHang
                    {
                        MaDonDatHang = dh.MaDonDatHang,
                        MaSP = item.maSP,
                        SoLuong = item.soLuong,
                        Gia = (int)Math.Round(item.donGia), // Lưu giá đã giảm (làm tròn để phù hợp với kiểu int)
                    };
                    CSDL.ChiTietDatHangs.Add(ctdh);

                    // Cập nhật số lượng tồn kho và số lượng bán
                    SanPham sanPham = CSDL.SanPhams.Find(item.maSP);
                    if (sanPham != null)
                    {
                        sanPham.SoLuongTon = Math.Max(0, (sanPham.SoLuongTon ?? 0) - item.soLuong);
                        sanPham.SoLuongBan = (sanPham.SoLuongBan ?? 0) + item.soLuong;
                    }
                }

                CSDL.SaveChanges();

                // Xóa giỏ hàng sau khi thanh toán thành công
                Session["gh"] = null;
                Session["SoLuongHangGH"] = 0;

                TempData["SuccessMessage"] = $"Thanh toán thành công! Đơn hàng #{dh.MaDonDatHang} đã được tạo và sẽ được giao trong 3 ngày tới.";
                return RedirectToAction("ThongBaoThanhToan", "ShoppingCart");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xử lý đơn hàng: " + ex.Message;
                return RedirectToAction("ThongBaoThanhToan", "ShoppingCart");
            }
        }

        public ActionResult ThongBaoThanhToan()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.InfoMessage = TempData["InfoMessage"];
            return View();
        }

        public ActionResult _Partial_GioHang()
        {
            GioHang gh = (GioHang)Session["gh"];
            if (gh != null)
            {
                ViewBag.tongTien = gh.TongTien();
            }
            return PartialView(gh);
        }

        public ActionResult Mua1SP(int MSP)
        {
            // Kiểm tra khách hàng đã đăng nhập chưa
            if (Session["TaiKhoan"] == null)
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để mua sản phẩm!";
                return RedirectToAction("Index", "Account"); // Hoặc trang đăng nhập của bạn
            }

            // Kiểm tra sản phẩm có tồn tại không
            SanPham sp = CSDL.SanPhams.FirstOrDefault(t => t.MaSP == MSP);
            if (sp == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại!";
                return RedirectToAction("Index", "Products");
            }

            // Kiểm tra sản phẩm còn hàng không
            if (sp.SoLuongTon <= 0)
            {
                TempData["ErrorMessage"] = "Sản phẩm đã hết hàng!";
                return RedirectToAction("ChiTiet", "Products", new { id = MSP });
            }

            try
            {
                // Lấy giỏ hàng từ Session hoặc tạo mới
                GioHang gh = (GioHang)Session["gh"];
                if (gh == null)
                {
                    gh = new GioHang();
                    Session["SoLuongHangGH"] = 0;
                }

                // Lưu tạm giỏ hàng cũ vào Session để có thể khôi phục
                if (gh.lst != null && gh.lst.Count > 0)
                {
                    Session["gh_backup"] = new GioHang(new List<CartItem>(gh.lst));
                }

                // Xóa tạm thời giỏ hàng cũ để chỉ mua 1 sản phẩm này
                gh.lst.Clear();

                // Thêm sản phẩm vào giỏ hàng
                int kq = gh.Them(MSP);
                if (kq == -1)
                {
                    // Nếu lỗi, khôi phục lại giỏ hàng cũ
                    if (Session["gh_backup"] != null)
                    {
                        Session["gh"] = Session["gh_backup"];
                        Session["gh_backup"] = null;
                    }
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng!";
                    return RedirectToAction("ChiTiet", "Products", new { id = MSP });
                }

                // Cập nhật Session
                Session["gh"] = gh;
                Session["SoLuongHangGH"] = gh.SoMatHang();

                TempData["InfoMessage"] = "Đã thêm sản phẩm vào giỏ hàng. Tiến hành thanh toán ngay.";

                // Chuyển trực tiếp đến trang thanh toán
                return RedirectToAction("ThanhToan", "ShoppingCart");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("ChiTiet", "Products", new { id = MSP });
            }
        }
    }
}