using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nhom12_Website_Ban_Cua.Models.EF;

namespace Nhom12_Website_Ban_Cua.Controllers
{
    
    public class HomeController : Controller
    {
        DBBanCuaEntities CSDL = new DBBanCuaEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GioiThieu()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult LienHe()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LienHe(FormCollection fc)
        {
            if (Session["TaiKhoan"] == null)
            {
                ViewBag.ErrorMessage = "Vui lòng đăng nhập để yêu cầu hỗ trợ liên hệ cùng với chúng tôi!";
                return View("LienHe");
            }
            int maNguoiDung = (int)Session["MaNguoiDung"];

            string hoTen = fc["hoten"];
            string email = fc["email"];
            string tieuDe = fc["tieude"];
            string noiDung = fc["noidung"];
            int maTrangThai = 2;

            try
            {
                YeuCauHoTro yc = new YeuCauHoTro
                {
                    MaNguoiDung = maNguoiDung,
                    NoiDung = $"{tieuDe}\n{hoTen} ({email}): {noiDung}",
                    MaTrangThai = maTrangThai,
                    NgayYeuCau = DateTime.Now
                };

                CSDL.YeuCauHoTroes.Add(yc);
                CSDL.SaveChanges();

                ViewBag.SuccessMessage = "Yêu cầu hỗ trợ của bạn đã được gửi thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra: " + ex.Message;
            }

            return View("LienHe");
        }

        public ActionResult Partial_LienHe()
        {
            ViewBag.Message = "Your application description page.";

            return PartialView("Partial_LienHe");
        }


        public ActionResult Navbar_Menu()
        {
            List<Loai> LoaiSP = CSDL.Loais.ToList();
            return PartialView(LoaiSP);
        }

        public ActionResult Product_MaybeLike()
        {
            List<SanPham> SP_MaybeLike = CSDL.SanPhams.OrderBy(x => Guid.NewGuid()).Take(4).ToList();
            return PartialView("ProductIndex", SP_MaybeLike);
        }

        public ActionResult Product_Top()
        {
            List<SanPham> SP_Top = CSDL.SanPhams.OrderByDescending(x => x.SoLuongBan).Take(4).ToList();
            return PartialView("ProductIndex", SP_Top);
        }
        public ActionResult Product_FlashSale()
        {
            List<SanPham> SP_FlashSale = CSDL.SanPhams.OrderByDescending(x => x.GiamGia).Take(4).ToList();
            return PartialView("ProductIndex", SP_FlashSale);
        }
    }
}