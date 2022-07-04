using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.CommonData;
using TicketManagement.Filters;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Helpers;
using TicketManagement.Reposistory;

namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdmin]
    public class MenuCategoryController : Controller
    {
        private readonly IRole _role;
        private readonly IMenuCategory _menuCategory;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        public MenuCategoryController(IRole role, IMenuCategory menuCategory)
        {
            _role = role;
            _menuCategory = menuCategory;
        }
        // GET: MenuCategory
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create()
        {
            AddCategoriesVM addCategoriesVm = new AddCategoriesVM()
            {
                ListofRoles = _role.GetAllActiveRoles()
            };
            return View(addCategoriesVm);

         
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddCategoriesVM category)
        {
           
            try
            {
                if (ModelState.IsValid)
                {
                    if (_menuCategory.CheckCategoryNameExists(category.MenuCategoryName, category.RoleId))
                    {
                        ModelState.AddModelError("", CommonMessages.CategoryAlreadyExistsMessages);
                        category.ListofRoles = _role.GetAllActiveRoles();
                        return View(category);
                    }
                    else
                    {
                        MenuCategoryMaster categories = new MenuCategoryMaster()
                        {
                            RoleId = category.RoleId,
                            IsActive = category.IsActive,
                            Createdt = DateTime.Now,
                            CreateId = Convert.ToInt64(_sessionHandler.UserId),
                            MenuCategoryName = category.MenuCategoryName,
                            LogNo = Convert.ToInt64(Response.Cookies["logno"].Value)

                        };

                        _menuCategory.AddCategory(categories);
                        TempData["CategorySuccessMessages"] = CommonMessages.CategorySuccessMessages;
                        return RedirectToAction("Create", "MenuCategory");
                    }
                }
                category.ListofRoles = _role.GetAllActiveRoles();
                return View(category);
            }
            catch
            {
                throw;
            }
        }

        public ActionResult Edit(int id)
        {

            try
            {
                var editdata = _menuCategory.GetCategoryById(id);
                editdata.ListofRoles = _role.GetAllActiveRoles();
                return View(editdata);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCategoriesVM category)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    var editdata = _menuCategory.GetCategoryById(category.MenuCategoryId);

                    if (editdata.RoleId == category.RoleId && editdata.MenuCategoryName == category.MenuCategoryName)
                    {
                        MenuCategoryMaster categories = new MenuCategoryMaster()
                        {
                            RoleId = category.RoleId,
                            MenuCategoryName = category.MenuCategoryName,
                            IsActive = category.IsActive,
                            SLNO = category.MenuCategoryId,
                            Modifydt = DateTime.Now,
                            ModifyId = Convert.ToInt64(_sessionHandler.UserId),
                            LogNo = Convert.ToInt64(Response.Cookies["logno"].Value)


                        };

                        _menuCategory.UpdateCategory(categories);
                        TempData["CategoryUpdateMessages"] = CommonMessages.CategorySuccessMessages;
                    }
                    else if (_menuCategory.CheckCategoryNameExists(category.MenuCategoryName, category.RoleId))
                    {
                        ModelState.AddModelError("", CommonMessages.CategoryAlreadyExistsMessages);
                        category.ListofRoles = _role.GetAllActiveRoles();
                        return View(category);
                    }
                    else
                    {
                        MenuCategoryMaster categories = new MenuCategoryMaster()
                        {
                            RoleId = category.RoleId,
                            MenuCategoryName = category.MenuCategoryName,
                            IsActive = category.IsActive,
                            SLNO = category.MenuCategoryId,
                            Modifydt = DateTime.Now,
                            ModifyId = Convert.ToInt64(_sessionHandler.UserId),
                            LogNo = Convert.ToInt64(Response.Cookies["logno"].Value)

                        };


                        _menuCategory.UpdateCategory(categories);
                        TempData["CategoryUpdateMessages"] = CommonMessages.CategorySuccessMessages;
                    }
                }

                category.ListofRoles = _role.GetAllActiveRoles();
                return View(category);
            }
            catch
            {
                throw;
            }
        }

        public ActionResult LoadallMenusCategory()
        {

            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                var rolesData = _menuCategory.ShowAllMenusCategory(sortColumn, sortColumnDir, searchValue);
                recordsTotal = rolesData.Count();
                var data = rolesData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult DeleteCategory(RequestDeleteCategory requestCategory)
        {

            try
            {
                var result = _menuCategory.DeleteCategory(requestCategory.MenuCategoryId);
                return Json(new { Result = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Cannot Delete" });
            }
        }
    }
}