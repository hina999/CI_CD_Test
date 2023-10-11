using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnex.Common;
using log4net;

namespace TransferConnexChatApp.Controllers
{
 
    [UserAuthorizationAttribute]
    public class BaseAdminController : Controller
    {
        protected readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true, bool saveInSession = false)
        {
            AddNotification(Enums.NotifyType.Success, message, persistForTheNextRequest, saveInSession);
        }
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true, bool saveInSession = false)
        {
            AddNotification(Enums.NotifyType.Error, message, persistForTheNextRequest, saveInSession);
        }
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            AddNotification(Enums.NotifyType.Error, exception.Message, persistForTheNextRequest, false);
        }
        protected virtual void AddNotification(Enums.NotifyType type, string message, bool persistForTheNextRequest, bool saveInSession)
        {
            var dataKey = string.Format("tmp.notifications.{0}", type);

            if (!saveInSession)
            {
                if (persistForTheNextRequest)
                {
                    if (TempData[dataKey] == null)
                    {
                        TempData[dataKey] = new List<string>();
                    }

                    ((List<string>)TempData[dataKey]).Add(message);
                }
                else
                {
                    if (ViewData[dataKey] == null)
                    {
                        ViewData[dataKey] = new List<string>();
                    }

                    ((List<string>)ViewData[dataKey]).Add(message);
                }
            }
            else
            {
                if (Session[dataKey] == null)
                {
                    Session[dataKey] = new List<string>();
                }

                ((List<string>)Session[dataKey]).Add(message);
            }
        }
        public sealed class UserAuthorizationAttribute : ActionFilterAttribute
        {
            /// <summary>
            /// Called by the ASP.NET MVC framework before the action method executes.
            /// </summary>
            /// <param name="filterContext">The filter context.</param>
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var ctx = System.Web.HttpContext.Current;

                string controllerName = (filterContext.Controller.ToString().Split('.')[filterContext.Controller.ToString().Split('.').Length - 1]).ToLower();

                if (controllerName != "logincontroller")
                {
                    // check if session is supported          
                    if (ProjectSession.LoginChatUserDetails != null)
                    {
                        if (controllerName != "securitycontroller")
                        {
                            //var list = ProjectSession.LoginUserDetails.RightsList;

                            if (1 == 0)
                            {
                                if (ctx.Request.HttpMethod.ToLower() != "post")
                                {
                                    filterContext.Result = new RedirectResult("~/Security/AccessDenied");
                                }
                              
                            }

                            //if (list != null && list.All(p => !String.Equals(p.RightsName, Convert.ToString(filterContext.Controller.ToString().Split('.')[filterContext.Controller.ToString().Split('.').Length - 1]), StringComparison.CurrentCultureIgnoreCase)))
                            //{
                            //    if (Convert.ToString(filterContext.Controller.ToString().Split('.')[filterContext.Controller.ToString().Split('.').Length - 1]).ToLower() != "HomeController")
                            //    {
                            //        filterContext.Result = new RedirectResult("~/Security/AccessDenied");
                            //        return;
                            //    }
                            //    filterContext.Result = new RedirectResult("~/Default/Index");
                            //}
                        }
                    }
                    else if (!filterContext.HttpContext.Request.IsAjaxRequest() && controllerName != "logincontroller"  && ProjectSession.LoginChatUserDetails == null)
                    {
                        filterContext.Result = ctx.Request.HttpMethod.ToLower() != "post"
                                    ? new RedirectResult("~/Login/Login?returnUrl=" + ctx.Request.Url)
                                    : new RedirectResult("~/Login/Login");
                    }
                    else
                    {
                        // if it's Ajax request 
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            // Set HttpContext Item  AjaxPermissionDenied 
                            // It will check on Global.asax->  Application_EndRequest with HttpContext item.
                            filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
                        }
                        else
                        {
                            if (!(ctx.Request.Url).AbsolutePath.Contains("/Security/AccessDenied") && controllerName != "dealcontroller")
                            {
                                // string ReturnURL = Redi Url.Action("Login", "Login", new { id = "#=LanguageID#" });
                                // check if a new session id was generated
                                filterContext.Result = ctx.Request.HttpMethod.ToLower() != "post"
                                    ? new RedirectResult("~/Login/Login?returnUrl=" + ctx.Request.Url)
                                    : new RedirectResult("~/Login/Login");
                            }
                        }

                        return;
                    }
                }
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();
                base.OnActionExecuting(filterContext);
            }
        }

    }
}