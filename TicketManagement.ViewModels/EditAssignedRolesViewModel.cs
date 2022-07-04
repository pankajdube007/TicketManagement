using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class EditAssignedRolesViewModel
    {
        public long SLNO { get; set; }
        public long AssignedRoleId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public bool? Status { get; set; }
    }
}
