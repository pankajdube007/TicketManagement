using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
//using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;

namespace TicketManagement.Interface
{
    public interface ICategory
    {
        List<SelectListItem> GetAllActiveSelectListItemCategory();
        CategoryMaster GetCategoryById(long? categoryId);
        long? AddCategory(CategoryMaster category);
        long? UpdateCategory(CategoryMaster category);
        int? DeleteCategory(int? categoryId);
        bool CheckCategoryNameExists(string categoryName);
        List<CategoryViewModel> GridGetCategory(string categoryName, int startIndex, int count, string sorting);
        long GetCategoryCount(string categoryName);
        string GetCategoryCodeByCategoryId(int? categoryId);
        List<SelectListItem> GetAllActiveCategoryforListbox();
        long GetCategoryIdsByUserId(long? userId);
        long? AddCategoryConfigration(CategoryConfigration category);
        CategoryConfigration GetCategoryConfigrationDetails(int? categoryConfigrationId);
        long GetCategoryConfigrationCount(string userName);
        List<ShowCategoryConfigration> GridGetCategoryConfigration(string userName, int startIndex,
            int count, string sorting);
        long? UpdateCategoryConfigration(CategoryConfigration category);
        bool CheckDuplicateCategoryConfigration(long adminuserId, long hoduserId, int categoryId);
        int? DeleteCategoryConfigration(int? categoryConfigrationId);
        long GetAdminCategory(long userId);
        long GetHodCategory(long userId);
    }
}
