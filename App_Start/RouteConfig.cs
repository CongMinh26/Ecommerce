using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ecommerce
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "khachhang",
               url: "khach-hang",
               defaults: new { controller = "KhachHang", action = "Index", id = UrlParameter.Optional }
           );
            // trang xem chi tiết
           // routes.MapRoute(
           //    name: "XemChiTiet",
           //    url: "{bidanh}-{id}",
           //    defaults: new { controller = "SanPham", action = "XemChiTiet", id = UrlParameter.Optional }
           //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
