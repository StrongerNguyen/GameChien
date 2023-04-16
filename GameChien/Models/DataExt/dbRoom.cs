using GameChien.Models.Data;
using GameChien.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models.DataExt
{
    public class dbRoom
    {
        public static RoomDto GetById(Guid Id)
        {
            using (var db = new GameChienEntities())
            {
                tblRoom room = db.tblRooms.FirstOrDefault(t => t.Id == Id);
                if (room == null) return null;
                else return new RoomDto()
                {
                    Id = room.Id,
                    Code = room.Code,
                    Name = room.Name,
                    Password = room.Password,
                    CreatedTime = room.CreatedTime,
                    AmountBet = room.AmountBet,
                    CreatedByPlayer = room.tblPlayer.AccountName,
                    EndTime = room.EndTime,
                    NumOfJoined = room.NumOfJoined,
                    RoomTypeId = room.RoomTypeId,
                    RoomTypeName = room.tblRoomType.Name,
                    StartTime = room.StartTime,
                    TotalNumOfPlayer = room.TotalNumOfPlayer
                };
            }
        }
    }
}