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
    
    public partial class tblTransaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblTransaction()
        {
            this.tblDeposits = new HashSet<tblDeposit>();
        }
    
        public System.Guid Id { get; set; }
        public string GetBy { get; set; }
        public string Device { get; set; }
        public string BankName { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }
        public string CD { get; set; }
        public long Amount { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.Guid> LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDeposit> tblDeposits { get; set; }
        public virtual tblUser tblUser { get; set; }
    }
}
