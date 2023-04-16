using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models.Dto
{
    public class RoomTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TotalNumOfPlayer { get; set; }
    }
}