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
    
    public partial class ReplyAttachmentDetail
    {
        public long SLNO { get; set; }
        public byte[] AttachmentBytes { get; set; }
        public Nullable<long> TicketId { get; set; }
        public Nullable<long> ReplyAttachmentId { get; set; }
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
