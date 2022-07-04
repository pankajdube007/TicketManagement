using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.CommonData;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;

namespace TicketManagement.Controllers
{
    //[AuthorizeSuperAdmin]
  
    public class CategoryController : Controller
    {
        private readonly ICategory _category;
        private readonly IUserMaster _userMaster;
        private readonly IBusinessHours _businessHours;

        GoldmedalTicketEntities db = new GoldmedalTicketEntities(); 



          SessionHandler _sessionHandler = new SessionHandler();
        public CategoryController(ICategory category, IUserMaster userMaster, IBusinessHours businessHours)
        {
            _category = category;
            _userMaster = userMaster;
            _businessHours = businessHours;
        }

        // GET: Category
        public ActionResult Index()
        {
            //var CategoryList = db.CategoryMasters.AsEnumerable().Where(x => x.IsActive == true && x.DelId == null).Select(x => x).ToList();
            //return View(CategoryList);
            return View();
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_category.CheckCategoryNameExists(category.CategoryName))
                    {
                        ModelState.AddModelError("", CommonMessages.CategoryAlreadyExistsMessages);
                        return View(category);
                    }
                    else
                    {
                        CategoryMaster c = new CategoryMaster();
                        c.Createdt = DateTime.Now;
                        c.CreateId = Convert.ToInt64(_sessionHandler.UserId);
                        c.IsActive = category.Status;
                        c.CategoryCode = category.CategoryCode;
                        c.CategoryName = category.CategoryName;
                        c.Description = category.CategoryDescription;
                        _category.AddCategory(c);
                     
                        TempData["CategorySuccessMessages"] = CommonMessages.CategorySuccessMessages;
                        return RedirectToAction("Create", "Category");
                    }
                }

                return View(category);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var editdata = _category.GetCategoryById(id);
                return View(editdata);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(int SLNO, CategoryMaster category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    category.Modifydt = DateTime.Now;
                    category.SLNO = SLNO;
                    //category.UserId = Convert.ToInt64(_sessionHandler.UserId);
                    _category.UpdateCategory(category);
                    TempData["EditCategorySuccessMessages"] = CommonMessages.CategorySuccessMessages;
                    return RedirectToAction("Edit", "Category", new { id = SLNO });

                }

