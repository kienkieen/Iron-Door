using Nhom12_Website_Ban_Cua.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nhom12_Website_Ban_Cua.Areas.Admin.Controllers
{
    public class RequestForSupportController : Controller
    {
        DBBanCuaEntities CSDL = new DBBanCuaEntities();

        // GET: Admin/RequestForSupport
        public ActionResult DSYeuCau()
        {
            List<YeuCauHoTro> YCHT = CSDL.YeuCauHoTroes.ToList();
            return View(YCHT);
        }
    }
}