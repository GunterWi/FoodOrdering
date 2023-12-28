using System.Web;
using System.Web.Mvc;

namespace FoodOrdering63131236
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
