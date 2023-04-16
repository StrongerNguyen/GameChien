using GameChien.Hubs;
using GameChien.Models;
using GameChien.Models.CustomAuthorize;
using GameChien.Models.Data;
using GameChien.Models.DataExt;
using GameChien.Models.Dto;
using GameChien.Models.Ext;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace GameChien.Controllers
{
    public class ArenaController : Controller
    {
        // GET: Room
        public ActionResult Index()
        {
            UserSessionModel userSession = (UserSessionModel)Session[CommonConstants.User_Session];
            if (userSession != null)
            {
                Player_RoomDto player_Room = dbPlayer_Room.GetPlayer_Room(userSession.UserId);
                if (player_Room != null)
                {
                    return View("Room");
                }
            }
            ViewBag.RoomTypes = dbRoomType.GetAll();
            return View("Arena");
        }
        [CustomAuthorization]
        public ActionResult PlayerRoom()
        {
            UserSessionModel userSession = (UserSessionModel)Session[CommonConstants.User_Session];

            PlayerDto player = dbPlayer.GetById(userSession.UserId);
            Player_RoomDto myRoom = dbPlayer_Room.GetPlayer_Room(player.Id);
            RoomDto room = dbRoom.GetById(myRoom.RoomId);
            List<Player_RoomDto> players_Room = dbPlayer_Room.GetPlayers_Room(room.Id);

            //ViewBag.myPlayer_Room = myRoom;
            ViewBag.room = room;
            ViewBag.players_room = players_Room;
            return PartialView("_PlayerRoomTable");
        }
        public ActionResult GetAll(Guid? roomTypeId, string searchText, int? curPage = 1)
        {
            string query = @"   SELECT r.Id, r.Code, rt.Name as RoomTypeName, r.Name, r.AmountBet, r.Password, r.NumOfJoined, r.TotalNumOfPlayer, p.AccountName as CreatedByPlayer, r.CreatedTime, r.StartTime, r.EndTime, ROW_NUMBER() over (order by r.CreatedTime desc) as r
                                FROM tblRoom r
                                JOIN tblRoomFee rf on r.RoomFeeId = rf.Id
                                JOIN tblRoomType rt on r.RoomTypeId = rt.Id
                                JOIN tblPlayer p on r.CreatedByPlayerId = p.Id ";
            string where = " WHERE r.isActive = 1 and r.StartTime is Null ";
            if (roomTypeId != null) where += $" AND r.RoomTypeId = '{roomTypeId}' ";
            if (!string.IsNullOrEmpty(searchText)) where += $" AND (r.Code like '%{searchText}%' or r.Name like '%{searchText}%' or r.AmountBet like '%{searchText}%' ) ";
            string PageSize = ConfigurationManager.AppSettings["pageSize"].ToString();
            query = "SELECT * FROM ( " + query + where + $") AS kq  where r > ({curPage} - 1) * {PageSize} and r <= {curPage}*{PageSize} order by CreatedTime desc";

            string queryMaxPage = string.Format(@"  Select CASE 
                                                                When COUNT(*)%{0} = 0 
                                                                    then count(*)/{0}
                                                                else ((count(*)/{0}) + 1) 
                                                            end
                                                    FROM tblRoom r
                                                    {1}", PageSize, where);
            using (var db = new GameChienEntities())
            {
                List<RoomDto> data = db.Database.SqlQuery<RoomDto>(query).ToList();
                ViewBag.maxPage = db.Database.SqlQuery<int>(queryMaxPage).FirstOrDefault();
                ViewBag.curPage = curPage;
                return PartialView("_RoomTable", data);
            }
        }
        public ActionResult GetById(Guid Id)
        {
            return PartialView("_RoomRow");
        }
        public ActionResult Detail(Guid? Id)
        {
            UserSessionModel userSession = (UserSessionModel)Session[CommonConstants.User_Session];

            PlayerDto myPlayer = dbPlayer.GetById(userSession.UserId);
            if (myPlayer == null) return Json(new { success = false, message = "Tài khoản của bạn không tồn tại trên hệ thống, vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);
            if (myPlayer.isBlock) return Json(new { success = false, message = "Tài khoản của bạn đang bị khóa." }, JsonRequestBehavior.AllowGet);
            if (!myPlayer.isVerifiedGameAccount) return Json(new { success = false, message = "Tài khoản game của bạn chưa được xác minh." }, JsonRequestBehavior.AllowGet);
            long minBet = dbConfig.GetMinBet();
            if (myPlayer.Credit < minBet) return Json(new { success = false, message = "Ngân sách của bạn đang không đủ số dư tối thiểu cho một trận đấu, vui lòng nạp thêm." }, JsonRequestBehavior.AllowGet);

            using (var db = new GameChienEntities())
            {
                tblRoom roomFromFB = db.tblRooms.FirstOrDefault(t => t.Id.Equals(Id ?? Guid.Empty));
                RoomDto roomDto = new RoomDto()
                {
                    Id = roomFromFB?.Id ?? Guid.Empty,
                    Name = roomFromFB?.Name ?? "",
                    RoomTypeId = roomFromFB?.RoomTypeId ?? Guid.Empty,
                    AmountBet = minBet
                };
                tblRoomFee roomFee = db.tblRoomFees.OrderByDescending(t => t.CreatedTime).FirstOrDefault();
                ViewBag.RoomTypeList = db.tblRoomTypes.Where(t => t.isActive).Select(t => new RoomTypeDto()
                {
                    Id = t.Id,
                    Name = t.Name,
                    TotalNumOfPlayer = t.TotalNumOfPlayer
                }).OrderBy(t => t.TotalNumOfPlayer).ToList();
                ViewBag.MinBet = minBet;
                ViewBag.RoomFee = new RoomFeeDto()
                {
                    Id = roomFee.Id,
                    FeeValue = roomFee.FeeValue,
                    isPercent = roomFee.isPercent
                };
                return PartialView("_RoomDetail", roomDto);
            }
        }
        [Authorize]
        public JsonResult Update(RoomDto roomDto)
        {
            //Validate data
            if (string.IsNullOrEmpty(roomDto.Name)) return Json(new { success = false, message = "Tên không để trống." }, JsonRequestBehavior.AllowGet);

            using (var db = new GameChienEntities())
            {
                if (roomDto.RoomTypeId == Guid.Empty) return Json(new { success = false, message = $"Vui lòng chọn chế độ chơi." }, JsonRequestBehavior.AllowGet);
                RoomTypeDto roomType = dbRoomType.GetRoomTypeDtoById(roomDto.RoomTypeId);
                if (roomType == null) return Json(new { success = false, message = $"Chế độ chơi không tồn tại" }, JsonRequestBehavior.AllowGet);

                long MinBet = dbConfig.GetMinBet();
                if (roomDto.AmountBet < MinBet) return Json(new { success = false, message = $"Tiền cược tối thiểu {MinBet.ToString("N0")}VND." }, JsonRequestBehavior.AllowGet);

                PlayerDto myPlayer = dbPlayer.GetPlayerDtoByAccountName(User.Identity.Name);
                if (myPlayer == null) return Json(new { success = false, message = $"Bạn không có quyền truy cập tính năng này." }, JsonRequestBehavior.AllowGet);

                //Check xem có đang ở trong phòng nào không
                if (db.tblPlayer_Room.Count(t => t.PlayerId == myPlayer.Id && t.EndTime == null) > 0) return Json(new { success = false, message = $"Bạn đang trong phòng khác." }, JsonRequestBehavior.AllowGet);

                RoomFeeDto roomFee = dbRoomFee.GetRoomFeeDto();
                var dateNow = DateTime.Now.Date;
                string roomCode = dateNow.ToString("yyMMdd") + db.tblRooms.Count(t => DbFunctions.TruncateTime(t.CreatedTime) == dateNow);

                tblRoom room = new tblRoom()
                {
                    Id = Guid.NewGuid(),
                    Code = roomCode,
                    Name = roomDto.Name,
                    RoomTypeId = roomType.Id,
                    AmountBet = roomDto.AmountBet,
                    RoomFeeId = roomFee.Id,
                    Password = roomDto.Password,
                    TotalNumOfPlayer = roomType.TotalNumOfPlayer,
                    CreatedTime = DateTime.Now,
                    CreatedByPlayerId = myPlayer.Id,
                    isActive = true
                };
                db.tblRooms.Add(room);

                tblPlayer_Room player_Room = new tblPlayer_Room()
                {
                    Id = Guid.NewGuid(),
                    PlayerId = myPlayer.Id,
                    RoomId = room.Id,
                    isOwner = true,
                    isTeam = false,
                    JoinTime = DateTime.Now,
                    isReady = true
                };
                db.tblPlayer_Room.Add(player_Room);
                room.NumOfJoined = 1;
                db.SaveChanges();

                new RealtimeHub().UpdateRoom(player_Room, true);
                return Json(new { success = true, message = "Tạo phòng thành công." }, JsonRequestBehavior.AllowGet);
            }
        }

        [MyRoomAuthorize]
        //Action
        public JsonResult Join(Guid roomId)
        {
            //Check kích hoạt
            //Check ngân sách
            //Check kicked
            //check số lượng
            using (var db = new GameChienEntities())
            {
                tblPlayer myPlayer = db.tblPlayers.FirstOrDefault(t => t.AccountName.Equals(User.Identity.Name));
                if (myPlayer == null) return Json(new { success = false, message = "Tài khoản của bạn không tồn tại, vui lòng đăng nhập lại" }, JsonRequestBehavior.AllowGet);
                tblRoom room = db.tblRooms.FirstOrDefault(t => t.Id.Equals(roomId));
                if (myPlayer.Credit < room.AmountBet)
                {
                    return Json(new { success = false, message = "Ngân sách của bạn không đủ, vui lòng nạp thêm." }, JsonRequestBehavior.AllowGet);
                }
                if (!myPlayer.isVerifiedGameAccount)
                {
                    return Json(new { success = false, message = "Tài khoản game của bạn chưa được xác minh." }, JsonRequestBehavior.AllowGet);
                }
                if (room.NumOfJoined >= room.TotalNumOfPlayer)
                {
                    return Json(new { success = false, message = "Đã đủ người tham gia phòng." }, JsonRequestBehavior.AllowGet);
                }
                List<tblPlayer_Room> player_Rooms = db.tblPlayer_Room.Where(t => t.RoomId.Equals(room.Id)).ToList();
                //Check xem room này còn cho phép vào không
                tblPlayer_Room player_Room = new tblPlayer_Room()
                {
                    Id = Guid.NewGuid(),
                    PlayerId = myPlayer.Id,
                    RoomId = room.Id,
                    isOwner = false,
                    isTeam = player_Rooms.Count(t => t.isTeam) < player_Rooms.Count(t => !t.isTeam) ? true : false,
                    JoinTime = DateTime.Now,
                    isReady = false
                };
                db.tblPlayer_Room.Add(player_Room);

                room.NumOfJoined += 1;

                db.SaveChanges();
                new RealtimeHub().UpdateRoom(player_Room, true);
                //Realtime
                return Json(new { success = true, message = "Tham gia phòng thành công." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Leave(Guid roomId)
        {
            if (!User.Identity.IsAuthenticated) return Json(new { success = false, message = "Bạn không có quyền truy cập chức năng này, vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);
            using (var db = new GameChienEntities())
            {
                PlayerDto myPlayer = dbPlayer.GetPlayerDtoByAccountName(User.Identity.Name);
                if (myPlayer == null) return Json(new { success = false, message = "Bạn không có quyền truy cập chức năng này, vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);

                tblPlayer_Room myPlayer_Room = db.tblPlayer_Room.FirstOrDefault(t => t.PlayerId.Equals(myPlayer.Id) && t.RoomId.Equals(roomId));
                if (myPlayer_Room == null) Json(new { success = false, message = "Bạn không có quyền truy cập trận đấu này." }, JsonRequestBehavior.AllowGet);

                tblRoom room = myPlayer_Room.tblRoom;
                if (room.EndTime != null) return Json(new { success = false, message = "Bạn không thể rời phòng vì trận đấu này đã kết thúc." }, JsonRequestBehavior.AllowGet);
                if (room.StartTime != null) return Json(new { success = false, message = "Bạn không thể rời phòng vì trận đấu này đang diễn ra." }, JsonRequestBehavior.AllowGet);

                if (!myPlayer_Room.isOwner && myPlayer_Room.isReady) return Json(new { success = false, message = "Vui lòng bỏ sẵn sàng trước khi thoát phòng" }, JsonRequestBehavior.AllowGet);

                db.tblPlayer_Room.Remove(myPlayer_Room);
                room.NumOfJoined -= 1;
                if (room.NumOfJoined == 0)
                {
                    //xóa phòng nếu hết người
                    room.isActive = false;
                }
                else if (myPlayer_Room.isOwner)
                {
                    //Đổi chủ phòng
                    tblPlayer_Room assignPlayer = db.tblPlayer_Room.Where(t => t.RoomId.Equals(room.Id) && t.PlayerId != myPlayer.Id).OrderBy(t => t.JoinTime).FirstOrDefault();
                    assignPlayer.isOwner = true;
                    assignPlayer.isReady = true;
                }
                db.SaveChanges();
                new RealtimeHub().UpdateRoom(myPlayer_Room, true);
                //Realtime
                ViewBag.RoomTypes = dbRoomType.GetAll();
                return View("Arena");
            }
        }
        public JsonResult Ready(Guid roomId)
        {
            if (!User.Identity.IsAuthenticated) return Json(new { success = false, message = "Bạn không có quyền truy cập chức năng này, vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);
            using (var db = new GameChienEntities())
            {
                PlayerDto myPlayer = dbPlayer.GetPlayerDtoByAccountName(User.Identity.Name);
                if (myPlayer == null) return Json(new { success = false, message = "Bạn không có quyền truy cập chức năng này, vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);

                tblPlayer_Room myPlayer_Room = db.tblPlayer_Room.FirstOrDefault(t => t.RoomId.Equals(roomId) && t.PlayerId.Equals(myPlayer.Id));
                if (myPlayer_Room == null) Json(new { success = false, message = "Bạn không có quyền truy cập trận đấu này." }, JsonRequestBehavior.AllowGet);

                tblRoom room = myPlayer_Room.tblRoom;
                if (room.EndTime != null) return Json(new { success = false, message = "Bạn không thể sẵn sàng vì trận đấu này đã kết thúc." }, JsonRequestBehavior.AllowGet);
                if (room.StartTime != null) return Json(new { success = false, message = "Bạn không thể sẵn sàng vì trận đấu này đang diễn ra." }, JsonRequestBehavior.AllowGet);

                myPlayer_Room.isReady = !myPlayer_Room.isReady;
                db.SaveChanges();

                new RealtimeHub().UpdateRoom(myPlayer_Room, false);
                return Json(new { success = true, message = myPlayer_Room.isReady ? "Đã sẵn sàng." : "Đã bỏ sẵn sàng" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Start(Guid roomId)
        {
            if (!User.Identity.IsAuthenticated) return Json(new { success = false, message = "Bạn không có quyền truy cập chức năng này, vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);

            using (var db = new GameChienEntities())
            {
                PlayerDto myPlayer = dbPlayer.GetPlayerDtoByAccountName(User.Identity.Name);
                if (myPlayer == null) return Json(new { success = false, message = "Bạn không có quyền truy cập chức năng này, vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);

                tblPlayer_Room myPlayer_Room = db.tblPlayer_Room.FirstOrDefault(t => t.RoomId.Equals(roomId) && t.PlayerId.Equals(myPlayer.Id));
                if (myPlayer_Room == null) Json(new { success = false, message = "Bạn không có quyền truy cập trận đấu này." }, JsonRequestBehavior.AllowGet);
                if (!myPlayer_Room.isOwner) return Json(new { success = false, message = "Bạn không có quyền bắt đầu trận đấu này" }, JsonRequestBehavior.AllowGet);


                tblRoom room = myPlayer_Room.tblRoom;
                if (room.EndTime != null) return Json(new { success = false, message = "Trận đấu này đã kết thúc." }, JsonRequestBehavior.AllowGet);
                if (room.StartTime != null) return Json(new { success = false, message = "Trận đấu này đã được bắt đầu trước đó." }, JsonRequestBehavior.AllowGet);

                var player_Rooms = db.tblPlayer_Room.Where(t => t.RoomId == room.Id);
                if (player_Rooms.Count() != room.TotalNumOfPlayer) return Json(new { success = false, message = "Số người tham gia chưa đủ" }, JsonRequestBehavior.AllowGet);
                if (player_Rooms.Count(t => t.isReady) != room.TotalNumOfPlayer) return Json(new { success = false, message = "Chỉ có thể bắt đầu khi tất cả người chơi đã sẵn sàng" }, JsonRequestBehavior.AllowGet);
                room.StartTime = DateTime.Now;
                //Trừ điểm tất cả các user trong room
                foreach (var item in player_Rooms)
                {
                    item.tblPlayer.Credit -= room.AmountBet;
                };
                db.SaveChanges();
                new RealtimeHub().UpdateRoom(myPlayer_Room, false);
                return Json(new { success = true, message = "Trận đấu đã bắt đầu" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult End(Guid roomId)
        {
            if (!User.Identity.IsAuthenticated) return Json(new { success = false, message = "Bạn không có quyền truy cập chức năng này, vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);
            using (var db = new GameChienEntities())
            {
                PlayerDto myPlayer = dbPlayer.GetPlayerDtoByAccountName(User.Identity.Name);
                if (myPlayer == null) return Json(new { success = false, message = "Bạn không có quyền truy cập chức năng này, vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);

                tblPlayer_Room myPlayer_Room = db.tblPlayer_Room.FirstOrDefault(t => t.RoomId.Equals(roomId) && t.PlayerId.Equals(myPlayer.Id));
                if (myPlayer_Room == null) Json(new { success = false, message = "Bạn không có quyền truy cập trận đấu này." }, JsonRequestBehavior.AllowGet);
                tblRoom room = myPlayer_Room.tblRoom;
                if (room.StartTime == null) return Json(new { success = false, message = "Trận đấu này chưa bắt đầu." }, JsonRequestBehavior.AllowGet);

                if (myPlayer_Room.EndTime != null && myPlayer_Room.isVoteWin != null) return Json(new { success = false, message = "Trận đấu này đã kết thúc." }, JsonRequestBehavior.AllowGet);
                //Show Vote
                return PartialView("_Vote", room.Id);
            }
        }
        public ActionResult Vote(Guid roomId, bool isWin)
        {
            if (!User.Identity.IsAuthenticated) return Json(new { success = false, message = "Bạn không có quyền truy cập chức năng này, vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);
            using (var db = new GameChienEntities())
            {
                PlayerDto myPlayer = dbPlayer.GetPlayerDtoByAccountName(User.Identity.Name);
                if (myPlayer == null) return Json(new { success = false, message = "Bạn không có quyền truy cập chức năng này, vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);

                tblPlayer_Room myPlayer_Room = db.tblPlayer_Room.FirstOrDefault(t => t.RoomId.Equals(roomId) && t.PlayerId.Equals(myPlayer.Id));
                if (myPlayer_Room == null) Json(new { success = false, message = "Bạn không có quyền truy cập trận đấu này." }, JsonRequestBehavior.AllowGet);

                tblRoom room = myPlayer_Room.tblRoom;
                if (room.StartTime == null) return Json(new { success = false, message = "Trận đấu này chưa bắt đầu." }, JsonRequestBehavior.AllowGet);
                if (myPlayer_Room.EndTime != null && myPlayer_Room.isVoteWin != null) return Json(new { success = false, message = "Trận đấu này đã kết thúc." }, JsonRequestBehavior.AllowGet);


                myPlayer_Room.EndTime = DateTime.Now;
                myPlayer_Room.isVoteWin = isWin;
                if (room.EndTime == null)
                {
                    room.EndTime = DateTime.Now;
                }
                db.SaveChanges();

                return PartialView("_VoteCompleted");
            }
        }

    }
}