using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;


namespace TicketManagement.Interface
{
    public interface ISubMenu 
    {
        IEnumerable<SubMenuMaster> GetAllSubMenu();
        EditSubMenuMaster GetSubMenuById(long? subMenuId);
        long? AddSubMenu(SubMenuMaster subMenuMaster);
        long? UpdateSubMenu(SubMenuMaster subMenuMaster);
        long DeleteSubMenu(long? subMenuId);
        bool CheckSubMenuNameExists(string subMenuName, long menuId);
        bool CheckSubMenuNameExists(string subMenuName, long? menuId, long? roleId, long? categoryId);
        IQueryable<SubMenuMasterViewModel> ShowAllSubMenus(string sortColumn, string sortColumnDir, string search);
        List<SelectListItem> GetAllActiveSubMenu(long menuid);
        List<SubMenuMaster> GetAllActiveSubMenuByMenuId(long menuid);
        List<SubMenuMasterOrderingVm> ListofSubMenubyRoleId(long roleId, long menuid);
        bool UpdateSubMenuOrder(List<SubMenuStoringOrder> submenuStoringOrder);
        List<SelectListItem> GetAllActiveSubMenuWithoutDefault(long menuid);
        bool EditValidationCheck(long? subMenuId, EditSubMenuMaster editsubMenu);
    }
}