using GameChien.Models.Data;
using GameChien.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models.Ext
{
    public class dbRoomFee
    {
        public static RoomFeeDto GetRoomFeeDto()
        {
            using (var db = new GameChienEntities())
            {
                tblRoomFee roomFee = db.tblRoomFees.OrderByDescending(t => t.CreatedTime).FirstOrDefault();
                if (roomFee == null) return null;
                else return new RoomFeeDto()
                {
                    Id = roomFee.Id,
                    FeeValue = roomFee.FeeValue,
                    isPercent = roomFee.isPercent
                };
            }
        }
    }
}