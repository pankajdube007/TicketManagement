using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;
namespace TicketManagement.Interface
{
    public interface IMenu
    {
        List<MenuMaster> GetAllMenu();
        List<SelectListItem> GetAllActiveMenu();
        EditMenuMasterViewModel GetMenuById(long? menuId);
        MenuViewModel GetMenutoDeleteById(long? menuId);
        long? AddMenu(MenuMaster menuMaster);
        long? UpdateMenu(MenuMaster menuMaster);
        void DeleteMenu(long? menuId, long UserId);
        bool CheckMenuNameExists(string menuName);
        List<MenuCategoryOrderingVm> ListofMenubyRoleCategoryId(long roleId);
        IQueryable<MenuViewModel> ShowAllMenus(string sortColumn, string sortColumnDir, string search);
        List<MenuMasterCacheViewModel> GetAllActiveMenu(long roleId);
        List<MenuMaster> GetAllActiveMenuSuperAdmin();
        List<MenuMasterOrderingVm> GetListofMenu(long roleId, long menuCategoryId);
        List<SelectListItem> ListofMenubyRoleIdSelectListItem(long roleId, long menuCategoryId);
        bool UpdateMenuOrder(List<MenuStoringOrder> menuStoringOrder);
        List<SelectListItem> GetAllAssignedMenu();
        
        List<SelectListItem> GetAllAssignedMenuWithRoles();
        bool EditValidationCheck(long? menuId, EditMenuMasterViewModel editMenu);
        bool CheckMenuNameExists(string menuName, long? roleId, long? categoryId);
        List<SelectListItem> ListofMenubyRoleId(RequestMenus requestMenus);
    }
}