using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;

namespace TicketManagement.Interface
{
    public interface ICategoryMaster
    {
        List<SelectListItem> GetAllActiveSelectListItemCategory();
        CategoryMaster GetCategoryById(long? categoryId);
        long? AddCategory(CategoryMaster category);
        long? UpdateCategory(CategoryMaster category);
        int? DeleteCategory(int? categoryId);
        bool CheckCategoryNameExists(string categoryName);
        List<CategoryViewModel> GridGetCategory(string categoryName, int startIndex, int count, string sorting);
        int GetCategoryCount(string categoryName);
        string GetCategoryCodeByCategoryId(int? categoryId);
        List<SelectListItem> GetAllActiveCategoryforListbox();
        short GetCategoryIdsByUserId(long? userId);
        int? AddCategoryConfigration(TicketManagement.Reposistory.CategoryConfigration category);
        TicketManagement.Reposistory.CategoryConfigration GetCategoryConfigrationDetails(int? categoryConfigrationId);
        int GetCategoryConfigrationCount(string userName);
        List<ShowCategoryConfigration> GridGetCategoryConfigration(string userName, int startIndex,
            int count, string sorting);
        int? UpdateCategoryConfigration(TicketManagement.Reposistory.CategoryConfigration category);
        bool CheckDuplicateCategoryConfigration(long adminuserId, long hoduserId, int categoryId);
        int? DeleteCategoryConfigration(int? categoryConfigrationId);
        int GetAdminCategory(long userId);
        int GetHodCategory(long userId);
    }
}