using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TicketManagement.ViewModels
{

    public class AddCategoriesVM
    {

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Enter CategoryName")]
        public string MenuCategoryName { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Select Role")]
        public long ? RoleId { get; set; }

  
        public bool ? Status { get; set; }
        [DisplayName("IsActive")]
        [Required(ErrorMessage = "Required IsActive")]
        public bool IsActive { get; set; }

        public List<SelectListItem> ListofRoles { get; set; }
    }

    public class EditCategoriesVM
    {
        public long MenuCategoryId { get; set; }

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Enter CategoryName")]
        public string MenuCategoryName { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Select Role")]
        public long ? RoleId { get; set; }

        public bool ? Status { get; set; }

        [DisplayName("IsActive")]
        [Required(ErrorMessage = "Required IsActive")]
        public bool? IsActive { get; set; }
        public List<SelectListItem> ListofRoles { get; set; }
    }

    public class RequestDeleteCategory
    {
        public long MenuCategoryId { get; set; }
    }

    public class RequestCategory
    {
        public long? RoleID { get; set; }
    }
}