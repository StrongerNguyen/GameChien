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
    
    public partial class tblCreditHistory
    {
        public System.Guid Id { get; set; }
        public System.Guid PlayerId { get; set; }
        public long Amount { get; set; }
        public Nullable<System.Guid> DepositId { get; set; }
        public Nullable<System.Guid> WithdrawId { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedTime { get; set; }
    }
}
