using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models.Dto
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public string Name { get; set; }
        public long AmountBet { get; set; }
        public string Password { get; set; }
        public int NumOfJoined { get; set; }
        public int TotalNumOfPlayer { get; set; }
        public string CreatedByPlayer { get; set; }
        public DateTime CreatedTime { get; set; }
        public Nullable<DateTime> StartTime { get; set; }
        public Nullable<DateTime> EndTime { get; set; }
        public long r { get; set; }
    }
}