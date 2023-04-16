using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameChien
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "dang-nhap",
                url: "dang-nhap",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "dang-ky",
                url: "dang-ky",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "dang-xuat",
                url: "dang-xuat",
                defaults: new { controller = "Account", action = "Logout", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "dau-truong",
               url: "dau-truong",
               defaults: new { controller = "Arena", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "room",
               url: "room",
               defaults: new { controller = "Room", action = "Index" }
            );
            routes.MapRoute(
               name: "xep-hang",
               url: "xep-hang",
               defaults: new { controller = "Rating", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "ban-be",
               url: "ban-be",
               defaults: new { controller = "Friend", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "lich-su-dau",
               url: "lich-su-dau",
               defaults: new { controller = "MatchHistory", action = "Index" }
            );
            routes.MapRoute(
               name: "gethistories",
               url: "gethistories",
               defaults: new { controller = "MatchHistory", action = "GetAll" }
            );
            routes.MapRoute(
               name: "khuyen-mai",
               url: "khuyen-mai",
               defaults: new { controller = "Promotion", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "nap-diem",
               url: "nap-diem",
               defaults: new { controller = "Deposit", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "rut-tien",
               url: "rut-tien",
               defaults: new { controller = "Withdraw", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "sansang",
               url: "san-sang",
               defaults: new { controller = "Arena", action = "Ready", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional }
            );
        }
    }
}
