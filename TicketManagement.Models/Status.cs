using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("Status")]
    public class Status
    {
        [Key]
        public short StatusId { get; set; }
        public string StatusText { get; set; }
        public bool IsInternalStatus { get; set; }
        public long  SlNO { get; set; }
        public bool IsActive { get; set; }
        public long CreateId { get; set; }
        public DateTime Creatdet { get; set; }
        public long ModifyId { get; set; }
        public DateTime Modifydt { get; set; }
        public DateTime Deldt { get; set; }
        public long DeltId { get; set; }
        public long LogNo { get; set; }

    }
}
