using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminCenter.App_Start
{
    public class AdminCenterAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AdminCenter";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("AdminCenter_default", "AdminCenter/{controller}/{action}/{id}", new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            }, new string[]
            {
                "AdminCenter.Controllers"
            });
        }
    }
}