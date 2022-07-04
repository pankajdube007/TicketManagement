using System;

namespace TicketManagement.ViewModels
{
    public class MenuCategoryViewModel
    {
        public string MenuCategoryName { get; set; }
        public string RoleName { get; set; }
        public bool ? Status { get; set; }
        public long MenuCategoryId { get; set; }
    }

    public class MenuCategoryCacheViewModel
    {
        public long MenuCategoryId { get; set; }
        public string MenuCategoryName { get; set; }
        public long RoleId { get; set; }

    }

    public class RenderCategoriesVM
    {
        public long MenuCategoryId { get; set; }
        public string MenuCategoryName { get; set; }
    }
}