using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Interface;
//using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;

namespace TicketManagement.Controllers
{
    [AuthorizeSuperAdminAttribute]
    public class AssignRoletoUserController : Controller
    {
        private readonly IRole _role;
        private readonly ISavedAssignedRoles _savedAssignedRoles;
        private readonly IUserMaster _userMaster;
        public AssignRoletoUserController(IRole role, IUserMaster userMaster, ISavedAssignedRoles savedAssignedRoles)
        {
            _role = role;
            _userMaster = userMaster;
            _savedAssignedRoles = savedAssignedRoles;
        }

        // GET: AssignRoletoUser
        public ActionResult Assign()
        {
            try
            {
                AssignViewUserRoleModel assignViewUserRoleModel = new AssignViewUserRoleModel()
                {
                    ListRole = _role.GetAllActiveRoles()

                };
                return View(assignViewUserRoleModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Assign(AssignViewUserRoleModel assignViewUserRoleModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    assignViewUserRoleModel = new AssignViewUserRoleModel()
                    {
                        ListRole = _role.GetAllActiveRoles(),
                        RoleId = assignViewUserRoleModel.RoleId,
                        UserId = assignViewUserRoleModel.UserId
                    };
                    return View(assignViewUserRoleModel);
                }

                if (_savedAssignedRoles.CheckAssignedRoles(assignViewUserRoleModel.UserId))
                {
                    assignViewUserRoleModel = new AssignViewUserRoleModel()
                    {
                        ListRole = _role.GetAllActiveRoles(),
                        RoleId = assignViewUserRoleModel.RoleId,
                        UserId = assignViewUserRoleModel.UserId
                    };

                    TempData["AssignedErrorMessage"] = "Role is Already Assigned to User";
                    return View(assignViewUserRoleModel);
                }
                else
                {
                    SavedAssignedRole savedAssignedRoles = new SavedAssignedRole()
                    {
                        RoleId = assignViewUserRoleModel.RoleId,
                        UserId = assignViewUserRoleModel.UserId,
                        IsActive = true,
                        Createdt = DateTime.Now,
                        SLNO = 0
                    };
                    _savedAssignedRoles.AddAssignedRoles(savedAssignedRoles);
                    TempData["AssignedMessage"] = "Role Assigned to User Successfully";
                    return RedirectToAction("Assign", "AssignRoletoUser");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetActiveUsersExceptSuperAdmin(string usernames)
        {
            try
            {
                return Json(_userMaster.GetAllUsersExceptSuperAdmin(usernames), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}