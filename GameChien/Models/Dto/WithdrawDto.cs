using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models.Dto
{
    public class WithdrawDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public long Amount { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}