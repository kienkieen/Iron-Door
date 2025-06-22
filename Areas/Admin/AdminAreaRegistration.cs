using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nhom12_Website_Ban_Cua.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "AdminLogin", id = UrlParameter.Optional },
                namespaces: new[] { "Nhom12_Website_Ban_Cua.Areas.Admin.Controllers" }
            );
        }
    }
}