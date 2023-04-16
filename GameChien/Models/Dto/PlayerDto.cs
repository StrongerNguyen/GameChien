using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models.Dto
{
    public class PlayerDto
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string GameAccount { get; set; }
        public string Avatar { get; set; }
        public long Credit { get; set; }
        public int GameLevel { get; set; }
        public int PercentOfLevelUp { get; set; }
        public int Status { get; set; }
        public bool isBlock { get; set; }
        public bool isVerifiedGameAccount { get; set; }
    }
}