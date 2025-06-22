using Nhom12_Website_Ban_Cua.Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nhom12_Website_Ban_Cua.Areas.Admin.Controllers
{
    public class StatiscialController : Controller
    {

        DBBanCuaEntities CSDL = new DBBanCuaEntities();

        // GET: Admin/Statiscial
        public ActionResult Index()
        {
            return View();
        }

    }
}