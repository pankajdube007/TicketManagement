//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TicketManagement.Reposistory
{
    using System;
    using System.Collections.Generic;
    
    public partial class Login_history
    {
        public long SLNO { get; set; }
        public Nullable<long> User_Id { get; set; }
        public Nullable<System.DateTime> loginTime { get; set; }
        public Nullable<System.DateTime> logouttime { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> CreateId { get; set; }
        public Nullable<System.DateTime> Createdt { get; set; }
        public Nullable<long> ModifyId { get; set; }
        public Nullable<System.DateTime> Modifydt { get; set; }
        public Nullable<long> DelId { get; set; }
        public Nullable<System.DateTime> Deldt { get; set; }
        public Nullable<long> LogNo { get; set; }
    }
}
