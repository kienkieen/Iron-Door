using Nhom12_Website_Ban_Cua.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nhom12_Website_Ban_Cua.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        DBBanCuaEntities CSDL = new DBBanCuaEntities();

        // GET: Admin/Order
        public ActionResult DSDatHang()
        {

            List<DatHang> DSDH = CSDL.DatHangs.ToList(); 
            return View(DSDH);
        }
    }
}