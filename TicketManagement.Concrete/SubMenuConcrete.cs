using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;
using System.Linq.Dynamic;
using System.Web.Mvc;
using Dapper;

namespace TicketManagement.Concrete
{
    public class SubMenuConcrete : ISubMenu
    {
        //private readonly DatabaseContext _context;
        GoldmedalTicketEntities _context = new GoldmedalTicketEntities();

        //public SubMenuConcrete(DatabaseContext context)
        //{
        //    _context = context;
        //}

        public long? AddSubMenu(TicketManagement.Reposistory.SubMenuMaster subMenuMaster)
        {
            try
            {
                long? result = -1;

                if (subMenuMaster != null)
                {
                    subMenuMaster.Createdt = DateTime.Now;
                    _context.SubMenuMasters.Add(subMenuMaster);
                    _context.SaveChanges();
                    result = subMenuMaster.SLNO;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long DeleteSubMenu(long? subMenuId)
        {
            try
            {
                TicketManagement.Reposistory.SubMenuMaster subMenuMaster = _context.SubMenuMasters.Find(subMenuId);
                if (subMenuMaster != null)
                    _context.Entry(subMenuMaster).State = EntityState.Deleted;
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<TicketManagement.Reposistory.SubMenuMaster> GetAllSubMenu()
        {
            try
            {
                return _context.SubMenuMasters.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public EditSubMenuMaster GetSubMenuById(long? subMenuId)
        {
            try
            {
                var result = (from submenu in _context.SubMenuMasters
                              where submenu.SLNO == subMenuId
                              select new EditSubMenuMaster()
                              {
                                  RoleID = submenu.RoleId,
                                  Status = submenu.IsActive,
                                  MenuCategoryId = submenu.CategoryId,
                                  SubMenuName = submenu.SubMenuName,
                                  ControllerName = submenu.ControllerName,
                                  ActionMethod = submenu.ActionMethod,
                                  MenuId = submenu.SLNO,
                                  SubMenuId = submenu.SLNO,

                              }).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long? UpdateSubMenu(TicketManagement.Reposistory.SubMenuMaster subMenuMaster)
        {
            try
            {
                long? result = -1;

                if (subMenuMaster != null)
                {
                    subMenuMaster.Createdt = DateTime.Now;
                    _context.Entry(subMenuMaster).State = EntityState.Modified;
                    _context.SaveChanges();
                    result = subMenuMaster.SLNO;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckSubMenuNameExists(string subMenuName, long menuId)
        {
            try
            {
                var result = (from submenu in _context.SubMenuMasters
                              where submenu.SubMenuName == subMenuName && submenu.MenuId == menuId
                              select submenu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckSubMenuNameExists(string subMenuName, long? menuId, long? roleId, long? categoryId)
        {
            try
            {
                var result = (from subMenu in _context.SubMenuMasters.AsNoTracking()
                              where subMenu.SubMenuName == subMenuName
                                    && subMenu.MenuId == menuId
                                    && subMenu.RoleId == roleId
                                    && subMenu.CategoryId == categoryId
                              select subMenu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<SubMenuMasterViewModel> ShowAllSubMenus(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesSubMenuMasters = (from submenu in _context.SubMenuMasters
                                                join category in _context.MenuCategoryMasters on submenu.CategoryId equals category.SLNO
                                                join roleMaster in _context.RoleMasters on submenu.RoleId equals roleMaster.RoleId
                                                join menuMaster in _context.MenuMasters on submenu.MenuId equals menuMaster.SLNO

                                                select new SubMenuMasterViewModel
                                                {
                                                    SubMenuName = submenu.SubMenuName,
                                                    MenuName = menuMaster.MenuName,
                                                    ActionMethod = submenu.ActionMethod,
                                                    ControllerName = submenu.ControllerName,
                                                    Status = submenu.IsActive,
                                                    SubMenuId = submenu.SLNO,
                                                    RoleName = roleMaster.RoleName,
                                                    MenuCategoryName = category.MenuCategoryName
                                                });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesSubMenuMasters = queryablesSubMenuMasters.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryablesSubMenuMasters = queryablesSubMenuMasters.Where(m => m.SubMenuName.Contains(search) || m.SubMenuName.Contains(search));
                }

                return queryablesSubMenuMasters;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAllActiveSubMenu(long menuid)
        {
            try
            {
                var listofActiveMenu = (from submenu in _context.SubMenuMasters
                                        where submenu.IsActive == true && submenu.MenuId == menuid
                                        select new SelectListItem
                                        {
                                            Value = submenu.SLNO.ToString(),
                                            Text = submenu.SubMenuName
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


        public List<SelectListItem> GetAllActiveSubMenuWithoutDefault(long menuid)
        {
            try
            {
                var listofActiveMenu = (from submenu in _context.SubMenuMasters
                                        where submenu.IsActive == true && submenu.MenuId == menuid
                                        select new SelectListItem
                                        {
                                            Value = submenu.SLNO.ToString(),
                                            Text = submenu.SubMenuName
                                        }).ToList();

                return listofActiveMenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<TicketManagement.Reposistory.SubMenuMaster> GetAllActiveSubMenuByMenuId(long menuid)
        {
            try
            {
                var listofActiveMenu = (from submenu in _context.SubMenuMasters
                                        where submenu.IsActive == true && submenu.MenuId == menuid
                                        select submenu).ToList();
                return listofActiveMenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SubMenuMasterOrderingVm> ListofSubMenubyRoleId(long roleId, long menuid)
        {
            var listofactiveMenus = (from tempsubmenu in _context.SubMenuMasters
                                     where tempsubmenu.IsActive == true && tempsubmenu.RoleId == roleId && tempsubmenu.MenuId == menuid
                                     orderby tempsubmenu.SortingOrder ascending
                                     select new SubMenuMasterOrderingVm
                                     {
                                         SubMenuId = tempsubmenu.SLNO,
                                         SubMenuName = tempsubmenu.SubMenuName
                                     }).ToList();

            return listofactiveMenus;
        }

        public bool UpdateSubMenuOrder(List<SubMenuStoringOrder> submenuStoringOrder)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    foreach (var submenu in submenuStoringOrder)
                    {
                        var param = new DynamicParameters();
                        param.Add("@MenuId", submenu.MenuId);
                        param.Add("@RoleId", submenu.RoleId);
                        param.Add("@SortingOrder", submenu.SortingOrder);
                        param.Add("@SubMenuId", submenu.SubMenuId);
                        con.Execute("Usp_UpdateSubMenuOrder", param, transaction, 0, CommandType.StoredProcedure);
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

        public bool EditValidationCheck(long? subMenuId, EditSubMenuMaster editsubMenu)
        {
            var result = (from submenu in _context.SubMenuMasters.AsNoTracking()
                          where submenu.SLNO == subMenuId
                          select submenu).SingleOrDefault();

            if (result != null && (editsubMenu.MenuId == result.MenuId
                                   && editsubMenu.MenuCategoryId == result.CategoryId
                                   && editsubMenu.RoleID == result.RoleId
                                   && editsubMenu.SubMenuName == result.SubMenuName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
