namespace TicketManagement.ViewModels
{
    public class SubMenuMasterOrderingVm
    {
        public long SubMenuId { get; set; }
        public string SubMenuName { get; set; }
    }

  
    public class RequestSubMenuMasterOrderVm
    {
        public long[] SelectedOrder { get; set; }
        public long RoleId { get; set; }
        public long MenuId { get; set; }
    }

    public class RequestMenu
    {
        public long RoleId { get; set; }
        public long MenuCategoryId { get; set; }
    }

    public class RequestSubMenu
    {
        public long RoleId { get; set; }
        public long MenuId { get; set; }
    }

    public class SubMenuStoringOrder
    {
        public long SubMenuId { get; set; }
        public long MenuId { get; set; }
        public long RoleId { get; set; }
        public long SortingOrder { get; set; }
    }

    public class RequestMenuCategoryOrderVm
    {
        public long[] SelectedOrder { get; set; }
        public long RoleId { get; set; }
    }

}