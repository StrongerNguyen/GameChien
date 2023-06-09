﻿using GameChien.App_Start;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace GameChien
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            base.AuthenticateRequest += OnAuthenticateRequest;
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        private void OnAuthenticateRequest(object sender, EventArgs eventArgs)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                var decodedTicket = FormsAuthentication.Decrypt(cookie.Value);
                var roles = decodedTicket.UserData.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                var principal = new GenericPrincipal(HttpContext.Current.User.Identity, roles);
                HttpContext.Current.User = principal;
            }
        }
    }
}
