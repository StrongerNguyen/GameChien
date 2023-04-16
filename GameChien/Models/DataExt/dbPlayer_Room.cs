using GameChien.Models.Data;
using GameChien.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models.Ext
{
    public class dbPlayer_Room
    {
        public static Player_RoomDto GetPlayer_Room(Guid playerId)
        {
            using (var db = new GameChienEntities())
            {
                tblPlayer_Room player_Room = db.tblPlayer_Room.FirstOrDefault(t => t.EndTime == null && t.PlayerId == playerId);
                if (player_Room == null) return null;
                else
                {
                    return new Player_RoomDto()
                    {
                        Id = player_Room.Id,
                        PlayerId = player_Room.PlayerId,
                        RoomId = player_Room.RoomId,
                        isOwner = player_Room.isOwner,
                        isTeam = player_Room.isTeam,
                        AccountName = player_Room.tblPlayer.AccountName,
                        Avatar = player_Room.tblPlayer.Avatar,
                        GameAccount = player_Room.tblPlayer.GameAccount,
                        isReady = player_Room.isReady,
                        JoinTime = player_Room.JoinTime,
                        isVoteWin = player_Room.isVoteWin
                    };
                }
            }
        }
        public static List<Player_RoomDto> GetPlayers_Room(Guid roomId)
        {
            using (var db = new GameChienEntities())
            {
                List<tblPlayer_Room> player_Rooms = db.tblPlayer_Room.Where(t => t.RoomId == roomId).ToList();
                if (player_Rooms == null) return null;
                else return player_Rooms.Select(t => new Player_RoomDto()
                {
                    Id = t.Id,
                    PlayerId = t.PlayerId,
                    RoomId = t.PlayerId,
                    isOwner = t.isOwner,
                    isTeam = t.isTeam,
                    AccountName = t.tblPlayer.AccountName,
                    Avatar = t.tblPlayer.Avatar,
                    GameAccount = t.tblPlayer.GameAccount,
                    isReady = t.isReady,
                    JoinTime = t.JoinTime,
                    isVoteWin = t.isVoteWin
                }).ToList();
            }
        }
    }
}