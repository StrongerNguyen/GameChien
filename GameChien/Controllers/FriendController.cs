using GameChien.Models.Data;
using GameChien.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameChien.Controllers
{
    [Authorize]
    public class FriendController : Controller
    {
        // GET: Friend
        public ActionResult Index()
        {
            using (var db = new GameChienEntities())
            {
                tblPlayer player = db.tblPlayers.FirstOrDefault(t => t.AccountName == User.Identity.Name);
                List<RelationshipDto> listFriend = db.tblRelationships.Where(t => (t.FromPlayer == player.Id || t.ToPlayer == player.Id) && t.isActive && t.isFriend != null && t.isFriend.Value).Select(t => new RelationshipDto()
                {
                    Id = t.Id,
                    AccountName = t.FromPlayer == player.Id ? t.tblPlayer.AccountName : t.tblPlayer1.AccountName,
                    Status = t.FromPlayer == player.Id ? t.tblPlayer.Status : t.tblPlayer1.Status
                }).ToList();
                return View(listFriend);

            }
        }
    }
}