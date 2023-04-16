using GameChien.Models;
using GameChien.Models.Data;
using GameChien.Models.Dto;
using GameChien.Models.Ext;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameChien.Controllers
{
    [CustomAuthorization]
    public class WithdrawController : Controller
    {
        // GET: CashOut
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetAll(string searchText, int? curPage = 1)
        {
            UserSessionModel userSession = (UserSessionModel)Session[CommonConstants.User_Session];

            PlayerDto player = dbPlayer.GetById(userSession.UserId);
            string query = @"   select w.Id, w.CreatedTime, w.Amount, w.Status, ROW_NUMBER() over (order by w.CreatedTime desc) as r
                                from tblWithdraw w
                                join tblBankAccountByPlayer bp on w.BankAccountByPlayerId = bp.Id ";
            string where = $" WHERE bp.PlayerId = '{player.Id}'";
            if (!string.IsNullOrEmpty(searchText)) where += $" AND (Amount like '%{searchText}%') ";
            string PageSize = ConfigurationManager.AppSettings["pageSize"].ToString();
            query = "SELECT * FROM ( " + query + where + $") AS kq  where r > ({curPage} - 1) * {PageSize} and r <= {curPage}*{PageSize} order by CreatedTime desc";

            string queryMaxPage = string.Format(@"  Select CASE 
                                                                When COUNT(*)%{0} = 0 
                                                                    then count(*)/{0}
                                                                else ((count(*)/{0}) + 1) 
                                                            end
                                                    from tblWithdraw w
                                                    join tblBankAccountByPlayer bp on w.BankAccountByPlayerId = bp.Id 
                                                    {1}", PageSize, where);
            using (var db = new GameChienEntities())
            {
                List<WithdrawDto> data = db.Database.SqlQuery<WithdrawDto>(query).ToList();
                ViewBag.maxPage = db.Database.SqlQuery<int>(queryMaxPage).FirstOrDefault();
                ViewBag.curPage = curPage;
                return PartialView("_WithdrawTable", data);
            }
        }
        public ActionResult Detail(Guid? Id, long Amount)
        {
            ViewBag.Amount = Amount.ToString("N0");
            return PartialView("_WithdrawDetail");
        }
    }
}