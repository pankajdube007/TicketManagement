using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.Models;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;


namespace TicketManagement.Concrete
{
    public class MenuConcrete : IMenu
    {
        //private readonly DatabaseContext _context;
        GoldmedalTicketEntities _context = new GoldmedalTicketEntities();

        //public MenuConcrete(DatabaseContext context)
        //{
        //    _context = context;
        //}


        public List<TicketManagement.Reposistory.MenuMaster> GetAllMenu()
        {
            try
            {
                return _context.MenuMasters.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectListItem> GetAllActiveMenu()
        {
            try
            {
                var listofActiveMenu = (from menu in _context.MenuMasters
                                        where menu.IsActive == true
                                        select new SelectListItem
                                        {
                                            Value = menu.SLNO.ToString(),
                                            Text = menu.MenuName
                                        }).ToList();


                listofActiveMenu.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "---Select---"
                });

                return listofActiveMenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public EditMenuMasterViewModel GetMenuById(long? menuId)
        {
            try
            {
                var editmenu = (from menu in _context.MenuMasters
                                where menu.SLNO == menuId
                                select new EditMenuMasterViewModel()
                                {
                                    IsActive = menu.IsActive,
                                    ActionMethod = menu.ActionMethod,
                                    MenuName = menu.MenuName,
                                    ControllerName = menu.ControllerName,
                                    MenuId = menu.SLNO,
                                    RoleId = menu.RoleId,
                                    MenuCategoryId = menu.CategoryId
                                }).FirstOrDefault();

                return editmenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MenuViewModel GetMenutoDeleteById(long? menuId)
        {
            try
            {
                var editmenu = (from menu in _context.MenuMasters
                                where menu.SLNO == menuId
                                select new MenuViewModel()
                                {
                                    Status = menu.IsActive,
                                    ActionMethod = menu.ActionMethod,
                                    MenuName = menu.MenuName,
                                    ControllerName = menu.ControllerName
                                }).FirstOrDefault();

                return editmenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long? AddMenu(TicketManagement.Reposistory.MenuMaster menuMaster)
        {
            try
            {
                long? result = -1;

                if (menuMaster != null)
                {
                    _context.MenuMasters.Add(menuMaster);
                    _context.SaveChanges();
                    result = menuMaster.SLNO;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long? UpdateMenu(TicketManagement.Reposistory.MenuMaster menuMaster)
        {
            try
            {
                long? result = -1;

                if (menuMaster != null)
                {
                    using (var db = new DatabaseContext())
                    {
                        menuMaster.Createdt = DateTime.Now;
                        _context.Entry(menuMaster).State = EntityState.Modified;
                        _context.SaveChanges();
                        result = menuMaster.SLNO;
                    }
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteMenu(long? menuId, long UserId)
        {
            try
            {
                TicketManagement.Reposistory.MenuMaster menuMaster = _context.MenuMasters.Find(menuId);
                if (menuMaster != null)
                    menuMaster.IsActive = false;
                menuMaster.Deldt = DateTime.Now;
                menuMaster.DelId = UserId;
                _context.Entry(menuMaster).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckMenuNameExists(string menuName)
        {
            try
            {
                var result = (from menu in _context.MenuMasters
                              where menu.MenuName == menuName
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IQueryable<MenuViewModel> ShowAllMenus(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryableMenuMaster = (from menu in _context.MenuMasters
                                           join category in _context.MenuCategoryMasters on menu.CategoryId equals category.SLNO
                                           join roleMaster in _context.RoleMasters on menu.RoleId equals roleMaster.RoleId
                                           orderby menu.SLNO descending
                                           select new MenuViewModel()
                                           {
                                               Status = menu.IsActive,
                                               ActionMethod = menu.ActionMethod,
                                               MenuName = menu.MenuName,
                                               ControllerName = menu.ControllerName,
                                               MenuId = menu.SLNO,
                                               RoleName = roleMaster.RoleName,
                                               MenuCategoryName = category.MenuCategoryName
                                           }
                    );

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryableMenuMaster = queryableMenuMaster.OrderBy(sortColumn + " " + sortColumnDir);
                }


                if (!string.IsNullOrEmpty(search))
                {
                    queryableMenuMaster = queryableMenuMaster.Where(m => m.MenuName.Contains(search) || m.MenuName.Contains(search));
                }

                return queryableMenuMaster;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MenuMasterCacheViewModel> GetAllActiveMenu(long roleId)
        {
            try
            {
                string keyMainMenu = "MainMenu_Cache_" + roleId;
                var menuList = (List<MenuMasterCacheViewModel>)CacheHelper.GetStoreCachebyKey(keyMainMenu);
                return menuList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<TicketManagement.Reposistory.MenuMaster> GetAllActiveMenuSuperAdmin()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    return con.Query<TicketManagement.Reposistory.MenuMaster>("Usp_GetMenusByRoleID_SuperAdmin", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MenuCategoryOrderingVm> ListofMenubyRoleCategoryId(long roleId)
        {
            var listofactiveMenus = (from tempmenu in _context.MenuMasters
                                     where tempmenu.IsActive == true && tempmenu.RoleId == roleId
                                     orderby tempmenu.SortingOrder ascending
                                     select new MenuCategoryOrderingVm
                                     {
                                         MenuCategoryId = tempmenu.SLNO,
                                         MenuCategoryName = tempmenu.MenuName
                                     }).ToList();

            return listofactiveMenus;
        }

        public List<MenuMasterOrderingVm> GetListofMenu(long roleId, long menuCategoryId)
        {
            var listofactiveMenus = (from tempmenu in _context.MenuMasters
                                     where tempmenu.IsActive == true && tempmenu.RoleId == roleId && tempmenu.CategoryId == menuCategoryId
                                     orderby tempmenu.SortingOrder ascending
                                     select new MenuMasterOrderingVm
                                     {
                                         MenuId = tempmenu.SLNO,
                                         MenuName = tempmenu.MenuName
                                     }).ToList();

            return listofactiveMenus;
        }

        public List<SelectListItem> ListofMenubyRoleId(RequestMenus requestMenus)
        {
            var listofactiveMenus = (from tempmenu in _context.MenuMasters
                                     join menu in _context.MenuMasters on tempmenu.SLNO equals menu.SLNO
                                     where tempmenu.IsActive == true && tempmenu.RoleId == requestMenus.RoleID && tempmenu.CategoryId == requestMenus.CategoryID
                                     orderby tempmenu.SLNO ascending
                                     select new SelectListItem
                                     {
                                         Value = menu.SLNO.ToString(),
                                         Text = menu.MenuName
                                     }).ToList();

            listofactiveMenus.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "---Select---"
            });
            return listofactiveMenus;
        }

        public List<SelectListItem> ListofMenubyRoleIdSelectListItem(long roleId, long menuCategoryId)
        {
            var listofactiveMenus = (from tempmenu in _context.MenuMasters
                                     where tempmenu.IsActive == true && tempmenu.RoleId == roleId && tempmenu.CategoryId == menuCategoryId
                                     orderby tempmenu.SortingOrder ascending
                                     select new SelectListItem
                                     {
                                         Value = tempmenu.SLNO.ToString(),
                                         Text = tempmenu.MenuName
                                     }).ToList();

            listofactiveMenus.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "---Select Main Menu---"
            });

            return listofactiveMenus;
        }



        public bool UpdateMenuOrder(List<MenuStoringOrder> menuStoringOrder)
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
                        param.Add("@MenuId", menu.MenuId);
                        param.Add("@RoleId", menu.RoleId);
                        param.Add("@SortingOrder", menu.SortingOrder);
                        con.Execute("Usp_UpdateMenuOrder", param, transaction, 0, CommandType.StoredProcedure);
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

        public List<SelectListItem> GetAllAssignedMenu()
        {
            try
            {
                var menulist = (from menu in _context.MenuMasters
                                where menu.RoleId != null && menu.IsActive == true
                                join roles in _context.RoleMasters on menu.RoleId equals roles.RoleId
                                select new SelectListItem()
                                {
                                    Text = menu.MenuName + " | " + roles.RoleName,
                                    Value = menu.SLNO.ToString()
                                }).ToList();
                return menulist;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectListItem> GetAllAssignedMenuWithRoles()
        {
            try
            {
                var menulist = (from menu in _context.MenuMasters
                                where menu.RoleId != null && menu.IsActive == true
                                join roles in _context.RoleMasters on menu.RoleId equals roles.RoleId
                                select new SelectListItem()
                                {
                                    Text = menu.MenuName + " | " + roles.RoleName,
                                    Value = menu.SLNO.ToString()
                                }).ToList();


                menulist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "---Select---"
                });

                return menulist;
            }
            catch (Exception)
            {

                throw;
            }
        }

       

        public bool EditValidationCheck(long? menuId, EditMenuMasterViewModel editMenu)
        {
            var result = (from menu in _context.MenuMasters.AsNoTracking()
                          where menu.SLNO == menuId
                          select menu).SingleOrDefault();

            if (result != null && (editMenu.MenuCategoryId == result.CategoryId && editMenu.RoleId == result.RoleId && editMenu.MenuName == result.MenuName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckMenuNameExists(string menuName, long? roleId, long? categoryId)
        {
            try
            {
                var result = (from menu in _context.MenuMasters.AsNoTracking()
                              where menu.MenuName == menuName && menu.RoleId == roleId && menu.CategoryId == categoryId
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
