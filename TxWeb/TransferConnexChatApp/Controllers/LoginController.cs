using FX_System.Models;
using FXAdminTransferConnex.Business.ClientChat;
using FXAdminTransferConnex.Business.User;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Data.LocalizationResource;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnex.Entities.LocalizationResource;
using log4net;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using LoginViewModel = FXAdminTransferConnex.Entities.LoginViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TransferConnexChatApp.Controllers
{
    public class LoginController : BaseAdminController
    {
        #region Initializations

        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly IUserService _usersService;
        private readonly IClientChatService _signalrService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientController"/> class.
        /// </summary>
        public LoginController()
        {
            _usersService = EngineContext.Resolve<IUserService>();
            _signalrService = EngineContext.Resolve<IClientChatService>();
        }

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Initializes a new instance of the  class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        public LoginController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        /// <summary>
        /// Gets the user manager.
        /// </summary>
        /// <value>The user manager.</value>
        public UserManager<ApplicationUser> UserManager { get; private set; }

        #endregion

        #region Login

        /// <summary>
        /// It is used to render login page with saved cookies details
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>Return ActionResult.</returns>
        [HttpGet]
        [AllowAnonymous]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Login(string returnUrl,string userId)
         {
            var model = new LoginViewModel();
            if (userId != "" && userId != null)
            {
                var user = _usersService.GetUserDetailsById(Convert.ToInt64(Security.Decrypt(userId)));
                if(user != null)
                {
                    var result = _usersService.AuthenticateUser(user.EmailAddress, user.Password);
                    if (result != null)
                    {
                        var objLoginUser = result;
                        objLoginUser.Email = user.EmailAddress;

                        string scheme = Request.Url.Scheme.ToString();
                        string authority = Request.Url.Authority.ToString();
                        string url = scheme + "://" + authority;
                        string ImagePath = ConfigurationManager.AppSettings["UserImagePath"];
                        if (!string.IsNullOrEmpty(ImagePath) && !string.IsNullOrEmpty(objLoginUser.ImageName))
                        {
                            string UserImagePath = ImagePath.Substring(1);
                            string finalpath = url + UserImagePath + objLoginUser.ImageName;
                            objLoginUser.ImageURL = finalpath;
                        }
                        ProjectSession.LoginChatUserDetails = objLoginUser;

                        //ProjectSession.UserAccessPermissions = _usersService.UserAccessPermissions(objLoginUser.UserTypeId, objLoginUser.UserId);
                        //ProjectSession.LastJobExecutedDate = objLoginUser.LastJobExecutedDate.HasValue ? objLoginUser.LastJobExecutedDate.Value : DateTime.MinValue;

                        // await CommonController.GetCurrencyCloudToken();

                     
                    }
                    else
                    {
                        ModelState.AddModelError("Password", FXAdminTransferConnexResource.UsernamePassworddoesnotmatch);
                    }
                    return RedirectToAction("LoadcontactsListToslack", "SignalRHelper");
                }
                else
                {
                    return View(model);
                }
           
            }
            else
            {
                ProjectSession.LoginChatUserDetails = null;

                ViewBag.ReturnUrl = returnUrl;

                
                var authCookie = Request.Cookies["TransferConnexChatCookie"];
                if (authCookie != null)
                {
                    try
                    {
                        var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                        if (authTicket != null)
                            model = JsonConvert.DeserializeObject<LoginViewModel>(authTicket.UserData);

                        model = LoginProcess(model);
                    }
                    catch (Exception ex)
                    {
                        Logout();
                    }
                }

                if (ProjectSession.LoginChatUserDetails == null)
                    return View(model);

                if (string.IsNullOrEmpty(returnUrl))
                {
                    //return View(model);
                    return RedirectToAction("LoadcontactsListToslack", "SignalRHelper");
                }
                //else
                //{
                //    return Redirect(TempData["ReturnURL"].ToString()); 
                //}

                return new RedirectResult(returnUrl);
            }
          
        }

        /// <summary>
        /// This method is used to check login credential and return login model or error message if any
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="changesiteid">The change site-id.</param>
        /// <returns>Return LoginViewModel</returns>
        public LoginViewModel LoginProcess(LoginViewModel model, int? changesiteid = 0)
        {
            //var result = _usersService.AuthenticateUser(model.Email, HttpUtility.UrlEncode(model.Password));
            var result = _usersService.AuthenticateUser(model.Email, model.Password);


            if (result != null)
            {
                var objLoginUser = result;
                objLoginUser.Email = model.Email;

                string scheme = Request.Url.Scheme.ToString();
                string authority = Request.Url.Authority.ToString();
                string url = scheme + "://" + authority;
                string ImagePath = ConfigurationManager.AppSettings["UserImagePath"];
                if (!string.IsNullOrEmpty(ImagePath) && !string.IsNullOrEmpty(objLoginUser.ImageName))
                {
                    string UserImagePath = ImagePath.Substring(1);
                    string finalpath = url + UserImagePath + objLoginUser.ImageName;
                    objLoginUser.ImageURL = finalpath;
                }
                ProjectSession.LoginChatUserDetails = objLoginUser;

                //ProjectSession.UserAccessPermissions = _usersService.UserAccessPermissions(objLoginUser.UserTypeId, objLoginUser.UserId);
                //ProjectSession.LastJobExecutedDate = objLoginUser.LastJobExecutedDate.HasValue ? objLoginUser.LastJobExecutedDate.Value : DateTime.MinValue;

                // await CommonController.GetCurrencyCloudToken();

                if (model.RememberMe)
                {
                    var userData = JsonConvert.SerializeObject(model);
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        model.Email,
                        DateTime.UtcNow,
                        DateTime.UtcNow.AddDays(30),
                        model.RememberMe, // pass here true, if you want to implement remember me functionality
                        userData);
                    var encTicket = FormsAuthentication.Encrypt(authTicket);
                    var facookie = new HttpCookie("TransferConnexChatCookie", encTicket) { Expires = DateTime.UtcNow.AddDays(30) };
                    Response.Cookies.Add(facookie);
                }
                else
                {
                    if (Request.Cookies["TransferConnexChatCookie"] == null) return model;
                    var myCookie = new HttpCookie("TransferConnexChatCookie") { Expires = DateTime.UtcNow.AddDays(-1d) };
                    Response.Cookies.Add(myCookie);
                }
            }
            else
            {
                ModelState.AddModelError("Password", FXAdminTransferConnexResource.UsernamePassworddoesnotmatch);
            }

            return model;
        }

        /// <summary>
        /// This method is used to check login credential entered by user
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="formCollection"></param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>If user is verified then send him to return URL or dashboard</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, FormCollection formCollection, string returnUrl)
        {
            try
            {
                model.RememberMe = formCollection["RememberMe"] != null;
                model = LoginProcess(model);

                if (ProjectSession.LoginChatUserDetails == null)
                    return View(model);

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                //var data = ProjectSession.UserAccessPermissions.FirstOrDefault();
                //if (data == null || ProjectSession.UserAccessPermissions.Count == 0)
                //{
                //    return RedirectToAction("PermissionDenied", "Common");
                //}

                return RedirectToAction("LoadcontactsListToslack", "SignalRHelper");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                ModelState.AddModelError(string.Empty, "");
                return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult FXTransferConnexLogin(string email)
        {
            return View();
        }

        /// <summary>
        /// It is used to logout the user
        /// </summary>
        /// <returns>redirect to login page</returns>
        [AllowAnonymous]
        public ActionResult Logout()
        {
            try
            {
                bool result = _signalrService.ChangeUserOnlineStatus(ProjectSession.LoginChatUserDetails.UserId);
            }
            catch (Exception ex)
            {
            }


            FormsAuthentication.SignOut();
            Session.Abandon();

            if (Request.Cookies["TransferConnexChatCookie"] == null)
                return RedirectToAction("LogIn", "LogIn");

            var myCookie = new HttpCookie("TransferConnexChatCookie")
            {
                Expires = DateTime.UtcNow.AddDays(-1d)
            };

            Response.Cookies.Add(myCookie);

            return RedirectToAction("Login", "Login");
        }

        #endregion

        #region Forgot password

        /// <summary>
        /// By Viram Keshwala
        /// 10 FEB 2017
        /// Renders the page which enables admins to enter email 
        /// if they have forgot it
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }


        /// <summary>
        /// By Viram Keshwala
        /// 10 FEB 2017
        /// Sends an email with reset password link to the admin
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ForgotPassword(LoginViewModel model)
        {

            string message = string.Empty;
            try
            {
                var userModel = _usersService.GetUserDetailOnEmail(model.Email);
                if (userModel.Count == 0)
                {
                    ModelState.AddModelError("Email", "Please enter valid email.");
                    return View();
                }
                else
                {
                    string bodyTemplate = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplates/ForgetPassword.html"));

                    //Add ImageD:\Project\FxAdmin-TransferConnex\trunk\FXAdminTransferConnex\Images\transfer-connex-logo.png

                    bodyTemplate = bodyTemplate.Replace("[@FirstName]", userModel[0].FirstName);
                    bodyTemplate = bodyTemplate.Replace("[@LastName]", userModel[0].LastName);
                    bodyTemplate = bodyTemplate.Replace("[@email]", userModel[0].EmailAddress);
                    bodyTemplate = bodyTemplate.Replace("[@password]", Security.Decrypt(userModel[0].Password));

                    bool sendmail = FXAdminTransferConnex.Common.EmailHelper.Send(userModel[0].EmailAddress, bodyTemplate, "Login Details");

                    if (sendmail)
                    {
                        SuccessNotification("Your login detail has been sent to your email address. please check your registered email address.");
                    }
                    else
                    {
                        ErrorNotification("Something went wrong with forgot password process. please try again later.");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorNotification("Something went wrong with forgot password process. please try again later.");
            }
            return RedirectToAction("Login", "Login");
        }

        #endregion

        #region Reset Password

        /// <summary>
        /// By Viram Keshwala
        /// 13 FEB 2017
        /// Renders the page Reset password after users clicks the 
        /// link sent to their mail in forgot password case
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPassword()
        {
            //Difference b/w current time and the link sent time
            var diff = DateTime.UtcNow
                        - DateTime.ParseExact(Security.Decrypt(Request.QueryString["key"]), "yyyy-MM-dd HH:mm:ss tt", CultureInfo.InvariantCulture);

            return View(diff.TotalHours >= 10
                            ? "~/Views/Security/LinkExpired.cshtml"
                            : "~/Views/Login/ResetPassword.cshtml");

        }

        /// <summary>
        /// By Viram Keshwala
        /// 13 FEB 2017
        /// Resets the password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(LoginViewModel model)
        {
            var userId = Request.QueryString["userid"];
            try
            {

                if (_usersService.ResetPassword(model, userId))
                {
                    ViewBag.Success = "suceess";
                    ModelState.AddModelError(string.Empty, FXBackOfficeSystemResource.PasswordResetSuccess);
                    return View("~/Views/Login/ResetPassword.cshtml", model);
                }

                ModelState.AddModelError(string.Empty, FXBackOfficeSystemResource.PasswordResetFailed);
                return View("~/Views/Login/ResetPassword.cshtml", model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                ModelState.AddModelError(string.Empty, ErrorHelper.GetCustomErrorMessage(ex));
                return View("~/Views/Login/ResetPassword.cshtml", model);
            }
        }

        #endregion

    }
}