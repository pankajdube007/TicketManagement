﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class ViewSubMenuRoleModel
    {
        public long SaveId { get; set; }
        public long? RoleId { get; set; }
        public string MenuName { get; set; }
        public string RoleName { get; set; }
        public string SubMenuName { get; set; }
        public bool ? Status { get; set; }
    }
}
