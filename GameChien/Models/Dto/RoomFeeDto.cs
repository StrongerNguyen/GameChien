using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models.Dto
{
    public class RoomFeeDto
    {
        public Guid Id { get; set; }
        public int FeeValue { get; set; }
        public bool isPercent { get; set; }
    }
}