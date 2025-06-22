using Nhom12_Website_Ban_Cua.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nhom12_Website_Ban_Cua.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {

        DBBanCuaEntities CSDL = new DBBanCuaEntities();

        // GET: Admin/ProductCategory
        public ActionResult DSLoaiSP()
        {

            List<Loai> DSL = CSDL.Loais.ToList();
            return View(DSL);
        }
    }
}