using Nhom12_Website_Ban_Cua.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nhom12_Website_Ban_Cua.Areas.Admin.Controllers
{
    public class AdminProductsController : Controller
    {
        DBBanCuaEntities CSDL = new DBBanCuaEntities();

        // GET: Admin/AdminProducts
        public ActionResult DanhSachSP()
        {
            // Lấy toàn bộ danh sách sản phẩm
            List<SanPham> DSSP = CSDL.SanPhams.ToList();

            return View(DSSP);
        }

        public ActionResult ThemSPMoi()
        {
            ViewBag.LoaiSanPham = new SelectList(CSDL.SanPhams.ToList(), "MaSP", "TenSP");
            return View();
        }

        [HttpPost]
        public ActionResult Xoa(int id)
        {
            var item = CSDL.SanPhams.Find(id);
            if (item != null)
            {
                CSDL.SanPhams.Remove(item);
                CSDL.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}