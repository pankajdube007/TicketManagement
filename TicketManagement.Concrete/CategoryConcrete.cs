using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;
namespace TicketManagement.Concrete
{
    public class CategoryConcrete : ICategory
    {
        private readonly DatabaseContext _context;
        GoldmedalTicketEntities db = new GoldmedalTicketEntities();
        public CategoryConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public long? AddCategory(CategoryMaster category)
        {
            try
            {
                long? result = -1;

                if (category != null)
                {
                    category.Createdt = DateTime.Now;
                   db.CategoryMasters.Add(category);
                   db.SaveChanges();
                    result = category.SLNO;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckCategoryNameExists(string categoryName)
        {
            try
            {
                var result = (from c in db.CategoryMasters
                              where c.CategoryName == categoryName
                              select c).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? DeleteCategory(int? categoryId)
        {
            try
            {
                var category = db.CategoryMasters.Find(categoryId);
                if (category != null)
                    category.IsActive = false;
                 category.Deldt = DateTime.Now;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAllActiveSelectListItemCategory()
        {
            try
            {
                List<SelectListItem> categoryList;
                string key = "Category_Cache";

                if (!CacheHelper.CheckExists(key))
                {
                    categoryList = (from cat in db.CategoryMasters
                                    where cat.IsActive == true
                                    select new SelectListItem()
                                    {
                                        Text = cat.CategoryName,
                                        Value = cat.SLNO.ToString()
                                    }).ToList();

                    categoryList.Insert(0, new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    });

                    CacheHelper.AddToCacheWithNoExpiration(key, categoryList);
                }
                else
                {
                    categoryList = (List<SelectListItem>)CacheHelper.GetStoreCachebyKey(key);
                }

                return categoryList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAllActiveCategoryforListbox()
        {
            try
            {
                List<SelectListItem> categoryList;
                string key = "Category_CacheListbox";

                if (!CacheHelper.CheckExists(key))
                {
                    categoryList = (from cat in _context.Category
                                    where cat.Status == true
                                    select new SelectListItem()
                                    {
                                        Text = cat.CategoryName,
                                        Value = cat.CategoryId.ToString()
                                    }).ToList();


                    CacheHelper.AddToCacheWithNoExpiration(key, categoryList);
                }
                else
                {
                    categoryList = (List<SelectListItem>)CacheHelper.GetStoreCachebyKey(key);
                }

                return categoryList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CategoryMaster GetCategoryById(long? categoryId)
        {
            try
            {
                var result = (from category in db.CategoryMasters
                              where category.SLNO == categoryId
                              select category).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long? UpdateCategory(CategoryMaster category)
        {
            try
            {
                long? result = -1;
                if (category != null)
                {
                    CategoryMaster c = db.CategoryMasters.Single(x => x.SLNO == category.SLNO);             
                    c.Modifydt = DateTime.Now;
                    c.SLNO = category.SLNO;
                    c.IsActive = category.IsActive;
                    c.CategoryName = category.CategoryName;
                    c.Description = category.Description;
                    c.CategoryCode = category.CategoryCode;
                    //db.Entry(c).State = EntityState.Modified;
                    db.SaveChanges();
                    result = c.SLNO;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CategoryViewModel> GridGetCategory(string categoryName, int startIndex, int count, string sorting)
        {
            try
            {

                //using (var db = new DatabaseContext())
                //{
                    var query = from category in db.CategoryMasters
                                select new CategoryViewModel()
                                {
                                    CategoryName = category.CategoryName,
                                    Status = category.IsActive,                                   
                                    CategoryId =category.SLNO,
                                    Code = category.CategoryCode
                                };

                    //Search
                    if (!string.IsNullOrEmpty(categoryName))
                    {
                        query = query.Where(p => p.CategoryName == categoryName);
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("CategoryId ASC"))
                    {
                        query = query.OrderBy(p => p.CategoryId);
                    }
                    else if (sorting.Equals("CategoryId DESC"))
                    {
                        query = query.OrderByDescending(p => p.CategoryId);
                    }
                    else if (sorting.Equals("CategoryName ASC"))
                    {
                        query = query.OrderBy(p => p.CategoryName);
                    }
                    else if (sorting.Equals("CategoryName DESC"))
                    {
                        query = query.OrderByDescending(p => p.CategoryName);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.CategoryId); //Default!
                    }

                    return count > 0
                        ? query.Skip(startIndex).Take(count).ToList()  //Paging
                        : query.ToList(); //No paging
               // }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long GetCategoryCount(string categoryName)
        {
            try
            {
                //using (var db = new DatabaseContext())
                //{
                    if (!string.IsNullOrEmpty(categoryName))
                    {
                        var result = (from category in db.CategoryMasters
                                      where category.CategoryName == categoryName
                                      select category).Count();
                        return result;
                    }
                    else
                    {
                        var result = (from category in db.CategoryMasters
                                      select category).Count();
                        return result;
                    }
               // }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetCategoryCodeByCategoryId(int? categoryId)
        {
            try
            {
                var result = (from category in db.CategoryMasters
                              where category.SLNO == categoryId
                              select category.CategoryCode).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long GetCategoryIdsByUserId(long? userId)
        {
            try
            {
                using (var db = new DatabaseContext())
                {

                    var result = (from category in db.AgentCategoryAssigned
                                  where category.UserId == userId
                                  select category.CategoryId).FirstOrDefault();
                    return result;

                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw;
            }
        }

        public long? AddCategoryConfigration(TicketManagement.Reposistory.CategoryConfigration category)
        {
            try
            {
                long? result = -1;

                if (category != null)
                {
                    category.Createdt = DateTime.Now;
                    db.CategoryConfigrations.Add(category);
                    db.SaveChanges();
                    result = category.SLNO;
                }
                return result;
            }
            catch (Exception Ex)
            {
                var msg = Ex.Message;
                throw;
            }
        }

        public TicketManagement.Reposistory.CategoryConfigration GetCategoryConfigrationDetails(int? categoryConfigrationId)
        {
            var data = (from cag in db.CategoryConfigrations
                        where cag.SLNO == categoryConfigrationId
                        select cag).SingleOrDefault();

            return data;
        }

        public List<ShowCategoryConfigration> GridGetCategoryConfigration(string userName, int startIndex, int count, string sorting)
        {
            try
            {

                using (var db = new DatabaseContext())
                {
                    var query = from categoryconfig in db.CategoryConfigration
                                join category in db.Category on categoryconfig.CategoryId equals category.SLNO
                                join businessHour in db.BusinessHours on categoryconfig.BusinessHoursId equals businessHour.SLNO
                                join usermaster in db.Usermasters on categoryconfig.AgentAdminUserId equals usermaster.SLNO
                                join hodUsermaster in db.Usermasters on categoryconfig.HodUserId equals hodUsermaster.SLNO
                                select new ShowCategoryConfigration()
                                {
                                    CategoryName = category.CategoryName,
                                    IsActive = category.IsActive,
                                    Name = businessHour.Name,
                                    CategoryConfigrationId = categoryconfig.SLNO,
                                    UserName = usermaster.FirstName+" "+ usermaster.LastName,
                                    HodName = hodUsermaster.FirstName + " " + hodUsermaster.LastName,
                                };


                    if (!string.IsNullOrEmpty(userName))
                    {
                        query = query.Where(p => p.UserName == userName);
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("CategoryConfigrationId ASC"))
                    {
                        query = query.OrderBy(p => p.CategoryConfigrationId);
                    }
                    else if (sorting.Equals("CategoryConfigrationId DESC"))
                    {
                        query = query.OrderByDescending(p => p.CategoryConfigrationId);
                    }
                    else if (sorting.Equals("CategoryName ASC"))
                    {
                        query = query.OrderBy(p => p.CategoryName);
                    }
                    else if (sorting.Equals("CategoryName DESC"))
                    {
                        query = query.OrderByDescending(p => p.CategoryName);
                    }
                    else if (sorting.Equals("UserName ASC"))
                    {
                        query = query.OrderBy(p => p.UserName);
                    }
                    else if (sorting.Equals("UserName DESC"))
                    {
                        query = query.OrderByDescending(p => p.UserName);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.CategoryConfigrationId); //Default!
                    }

                    return count > 0
                        ? query.Skip(startIndex).Take(count).ToList()  //Paging
                        : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long GetCategoryConfigrationCount(string userName)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (!string.IsNullOrEmpty(userName))
                    {
                        var result = (from categoryconfig in db.CategoryConfigration
                                      join usermaster in db.Usermasters on categoryconfig.AgentAdminUserId equals usermaster.SLNO
                                      where usermaster.UserName == userName && categoryconfig.Status == true
                                      select categoryconfig).Count();
                        return result;
                    }
                    else
                    {
                        var result = (from categoryconfig in db.CategoryConfigration
                                      select categoryconfig).Count();
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long? UpdateCategoryConfigration(TicketManagement.Reposistory.CategoryConfigration category)
        {
            try
            {
                long? result = -1;
                if (category != null)
                {
                    category.Createdt = DateTime.Now;
                   db. Entry(category).State = EntityState.Modified;
                    db .SaveChanges();
                    result = category.CategoryId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckDuplicateCategoryConfigration(long adminuserId, long hoduserId, int categoryId)
        {
            using (var db = new DatabaseContext())
            {
                var result = (from categoryconfig in db.CategoryConfigration
                              where categoryconfig.AgentAdminUserId == adminuserId && categoryconfig.AgentAdminUserId == hoduserId && categoryconfig.CategoryId == categoryId
                              select categoryconfig).Count();
                return result > 0;
            }
        }

        public int? DeleteCategoryConfigration(int? categoryConfigrationId)
        {
            try
            {
                var categoryConfigration = _context.CategoryConfigration.Find(categoryConfigrationId);
                if (categoryConfigration != null)
                    _context.CategoryConfigration.Remove(categoryConfigration);
                _context.SaveChanges();
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long GetAdminCategory(long userId)
        {
            try
            {
                var result = (from categoryConfigration in _context.CategoryConfigration
                              where categoryConfigration.AgentAdminUserId == userId
                              select categoryConfigration.CategoryId).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long GetHodCategory(long userId)
        {
            try
            {
                var result = (from categoryConfigration in _context.CategoryConfigration
                    where categoryConfigration.HodUserId == userId
                    select categoryConfigration.CategoryId).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
