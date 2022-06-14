using System.Web.Mvc;
using System.Web.Routing;
using WebShop.Models;

namespace WebShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Sales",
                "",
                new { area = "Sales", controller = "HomeSales", action = "Home" }
            );

            routes.MapRoute(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { area = "Sales", controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}
