using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagement.CommonData;
using TicketManagement.Helpers;

namespace TicketManagement.Filters
{
    public class AuthorizeAdminAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                SessionHandler sessionHandler = new SessionHandler();
                var role = Convert.ToString(sessionHandler.RoleId);

                if (!string.IsNullOrEmpty(role))
                {
                    var roleValue = Convert.ToInt32(role);

                    if (roleValue != Convert.ToInt32(StatusMain.Roles.Admin) 
                        && roleValue != Convert.ToInt32(StatusMain.Roles.AgentAdmin) 
                        && roleValue != Convert.ToInt32(StatusMain.Roles.Hod))
                    {
                        filterContext.HttpContext.Session.Abandon();

                        filterContext.Result = new RedirectToRouteResult
                        (
                            new RouteValueDictionary
                            (new
                                { controller = "Error", action = "Error" }
                            ));
                    }
                }
                else
                {
                    filterContext.HttpContext.Session.Abandon();

                    filterContext.Result = new RedirectToRouteResult
                    (
                        new RouteValueDictionary
                        (new
                            { controller = "Error", action = "Error" }
                        ));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class AuthorizeAgentsAdminAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                SessionHandler sessionHandler = new SessionHandler();
                var role = Convert.ToString(sessionHandler.RoleId);

                if (!string.IsNullOrEmpty(role))
                {
                    var roleValue = Convert.ToInt32(role);

                    if (roleValue != Convert.ToInt32(StatusMain.Roles.AgentAdmin))
                    {
                        filterContext.HttpContext.Session.Abandon();

                        filterContext.Result = new RedirectToRouteResult
                        (
                            new RouteValueDictionary
                            (new
                                { controller = "Error", action = "Error" }
                            ));
                    }
                }
                else
                {
                    filterContext.HttpContext.Session.Abandon();

                    filterContext.Result = new RedirectToRouteResult
                    (
                        new RouteValueDictionary
                        (new
                            { controller = "Error", action = "Error" }
                        ));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}