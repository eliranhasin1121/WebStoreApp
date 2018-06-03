using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //route for select specific type of product.
            routes.MapRoute(
                "ProductsByType",
                "Products/type/{type}",
                new { controller = "Products", action = "ByProductType" });
            //route for the hot product in the store
            routes.MapRoute(
                "HotProudcts",
                "{controller}/{action}",
               new { controller = "Products", action = "getHotProducts" });
            //defulte route.
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
