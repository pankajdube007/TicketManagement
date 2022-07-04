using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.CommonData;
using TicketManagement.Interface;
using TicketManagement.ViewModels;
using System.Web.Caching;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.Helpers;
//using TicketManagement.Models;
using TicketManagement.Common;
using TicketManagement.Reposistory;

namespace TicketManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserMaster _iUserMaster;
        private readonly IPassword _password;
        private readonly ISavedAssignedRoles _savedAssignedRoles;
        private readonly IAgentCheckInStatus _agentCheckInStatus;
        private readonly IVerify _verify;
        private readonly IProfile _iProfile;
        private readonly ICategory _category;
        readonly SessionHandler _sessionHandler = new SessionHandler();
        private readonly IVerification _verification;
        public LoginController(IUserMaster userMaster,
            IPassword password,
            ISavedAssignedRoles savedAssignedRoles,
            IAgentCheckInStatus agentCheckInStatus,
            IVerify verify,
            IProfile profile, ICategory category, IVerification verification)
        {
            _iUserMaster = userMaster;
            _password = password;
            _savedAssignedRoles = savedAssignedRoles;
            _agentCheckInStatus = agentCheckInStatus;
            _verify = verify;
            _iProfile = profile;
            _category = category;
            _verification = verification;
        }

        // GET: Login/Create
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!_iUserMaster.CheckUsernameExists(loginViewModel.Username))
                    {
                        TempData["LoginErrors"] = "Invalid Credentials";
                        return View(loginViewModel);
                    }

                    var usermasterModel = _iUserMaster.GetUserByUsername(loginViewModel.Username);
                    if (usermasterModel != null)
                    {
                        if (usermasterModel.IsActive == false)
                        {
                            TempData["LoginErrors"] = "User Account is Deactivated Please Contact Admin";
                            return View(loginViewModel);
                        }

                        var usersalt = _iUserMaster.GetUserSaltbyUserid(Convert.ToInt64(usermasterModel.SLNO));
                        if (usersalt == null)
                        {
                            TempData["LoginErrors"] = "Entered Username or Password is Invalid";
                            return View();
                        }

                        var storedpassword = _password.GetPasswordbyUserId(usermasterModel.SLNO);
                        if (storedpassword == null)
                        {
                            TempData["LoginErrors"] = "Invalid Credentials";
                            return View(loginViewModel);
                        }

                        var generatehash = GenerateHashSha512.Sha512(loginViewModel.Password, usersalt.PasswordSalt);


                        if (string.Equals(storedpassword, generatehash, StringComparison.Ordinal))
                        {
                            if (_savedAssignedRoles.GetAssignedRolesbyUserId(usermasterModel.SLNO) != null)
                            {
                                var rolesModel = _savedAssignedRoles.GetAssignedRolesbyUserId(usermasterModel.SLNO);

                                if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.User))
                                {
                                    //if (!_verification.CheckIsEmailVerifiedRegistration(usermasterModel.SLNO))
                                    //{
                                    //    TempData["LoginErrors"] = "Please Verify Your Email-Id to Use Application";
                                    //    return View(loginViewModel);
                                    //}

                                    //ApplicationCustomSettings applicationCustomSettings = new ApplicationCustomSettings();
                                    //if (applicationCustomSettings.GetGeneralSetting().EnableEmailFeature && _verify.CheckVerificationCodeExists(usermasterModel.SLNO))
                                    //{
                                    //    TempData["LoginErrors"] = "Please Verify Your Email-Id to Use Application";
                                    //    return View(loginViewModel);
                                    //}

                                }


                                if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.Agent) || rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.AgentAdmin) || rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.Hod))
                                {
                                    if (!IsCategogryAssigned(usermasterModel, rolesModel))
                                    {
                                        TempData["LoginErrors"] = "Category is not Assigned, Please contact your administrator";
                                        return View(loginViewModel);
                                    }
                                }

                                if (Convert.ToBoolean(usermasterModel.IsFirstLogin))
                                {
                                    Session["ChangePasswordUserId"] = usermasterModel.SLNO;
                                    Session["ChangeRoleId"] = rolesModel.RoleId;
                                    return RedirectToAction("ChangePassword", "Force");
                                }

                                AssignSessionValues(usermasterModel, rolesModel);
                                usermasterModel.LogNo = Convert.ToInt64(Response.Cookies["logno"].Value);                               
                                var result = _iUserMaster.UpdateLoginHistory(usermasterModel,"INSERT");
                                var result1 = _iUserMaster.AddLoginHistory(usermasterModel);
                                return RedirectionManager(usermasterModel, rolesModel);
                            }
                            else
                            {
                                TempData["LoginErrors"] = "Access Not Assigned";
                                return View(loginViewModel);
                            }
                        }
                        else
                        {
                            TempData["LoginErrors"] = "Invalid Credentials";
                            return View(loginViewModel);
                        }
                    }
                    else
                    {
                        TempData["LoginErrors"] = "Invalid Credentials";
                        return View(loginViewModel);
                    }
                }
                else
                {
                    return View(loginViewModel);
                }

            }
            catch (Exception ex)
            {
                var Msg = ex.Message;
                throw;
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {

            try
            {
                Usermaster usermasterModel = new Usermaster();
                usermasterModel.SLNO = Convert.ToInt64(_sessionHandler.UserId);
                usermasterModel.LogNo = Convert.ToInt64(Response.Cookies["logno"].Value);
                var result = _iUserMaster.UpdateLoginHistory(usermasterModel, "Update");

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                Response.Cache.SetNoStore();

                HttpCookie cookies = new HttpCookie("WebTime");
                cookies.Value = "";
                cookies.Expires = DateTime.Now.AddHours(-1);
                Response.Cookies.Add(cookies);
                HttpContext.Session.Clear();
                Session.Abandon();
                return RedirectToAction("Login", "Login");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        public void remove_Anonymous_Cookies()
        {
            try
            {

                if (Request.Cookies["WebTime"] != null)
                {
                    var option = new HttpCookie("WebTime") { Expires = DateTime.Now.AddDays(-1) };
                    Response.Cookies.Add(option);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult RedirectionManager(Usermaster usermasterModel, SavedAssignedRolesViewModel rolesModel)
        {
            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.SuperAdmin))
            {
                return RedirectToAction("Dashboard", "SuperDashboard");
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.User))
            {
                return RedirectToAction("Dashboard", "UserDashboard");
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.Admin))
            {
                return RedirectToAction("Dashboard", "AdminDashboard");
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.AgentAdmin))
            {
                return RedirectToAction("Dashboard", "AgentAdminDashboard");
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.Hod))
            {
                return RedirectToAction("Dashboard", "HODDashboard");
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.Agent))
            {
                if (_agentCheckInStatus.CheckIsalreadyCheckedIn(usermasterModel.SLNO))
                {
                    return RedirectToAction("Dashboard", "AgentDashboard");
                }
                else
                {
                    return RedirectToAction("Alerts", "CheckInAlert");
                }
            }

            return RedirectToAction("Login", "Login");

        }

        private void AssignSessionValues(Usermaster usermasterModel, SavedAssignedRolesViewModel rolesModel)
        {
            var sessionHandler = new SessionHandler
            {
                UserId = Convert.ToString(usermasterModel.SLNO),
                UserName = usermasterModel.FirstName + " " + usermasterModel.LastName,
                EmailId = usermasterModel.EmailId,
                RoleId = Convert.ToString(rolesModel.RoleId),
                RoleName = Convert.ToString(rolesModel.RoleName),
                CacheProfileKey = "Cache_" + usermasterModel.SLNO
            };

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.AgentAdmin))
            {
                sessionHandler.AgentAdminCategoryId = Convert.ToString(_category.GetAdminCategory(usermasterModel.SLNO));
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.Hod))
            {
                sessionHandler.HodCategoryId = Convert.ToString(_category.GetHodCategory(usermasterModel.SLNO));
            }


            var result = _iProfile.IsProfileImageExists(Convert.ToInt64(sessionHandler.UserId));

            if (result)
            {
                string cacheProfileKey = Convert.ToString(sessionHandler.CacheProfileKey);

                if (!CacheHelper.CheckExists(cacheProfileKey))
                {
                    var imageBase64String = _iProfile.GetProfileImageBase64String(Convert.ToInt64(sessionHandler.UserId));
                    var tempimageBase64String = string.Concat("data:image/png;base64,", imageBase64String);
                    CacheHelper.AddToCacheWithNoExpiration(cacheProfileKey, tempimageBase64String);
                }
            }
            string ip = HttpContext.Request.UserHostAddress;
            string MachineName1 = Environment.MachineName;
            string logintime1 = DateTime.Now.TimeOfDay.ToString();
            //string macaddress1=HttpContext.Request.getp
            var logno2 = _iUserMaster.GetLogNo(usermasterModel,ip,MachineName1, logintime1);           
            HttpContext.Response.Cookies["logno"].Value = Convert.ToString(logno2);
            HttpContext.Response.Cookies["logno"].Expires = DateTime.Now.AddHours(12);




        }


        private bool IsCategogryAssigned(Usermaster usermasterModel, SavedAssignedRolesViewModel rolesModel)
        {
            if (rolesModel.RoleId== Convert.ToInt32(StatusMain.Roles.Agent) &&_agentCheckInStatus.CheckIsCategoryAssignedtoAgent(usermasterModel.SLNO))
            {
                return true;
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.AgentAdmin) && _iUserMaster.CheckIsCategogryAssignedtoAgentAdmin(usermasterModel.SLNO))
            {
                return true;
            }

            if (rolesModel.RoleId == Convert.ToInt32(StatusMain.Roles.Hod) && _iUserMaster.CheckIsCategogryAssignedtoHod(usermasterModel.SLNO))
            {
                return true;
            }

            return false;
        }
    }
}
