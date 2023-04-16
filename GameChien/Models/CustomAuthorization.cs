using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameChien.Models
{
    public class CustomAuthorization: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.Session[CommonConstants.User_Session] == null)
            {
                return false;
            }
            else return true;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                //filterContext.Result = new JsonResult()
                //{
                //    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                //    Data = new
                //    {
                //        success = false,
                //        message = "Bạn không có quyền truy cập"
                //    }
                //};
                filterContext.Result = new PartialViewResult()
                {
                    ViewName = "_UnAuthorization"
                };
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Login"
                }));
            }
        }
    }
}