using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models.Dto
{
    public class HistoryDto
    {
        public Guid RoomId { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public string RoomTypeName { get; set; }
        public long AmountBet { get; set; }
        public string Password { get; set; }
        public string CreatedByPlayer { get; set; }
        public Nullable<DateTime> StartTime { get; set; }
        public Nullable<DateTime> EndTime { get; set; }
        public Nullable<bool> isWin { get; set; }
    }
}