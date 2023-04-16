using GameChien.Models;
using GameChien.Models.Data;
using GameChien.Models.Dto;
using GameChien.Models.Ext;
using GameChien.Models.FormModel;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Security;

namespace GameChien.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Login(bool reg = false)
        {
            if (Session[CommonConstants.User_Session] != null) return Redirect("/dau-truong");
            ViewBag.Reg = reg;
            return View(new LoginFormModel());
        }
        [HttpPost]
        public ActionResult Login(LoginFormModel loginForm)
        {
            try
            {
                if (!ModelState.IsValid) return View(loginForm);
                ResultModel<UserSessionModel> resultModel = dbPlayer.Login(loginForm.AccountName, loginForm.Password);
                if (!resultModel.Success)
                {
                    ModelState.AddModelError("", resultModel.Message);
                    return View(loginForm);
                }
                else
                {
                    Session.Add(CommonConstants.User_Session, resultModel.Data);
                    return Redirect("/dau-truong");
                }
            }
            catch (Exception ex)
            {
                Logging.Log.Error(ex);
                ModelState.AddModelError("", MessageEndUser.CodeException);
                return View(loginForm);
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            if (Session[CommonConstants.User_Session] != null) return Redirect("/dau-truong");
            return View(new RegisterFormModel());
        }
        [HttpPost]
        public ActionResult Register(RegisterFormModel registerForm)
        {
            try
            {
                if (!ModelState.IsValid) return View(registerForm);
                ResultModel result = dbPlayer.Register(registerForm);
                if (result.Success)
                {
                    return Redirect("/dang-nhap?reg=true");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    return View(registerForm);
                }
            }
            catch (Exception ex)
            {
                Logging.Log.Error(ex);
                ModelState.AddModelError("", MessageEndUser.CodeException);
                return View(registerForm);
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return Redirect("/dang-nhap");
        }
    }
}