using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using Dapper;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;

namespace TicketManagement.Concrete
{
    public class MenuCategoryConcrete : IMenuCategory
    {
        //private readonly DatabaseContext _context;
        GoldmedalTicketEntities _context = new GoldmedalTicketEntities();


        //public MenuCategoryConcrete(DatabaseContext context)
        //{
        //    _context = context;
        //}

        public long? AddCategory(MenuCategoryMaster category)
        {
            long? result = -1;

            if (category != null)
            {
                category.Createdt = DateTime.Now;
                _context.MenuCategoryMasters.Add(category);
                _context.SaveChanges();
                result = category.SLNO;
            }
            return result;
        }

        public bool CheckCategoryNameExists(string menuCategoryName, long ? roleId)
        {
            try
            {
                var result = (from category in _context.MenuCategoryMasters.AsNoTracking()
                              where category.MenuCategoryName == menuCategoryName && category.RoleId == roleId
                              select category).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long? DeleteCategory(long? categoryId)
        {
            try
            {
                var category = _context.MenuCategoryMasters.Find(categoryId);       

                if (category != null)
                    category.IsActive = false;
                category.Deldt = DateTime.Now;
                _context.Entry(category).State = EntityState.Modified;
                _context.SaveChanges();
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAllActiveSelectListItemCategory()
        {
            var categoryList = (from cat in _context.MenuCategoryMasters
                                where cat.IsActive == true
                                select new SelectListItem()
                                {
                                    Text = cat.MenuCategoryName,
                                    Value = cat.SLNO.ToString()
                                }).ToList();

            categoryList.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });

            return categoryList;
        }

        public EditCategoriesVM GetCategoryById(long? menuCategoryId)
        {
            try
            {
                var result = (from category in _context.MenuCategoryMasters.AsNoTracking()
                              where category.SLNO == menuCategoryId
                              select new EditCategoriesVM()
                              {
                                  RoleId = category.RoleId,
                                  IsActive = category.IsActive,
                                  MenuCategoryId = category.SLNO,
                                  MenuCategoryName = category.MenuCategoryName

                              }).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectListItem> GetCategorybyRoleId(long? roleId)
        {
            var categoryList = (from cat in _context.MenuCategoryMasters
                                where cat.IsActive == true && cat.RoleId == roleId
                                select new SelectListItem()
                                {
                                    Text = cat.MenuCategoryName,
                                    Value = cat.SLNO.ToString()
                                }).ToList();

            categoryList.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });

            return categoryList;
        }

        public long GetCategoryCount(string menuCategoryName)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (!string.IsNullOrEmpty(menuCategoryName))
                    {
                        var result = (from category in db.MenuCategory
                                      where category.MenuCategoryName == menuCategoryName
                                      select category).Count();
                        return result;
                    }
                    else
                    {
                        var result = (from category in db.MenuCategory
                                      select category).Count();
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<MenuCategoryViewModel> ShowAllMenusCategory(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryableMenuMaster = (from menuCategory in _context.MenuCategoryMasters
                                           join roleMaster in _context.RoleMasters on menuCategory.RoleId equals roleMaster.SLNO
                                           select new MenuCategoryViewModel()
                                           {
                                               Status = menuCategory.IsActive,
                                               MenuCategoryId = menuCategory.SLNO,
                                               MenuCategoryName = menuCategory.MenuCategoryName,
                                               RoleName = roleMaster.RoleName
                                           }
                    );

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryableMenuMaster = queryableMenuMaster.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryableMenuMaster = queryableMenuMaster.Where(m => m.MenuCategoryName.Contains(search) || m.MenuCategoryName.Contains(search));
                }

                return queryableMenuMaster;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public long? UpdateCategory(MenuCategoryMaster category)
        {
            try
            {
                long? result = -1;
                if (category != null)
                {
                    category.Modifydt = DateTime.Now;
                    _context.Entry(category).State = EntityState.Modified;
                    _context.SaveChanges();
                    result = category.SLNO;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MenuCategoryCacheViewModel> ShowCategories(long roleId)
        {
            List<MenuCategoryCacheViewModel> renderCategoriesList;
            var key = $"MenuCategory_Cache_{roleId}";
            if (!CacheHelper.CheckExists(key))
            {
                //using (var db = new DatabaseContext())
                //{
                    var data = (from cat in _context.MenuCategoryMasters
                                orderby cat.SortingOrder
                                where cat.IsActive == true && cat.RoleId == roleId
                                select new MenuCategoryCacheViewModel()
                                {
                                    MenuCategoryName = cat.MenuCategoryName,
                                    MenuCategoryId = cat.SLNO
                                }).ToList();

                    CacheHelper.AddToCacheWithNoExpiration(key, data);
                    return data;
                //}
            }
            else
            {
                renderCategoriesList = (List<MenuCategoryCacheViewModel>)CacheHelper.GetStoreCachebyKey(key);
            }

            return renderCategoriesList;
        }

        public bool UpdateMenuCategoryOrder(List<MenuCategoryStoringOrder> menuStoringOrder)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    foreach (var menu in menuStoringOrder)
                    {
                        var param = new DynamicParameters();
                        param.Add("@MenuCategoryId", menu.MenuCategoryId);
                        param.Add("@RoleId", menu.RoleId);
                        param.Add("@SortingOrder", menu.SortingOrder);
                        con.Execute("Usp_UpdateMenuCategoryOrder", param, transaction, 0, CommandType.StoredProcedure);
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}