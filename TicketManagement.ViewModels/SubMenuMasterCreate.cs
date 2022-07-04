using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Models;

namespace TicketManagement.ViewModels
{
    public class SubMenuMasterCreate
    {
        public long SubMenuId { get; set; }

        [Required(ErrorMessage = "Enter ControllerName")]
        public string ControllerName { get; set; }

        [Required(ErrorMessage = "Enter ActionMethod")]
        public string ActionMethod { get; set; }

        [Required(ErrorMessage = "Enter SubMenuName")]
        public string SubMenuName { get; set; }

       
        public bool ? Status { get; set; }
        public DateTime? CreateDate { get; set; }

        [Required(ErrorMessage = "Choose Status")]
        public bool IsActive { get; set; }
        

        [DisplayName("Menu")]
        [Required(ErrorMessage = "Choose Menu")]
        public long MenuId { get; set; }
        public List<SelectListItem> MenuList { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Select Role")]
        public long? RoleID { get; set; }
        public List<SelectListItem> ListofRoles { get; set; }

        [DisplayName("Menu Category")]
        [Required(ErrorMessage = "Select Menu Category")]
        public long MenuCategoryId { get; set; }
        public List<SelectListItem> ListofMenuCategory { get; set; }
    }

    public class EditSubMenuMaster
    {
        public long SubMenuId { get; set; }

        [Required(ErrorMessage = "Enter ControllerName")]
        public string ControllerName { get; set; }

        [Required(ErrorMessage = "Enter ActionMethod")]
        public string ActionMethod { get; set; }

        [Required(ErrorMessage = "Enter SubMenuName")]
        public string SubMenuName { get; set; }

        [Required(ErrorMessage = "Choose Status")]
        public bool ? Status { get; set; }
        public DateTime? CreateDate { get; set; }

        [DisplayName("Menu")]
        [Required(ErrorMessage = "Choose Menu")]
        public long MenuId { get; set; }
        public List<SelectListItem> MenuList { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Select Role")]
        public long? RoleID { get; set; }
        public List<SelectListItem> ListofRoles { get; set; }

        [DisplayName("Menu Category")]
        [Required(ErrorMessage = "Select Menu Category")]
        public long? MenuCategoryId { get; set; }
        public List<SelectListItem> ListofMenuCategory { get; set; }
    }

    public class RequestDeleteSubMenu
    {
        public long? SubMenuId { get; set; }
    }
}
