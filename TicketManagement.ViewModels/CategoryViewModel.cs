using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class CategoryViewModel
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool? Status { get; set; }
        public string Code { get; set; }
    }
}
