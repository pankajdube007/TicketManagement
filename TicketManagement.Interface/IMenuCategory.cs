using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;

namespace TicketManagement.Interface
{
    public interface IMenuCategory
    {
        List<SelectListItem> GetAllActiveSelectListItemCategory();
        EditCategoriesVM GetCategoryById(long? menuCategoryId);
        long? AddCategory(MenuCategoryMaster category);
        long? UpdateCategory(MenuCategoryMaster category);
        long? DeleteCategory(long? categoryId);
        bool CheckCategoryNameExists(string menuCategoryName, long ? roleId);
        long GetCategoryCount(string menuCategoryName);
        IQueryable<MenuCategoryViewModel> ShowAllMenusCategory(string sortColumn, string sortColumnDir, string search);
        List<SelectListItem> GetCategorybyRoleId(long? roleId);
        List<MenuCategoryCacheViewModel> ShowCategories(long roleId);
        bool UpdateMenuCategoryOrder(List<MenuCategoryStoringOrder> menuStoringOrder);
    }
}