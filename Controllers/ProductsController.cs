using Nhom12_Website_Ban_Cua.Models.EF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Data.SqlClient;
using Nhom12_Website_Ban_Cua.Models;

namespace Nhom12_Website_Ban_Cua.Controllers
{
    public class ProductsController : Controller
    {

        DBBanCuaEntities CSDL = new DBBanCuaEntities();

        // GET: Products
        public ActionResult Index(string Searchtext)
        {
            // Lấy toàn bộ danh sách sản phẩm
            var items = CSDL.SanPhams.AsQueryable();

            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.TenSP.Contains(Searchtext) || x.Loai.TenLoai.Contains(Searchtext));
                // Truyền từ khóa tìm kiếm vào ViewBag để hiển thị trong thông báo
                ViewBag.SearchText = Searchtext;
            }

            items = items.Where(x => x.SoLuongTon > 0);

            return View(items.ToList());
        }

        public ActionResult Menu_LoaiCua()
        {
            List<Loai> DSLC = CSDL.Loais.ToList();
            return PartialView(DSLC);
        }

        public ActionResult SanPhamTheoLoai(int loaiSP)
        {
            List<SanPham> sanPhams = new List<SanPham>();

            if (loaiSP == 0)
            {
                // Lấy tất cả sản phẩm khi loaiSP == 0
                sanPhams = CSDL.SanPhams.ToList();
            }
            else
            {
                // Lọc sản phẩm theo mã loại
                sanPhams = CSDL.SanPhams.Where(s => s.MaLoai == loaiSP).ToList();
            }

            // Trả về PartialView thay vì View
            return PartialView("_SanPhamTheoLoai", sanPhams);
        }

        public ActionResult SX_SPBanChay()
        {
            List<SanPham> SP_Top = CSDL.SanPhams.OrderByDescending(x => x.SoLuongBan).ToList();
            return View("Index", SP_Top);
        }

        public ActionResult SX_SPGiaRe()
        {
            List<SanPham> SP_GiaRe = CSDL.SanPhams.OrderBy(x => x.Gia).ToList();
            return View("Index", SP_GiaRe);
        }

        public ActionResult SX_SPNgauNhien()
        {
            List<SanPham> SP_GiaRe = CSDL.SanPhams.OrderBy(x => Guid.NewGuid()).ToList();
            return View("Index", SP_GiaRe);
        }

        public ActionResult Showing4SP()
        {
            List<SanPham> sanPhams = CSDL.SanPhams.Take(4).ToList();
            return View("Index", sanPhams);
        }

        public ActionResult Showing8SP()
        {
            List<SanPham> sanPhams = CSDL.SanPhams.Take(8).ToList();
            return View("Index", sanPhams);
        }

        public ActionResult ShowingAll()
        {
            List<SanPham> sanPhams = CSDL.SanPhams.ToList();
            return View("Index", sanPhams);
        }

        public ActionResult Navbar_TheoLoaiSP(int maSP)
        {
            List<SanPham> sanPhams = CSDL.SanPhams.Where(s => s.MaLoai == maSP).ToList();
            return View("Index", sanPhams);
        }


        public ActionResult SanPhamTheoGia(List<string> prices)
        {
            List<SanPham> sanPhams = new List<SanPham>();

            // Kiểm tra nếu prices không có giá trị, trả về toàn bộ sản phẩm
            if (prices == null || prices.Count == 0)
            {
                sanPhams = CSDL.SanPhams.ToList();
                return PartialView("_SanPhamTheoLoai", sanPhams);
            }

            // Chuyển khoảng giá từ chuỗi sang tuple (min, max)
            List<(decimal min, decimal max)> priceRanges = new List<(decimal, decimal)>();

            foreach (var price in prices)
            {
                decimal min = 0;
                decimal max = 0;

                switch (price)
                {
                    case "0":
                        min = 0;
                        max = 100000000;
                        break;
                    case "0-5":
                        min = 0;
                        max = 5000000;
                        break;
                    case "5-10":
                        min = 5000000;
                        max = 10000000;
                        break;
                    case "10-15":
                        min = 10000000;
                        max = 15000000; // Sửa lại từ 150000000
                        break;
                    case "15-20":
                        min = 15000000;
                        max = 20000000; // Sửa lại từ 200000000
                        break;
                    case "20":
                        min = 20000000;
                        max = 100000000;
                        break;
                    default:
                        continue;
                }

                priceRanges.Add((min, max));
            }

            // Tạo danh sách các điều kiện WHERE cho từng khoảng giá
            var query = CSDL.SanPhams.AsQueryable();
            var filteredProducts = new List<SanPham>();

            foreach (var range in priceRanges)
            {
                // Lọc sản phẩm có giá sau giảm nằm trong khoảng
                var productsInRange = query.Where(p =>
                    (p.Gia ?? 0) - ((p.Gia ?? 0) * (p.GiamGia ?? 0) / 100) >= range.min &&
                    (p.Gia ?? 0) - ((p.Gia ?? 0) * (p.GiamGia ?? 0) / 100) <= range.max
                ).ToList();

                filteredProducts.AddRange(productsInRange);
            }

            // Loại bỏ sản phẩm trùng lặp
            sanPhams = filteredProducts.GroupBy(p => p.MaSP).Select(g => g.First()).ToList();

            return PartialView("_SanPhamTheoLoai", sanPhams);
        }

        // Phương thức hỗ trợ để tính giá sau khi giảm (có thể dùng ở nơi khác)
        public static decimal TinhGiaSauGiam(int? giaGoc, int? giamGia)
        {
            if (!giaGoc.HasValue) return 0;

            decimal gia = giaGoc.Value;
            decimal phanTramGiam = giamGia ?? 0;

            return gia - (gia * phanTramGiam / 100);
        }

        public ActionResult ChiTietSanPham(int id)
        {
            var SPham = CSDL.SanPhams.FirstOrDefault(m => m.MaSP == id);

            if (SPham == null)
            {
                return HttpNotFound("Không tìm thấy sản phẩm!");
            }

            // Truy vấn tách chuỗi AnhNho
            List<string> Anh;
            if (!string.IsNullOrEmpty(SPham.AnhNho))
            {
                Anh = CSDL.Database.SqlQuery<string>(
                    "SELECT Item FROM dbo.SplitString(@AnhNho, ',')",
                    new SqlParameter("@AnhNho", SPham.AnhNho)).ToList();
                Anh = Anh.Select(a => a.Trim()).ToList();
            }
            else
            {
                Anh = new List<string>(); // Nếu không có ảnh nào
            }

            // Truy vấn tách chuỗi MauSac
            List<string> MauSac;
            if (!string.IsNullOrEmpty(SPham.MauSac))
            {
                MauSac = CSDL.Database.SqlQuery<string>(
                    "SELECT Item FROM dbo.SplitString(@MauSac, ',')",
                    new SqlParameter("@MauSac", SPham.MauSac)).ToList();
                MauSac = MauSac.Select(a => a.Trim()).ToList();
            }
            else
            {
                MauSac = new List<string>(); // Nếu không có màu nào
            }

            List<string> HuongCua;
            if (!string.IsNullOrEmpty(SPham.HuongCua))
            {
                HuongCua = CSDL.Database.SqlQuery<string>(
                    "SELECT Item FROM dbo.SplitString(@HuongCua, ',')",
                    new SqlParameter("@HuongCua", SPham.HuongCua)).ToList();
                HuongCua = HuongCua.Select(a => a.Trim()).ToList();
            }
            else
            {
                HuongCua = new List<string>(); 
            }

            ViewBag.HuongCua = HuongCua;
            ViewBag.MauSac = MauSac;
            ViewBag.AnhSP = Anh;
            ViewBag.MaSP = id;

            return View(SPham);
        }

        


    }
}