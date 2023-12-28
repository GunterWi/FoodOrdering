using FoodOrdering63131236.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FoodOrdering63131236.Areas.Admin.Data
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthor : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            User_63131236 user = (User_63131236)HttpContext.Current.Session["Account"];
            if (user.Account != null && user.Account.VaiTro == Roles)
                return true;
            return false;
        }
    }
}