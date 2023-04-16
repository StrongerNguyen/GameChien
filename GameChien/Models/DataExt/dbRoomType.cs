using GameChien.Models.Data;
using GameChien.Models.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;

namespace GameChien.Models.DataExt
{
    public class dbRoomType
    {
        public static RoomTypeDto GetRoomTypeDtoById(Guid Id)
        {
            using (var db = new GameChienEntities())
            {
                tblRoomType roomType = db.tblRoomTypes.FirstOrDefault(t => t.Id.Equals(Id));
                if (roomType == null) return null;
                else return new RoomTypeDto()
                {
                    Id = roomType.Id,
                    Name = roomType.Name,
                    TotalNumOfPlayer = roomType.TotalNumOfPlayer
                };
            }
        }
        public static List<RoomTypeDto> GetAll()
        {
            using (var db = new GameChienEntities())
            {
                return db.tblRoomTypes.Select(t => new RoomTypeDto()
                {
                    Id = t.Id,
                    Name = t.Name,
                    TotalNumOfPlayer = t.TotalNumOfPlayer
                }).ToList();
            }
        }
    }
}