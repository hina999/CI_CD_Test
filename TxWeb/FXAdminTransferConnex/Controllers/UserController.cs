using System.Web.Mvc;
using FXAdminTransferConnex.Business.User;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using FXAdminTransferConnex.Entities.LocalizationResource;
using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;

namespace FXAdminTransferConnexApp.Controllers
{

    public class UserController : BaseAdminController
    {
        #region Initializations

        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly IUserService _usersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public UserController()
        {
            _usersService = EngineContext.Resolve<IUserService>();
        }
        #endregion

        #region User

        /// <summary>
        /// By Mayank
        /// 14 FEB 2017
        /// Get list of users
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //ViewData["UserList"] = SystemEnum.GetEnumList(typeof(SystemEnum.UserType)).Where(x => x.ID != SystemEnum.UserType.Client.GetHashCode());
            ViewData["UserList"] = Enum.GetValues(typeof(SystemEnum.UserType)).Cast<SystemEnum.UserType>().Where(e => e != SystemEnum.UserType.Client).Where(e => e != SystemEnum.UserType.Trader)
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.GetDescription().ToString()
                });

            return View();
        }


        /// <summary>
        /// By Mayank
        /// 14 FEB 2017
        /// Gets the list of users
        /// </summary>
        /// <param name="request"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult GetUserList([DataSourceRequest]DataSourceRequest request, string search)
        {
            List<UserModel> userList = _usersService.GetUserlist(search);
            return Json(userList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        
        /// <summary>
        /// By Mayank
        /// 15 FEB 2017
        /// Change user active status
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeStatus(int userId, bool status)
        {
            bool result = _usersService.Changestatus(userId, status);
            return Json(result
                ? new { Message = FXAdminTransferConnexResource.StatusSuccess, IsError = Enums.NotifyType.Success }
                : new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 15 FEB 2017
        /// Delete user details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult DeleteUser(long userId)
        {
            bool result = _usersService.DeleteUser(userId);
            return Json(result
                ? new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }
                : new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                , JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// By Mayank
        /// 17 FEB 2017
        /// Get list of users
        /// </summary>
        /// <returns></returns>
        public ActionResult MyProfile()
        {
            UserModel result = _usersService.GetUserDetailsById(ProjectSession.LoginUserDetails.UserId);
            return View(result);
        }

        

        [HttpGet]
        public ActionResult ManageUser(int id = 0)
        {
            UserModel model = new UserModel();
            ViewBag.UserTypeList = Enum.GetValues(typeof(SystemEnum.UserType)).Cast<SystemEnum.UserType>().Where(e => e != SystemEnum.UserType.Client).Where(e => e != SystemEnum.UserType.Trader)
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.GetDescription().ToString()
                });
            UserModel userModel = _usersService.GetUserDetailsById(id);
            if (userModel != null)
            {
                model = userModel;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ManageUser(UserModel model)
        {
            if (model != null && model.ImageFile != null)
            {
                string FileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                string FileExtension = Path.GetExtension(model.ImageFile.FileName);
                Guid guidFile = Guid.NewGuid();
                FileName = guidFile + FileExtension;

                string UploadPath = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["UserImagePath"].ToString());

                if (!Directory.Exists(UploadPath))
                {
                    Directory.CreateDirectory(UploadPath);
                }

                model.ImageName = FileName;

                model.ImageFile.SaveAs(Path.Combine(UploadPath, Regex.Replace(System.Web.HttpUtility.UrlDecode(FileName), @"\s+", String.Empty)));

            }

            model.UpdatedBy = ProjectSession.LoginUserDetails.UserId;
            model.CreatedBy = ProjectSession.LoginUserDetails.UserId;
            int result = _usersService.AddUser(model);

            if (result <= 0)
            {
                if (result == -2)
                    ErrorNotification("Email address already exists");
                else
                    ErrorNotification("Error Occur record not saved successfully");
            }
            else
            {
                if (model.UserId > 0)
                {
                    SuccessNotification("User Updated Successfully");
                }
                else
                {
                    SuccessNotification("User Saved Successfully");
                }
            }
            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public ActionResult MyProfile(UserModel obj)
        {
            obj.UpdatedBy = ProjectSession.LoginUserDetails.UserId;
            var result = _usersService.AddUser(obj);
            return RedirectToAction("MyProfile");
        }

        #endregion
        #region User Rights Assignment
        public ActionResult UserRightsAssignment()
        {
            ViewBag.UserTypeList = Enum.GetValues(typeof(SystemEnum.UserType)).Cast<SystemEnum.UserType>().Where(e => e != SystemEnum.UserType.Client)
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.GetDescription().ToString()
                });

            return View();
        }
        public PartialViewResult GetAccessPermissionsByUserId(int userTypeId, long userId)
        {
            ViewData["userRightsList"] = _usersService.GetAccessPermissionsByUserId(userTypeId, userId);
            return PartialView("~/Views/User/_RightsAssigntblPartial.cshtml");
        }

        [HttpPost]
        public ActionResult SaveUserRightsAssignment(UserAssignRightsModel model)
        {
            if (!string.IsNullOrEmpty(model.strRoleRights))
            {
                model.roleOperationList = JsonConvert.DeserializeObject<List<UserRoleOperation>>(model.strRoleRights);

            }
            int result = _usersService.SaveUserRightsAssignment(model);
            if (result > 0)
            {
                SuccessNotification("User rights assignment successfully");
            }
            else
            {
                ErrorNotification("Error in user rights assignment");

            }
            return RedirectToAction("UserRightsAssignment");
        }

        #endregion
    }
}