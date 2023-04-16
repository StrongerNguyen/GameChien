using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models.Dto
{
    public class Player_RoomDto
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid RoomId { get; set; }
        public string AccountName { get; set; }
        public string GameAccount { get; set; }
        public string Avatar { get; set; }
        public bool isOwner { get; set; }
        public bool isTeam { get; set; }
        public DateTime JoinTime { get; set; }
        public bool isReady { get; set; }
        public Nullable<bool> isVoteWin { get; set; }
    }
}