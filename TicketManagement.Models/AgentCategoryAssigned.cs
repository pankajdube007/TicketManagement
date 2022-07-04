using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("AgentCategoryAssigned")]
    public class AgentCategoryAssigned
    {
        [Key]
        public long AgentCategoryId { get; set; }
        public long CategoryId { get; set; }
        public long UserId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