                return View(category);
            }
            catch
            {
                return View();
            }
        }

        public JsonResult CheckCategoryName(string categoryName)
        {
            try
            {
                if (!string.IsNullOrEmpty(categoryName))
                {
                    var result = _category.CheckCategoryNameExists(categoryName);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost] //Gets the todo Lists.  
        public JsonResult GetCategory(string categoryName, int jtStartIndex = 0, int jtPageSize = 0,
            string jtSorting = null)
        {

            try
            {
                var rolesCount = _category.GetCategoryCount(categoryName);
                var categoryList = _category.GridGetCategory(categoryName, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = categoryList, TotalRecordCount = rolesCount });
            }
            catch (Exception Ex)
            {
                return Json(new { Result = "ERROR", Message = Ex.Message  });
            }
        }

        public JsonResult DeleteCategory(int categoryId)
        {
            try
            {
                var result = _category.DeleteCategory(categoryId);
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Result = "ERROR", Message = "Cannot Delete" });
            }
        }

        public ActionResult Assign()
        {
            CategoryConfigrationViewModel categoryConfigrationViewModel = new CategoryConfigrationViewModel()
            {
                BusinessHoursId = 0,
                AgentAdminUserId = 0,
                ListofAdmin = _userMaster.GetListofAgentsAdmin(),
                ListofHod = _userMaster.GetListofHod(),
                ListofBusinessHours = _businessHours.ListofBusinessHours(),
                CategoryId = 0,
                ListofCategory = _category.GetAllActiveSelectListItemCategory(),
                Status = true
            };
            return View(categoryConfigrationViewModel);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign(CategoryConfigrationViewModel ccViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_category.CheckDuplicateCategoryConfigration(ccViewModel.AgentAdminUserId, ccViewModel.HodUserId, ccViewModel.CategoryId))
                {
                    ModelState.AddModelError("", "Already Category is Assigned to User");
                }
                else
                {
                   TicketManagement.Reposistory.CategoryConfigration categoryConfigration = new TicketManagement.Reposistory.CategoryConfigration()
                    {
                        BusinessHoursId = ccViewModel.BusinessHoursId,
                        CategoryId = ccViewModel.CategoryId,
                        IsActive = ccViewModel.Status,
                        AgentAdminUserId = ccViewModel.AgentAdminUserId,
                        HodUserId = ccViewModel.HodUserId,
                        SLNO = 0
                    };

                    var result = _category.AddCategoryConfigration(categoryConfigration);

                    if (result > 0)
                    {
                        TempData["MessageCategoryConfigration"] = "Category Configration Saved Successfully";
                    }
                }
            }

            ccViewModel.ListofAdmin = _userMaster.GetListofAgentsAdmin();
            ccViewModel.ListofBusinessHours = _businessHours.ListofBusinessHours();
            ccViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
            ccViewModel.ListofHod = _userMaster.GetListofHod();
            return View(ccViewModel);
        }

        [HttpGet]
        public ActionResult AssignEdit(int? id)
        {
            var data = _category.GetCategoryConfigrationDetails(id);

            EditCategoryConfigrationViewModel categoryConfigrationViewModel = new EditCategoryConfigrationViewModel();


            // CATEGO = data.SLNO,
                 categoryConfigrationViewModel.SLNO = data.SLNO;
                categoryConfigrationViewModel.BusinessHoursId = data.BusinessHoursId;
            categoryConfigrationViewModel.UserId = data.AgentAdminUserId;
            categoryConfigrationViewModel.CategoryId = data.CategoryId;
            categoryConfigrationViewModel.ListofAdmin = _userMaster.GetListofAgentsAdmin();
            categoryConfigrationViewModel.ListofBusinessHours = _businessHours.ListofBusinessHours();
            categoryConfigrationViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
            categoryConfigrationViewModel.Status = data.IsActive;
            categoryConfigrationViewModel.HodUserId = data.HodUserId;
            categoryConfigrationViewModel.ListofHod = _userMaster.GetListofHod();
                return View(categoryConfigrationViewModel);
        }

        [HttpPost]
        public ActionResult AssignEdit(EditCategoryConfigrationViewModel editCategory)
        {
           TicketManagement.Reposistory.CategoryConfigration categoryConfigration = new TicketManagement.Reposistory.CategoryConfigration()
            {
                BusinessHoursId = editCategory.BusinessHoursId,
                AgentAdminUserId = editCategory.UserId,
                Modifydt = DateTime.Now,
                CategoryId = editCategory.CategoryId,
                SLNO = editCategory.CategoryConfigrationId,
                IsActive = editCategory.Status,
                HodUserId = editCategory.HodUserId
            };

            _category.UpdateCategoryConfigration(categoryConfigration);
            editCategory.ListofAdmin = _userMaster.GetListofAgentsAdmin();
            editCategory.ListofBusinessHours = _businessHours.ListofBusinessHours();
            editCategory.ListofCategory = _category.GetAllActiveSelectListItemCategory();
            editCategory.ListofHod = _userMaster.GetListofHod();
            TempData["MessageCategoryConfigration"] = "Category Configration Updated Successfully";

            return View(editCategory);
        }

        public ActionResult AllCategoryConfigration()
        {
            return View();
        }

        [HttpPost] //Gets the todo Lists.  
        public JsonResult GetAssignCategoryConfigration(string userName, int jtStartIndex = 0, int jtPageSize = 0,
            string jtSorting = null)
        {

            try
            {
                var categoryCount = _category.GetCategoryConfigrationCount(userName);
                var getCategoryList = _category.GridGetCategoryConfigration(userName, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = getCategoryList, TotalRecordCount = categoryCount });
            }
            catch (Exception)
            {
                return Json(new { Result = "ERROR", Message = "Something Went Wrong!" });
            }
        }

        public JsonResult DeleteCategoryConfigration(int categoryConfigrationId)
        {
            try
            {
                var result = _category.DeleteCategoryConfigration(categoryConfigrationId);
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Result = "ERROR", Message = "Cannot Delete" });
            }
        }
    }
}
