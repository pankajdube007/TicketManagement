using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("CategoryMaster")]
    public class Category
    {
        [Key]
        public short CategoryId { get; set; }

        [Required(ErrorMessage = "Enter CategoryName")]
        public string CategoryName { get; set; }
        //[Required(ErrorMessage = "Required Status")]
        public bool Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UserId { get; set; }

        [Required(ErrorMessage = "Enter Category Code")]
        [DisplayName("Category Code")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Code for Category Must be of only 1 words")]
        public string CategoryCode { get; set; }
        public string Code { get; set; }
        public string CategoryDescription { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; } = false;
        public long SLNO { get; set; }
     


    }
}
