using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class MenuCategoryOrderingVm
    {
        public long MenuCategoryId { get; set; }
        public string MenuCategoryName { get; set; }
    }

    public class MenuCategoryStoringOrder
    {
        public long MenuCategoryId { get; set; }
        public long RoleId { get; set; }
        public long SortingOrder { get; set; }
    }

    public class MenuMasterOrderingVm
    {
        public long MenuId { get; set; }
        public string MenuName { get; set; }
    }

    public class RequestMenuMasterOrderVm
    {
        public long[] SelectedOrder { get; set; }
        public long RoleId { get; set; }
    }

    public class MenuStoringOrder
    {
        public long MenuId { get; set; }
        public long RoleId { get; set; }
        public long SortingOrder { get; set; }
    }
}
