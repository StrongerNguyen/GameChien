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
    public class MatchHistoryController : Controller
    {
        // GET: History
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        //route: /lich-su-dau
        [HttpGet]
        public ActionResult GetAll(string searchText, int? curPage = 1)
        {
            UserSessionModel userSession = (UserSessionModel)Session[CommonConstants.User_Session];

            PlayerDto player = dbPlayer.GetById(userSession.UserId);
            string query = @"   select r.Id as RoomId, r.Code as RoomCode, r.Name as RoomName, rt.Name as RoomTypeName, r.AmountBet, r.Password, r.StartTime, r.EndTime, pr.JoinTime
		                            , CASE 
			                            WHEN (r.isTeamWin = pr.isTeam) THEN 1
			                            WHEN (r.isTeamWin !=  pr.isTeam) THEN 0 
			                            ELSE NULL
			                            END As isWin
                                    ,ROW_NUMBER() over (order by pr.JoinTime desc) as r
                            from tblPlayer_Room pr
                            join tblRoom r on pr.RoomId = r.Id
                            join tblRoomType rt on r.RoomTypeId = rt.Id ";
            string where = $" WHERE pr.PlayerId = '{player.Id}'";
            if (!string.IsNullOrEmpty(searchText)) where += $" AND (r.Code like '%{searchText}%' or r.Name like '%{searchText}%' or r.AmountBet like '%{searchText}%' ) ";
            string PageSize = ConfigurationManager.AppSettings["pageSize"].ToString();
            query = "SELECT * FROM ( " + query + where + $") AS kq  where r > ({curPage} - 1) * {PageSize} and r <= {curPage}*{PageSize} order by JoinTime desc";

            string queryMaxPage = string.Format(@"  Select CASE 
                                                                When COUNT(*)%{0} = 0 
                                                                    then count(*)/{0}
                                                                else ((count(*)/{0}) + 1) 
                                                            end
                                                    from tblPlayer_Room pr
                                                    join tblRoom r on pr.RoomId = r.Id
                                                    join tblRoomType rt on r.RoomTypeId = rt.Id
                                                    {1}", PageSize, where);
            using (var db = new GameChienEntities())
            {
                List<HistoryDto> data = db.Database.SqlQuery<HistoryDto>(query).ToList();
                ViewBag.maxPage = db.Database.SqlQuery<int>(queryMaxPage).FirstOrDefault();
                ViewBag.curPage = curPage;
                return PartialView("_MatchHistoryTable", data);
            }
        }
    }
}