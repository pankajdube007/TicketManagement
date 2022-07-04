﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("Usermaster")]
    public class Usermaster
    {
        [Key]
        public long SLNO { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public DateTime? Createdt { get; set; } = DateTime.Now;
        public bool? IsActive { get; set; }
        public long? CreateId { get; set; }
        public bool IsFirstLogin { get; set; } = false;
        public DateTime? Modifydt { get; set; }
        public Nullable<bool> IsLogin_Active { get; set; }
    }


    [NotMapped]

    public class DropdownUsermaster
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
    }
}
