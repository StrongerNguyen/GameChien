//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameChien.Models.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblPlayer_Room
    {
        public System.Guid Id { get; set; }
        public System.Guid PlayerId { get; set; }
        public System.Guid RoomId { get; set; }
        public bool isOwner { get; set; }
        public bool isTeam { get; set; }
        public System.DateTime JoinTime { get; set; }
        public bool isReady { get; set; }
        public Nullable<bool> isVoteWin { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string AttachFile { get; set; }
    
        public virtual tblPlayer tblPlayer { get; set; }
        public virtual tblRoom tblRoom { get; set; }
    }
}
