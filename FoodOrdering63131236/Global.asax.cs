using FoodOrdering63131236.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FoodOrdering63131236
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["DungChung"] = new Trash_63131236();
            Application["SoLuotTruyCap"] = 0;
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            Session["Account"] = new User_63131236();
        }
        protected void Session_End(object sender, EventArgs e)
        {
            Session.Clear();
        }
        void Application_BeginRequest(object sender, EventArgs e)
        {
            Application["SoLuotTruyCap"] = (int)Application["SoLuotTruyCap"] + 1;
        }
    }
}
