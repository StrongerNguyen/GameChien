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
    
    public partial class tblMatching
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> PlayerId { get; set; }
        public Nullable<System.Guid> RoomTypeId { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
    
        public virtual tblPlayer tblPlayer { get; set; }
        public virtual tblRoomType tblRoomType { get; set; }
    }
}
