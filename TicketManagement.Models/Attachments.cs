using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("Attachments")]
    public class Attachments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SLNO { get; set; }
        [MaxLength(50)]
        public string AttachmentName { get; set; }
        [MaxLength(50)]
        public string AttachmentType { get; set; }
        public long? TicketId { get; set; }
        public DateTime? Createdt { get; set; }
        public long? UserId { get; set; }
     
    }

}
