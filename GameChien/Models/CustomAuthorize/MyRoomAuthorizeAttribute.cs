using GameChien.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameChien.Models.CustomAuthorize
{
    public class MyRoomAuthorizeAttribute : AuthorizeAttribute
    {
        private GameChienEntities db = new GameChienEntities();
        private readonly string[] allowedroles;
        public MyRoomAuthorizeAttribute(params string[] roles)
        {
            allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.IsAuthenticated) return false;
            else
            {
                //check player
                var roomId = httpContext.Request.Form.GetValues("roomIds")?.FirstOrDefault() ?? null;
                return true;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        success = false,
                        message = "Bạn không có quyền truy cập"
                    }
                };

            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new
                {
                    controller = "Account",
                    action = "Logout"
                }));
            }
        }
    }
}