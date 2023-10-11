using FXAdminTransferConnex.Business.User;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FXAdminTransferConnexApp.Controllers
{
    public class SettingController : BaseAdminController
    {
        #region Initializations

        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly IUserService _usersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingController"/> class.
        /// </summary>
        public SettingController()
        {
            _usersService = EngineContext.Resolve<IUserService>();
        }
        #endregion

        #region Setting

        // GET: Setting
        public ActionResult Index()
        {
            SettingModel model = new SettingModel();
            model.UserId = ProjectSession.LoginUserDetails.UserId;
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(SettingModel model)
        {
            if (Security.Encrypt(model.OldPassword) == ProjectSession.LoginUserDetails.Password)
            {
                if (_usersService.ChangePassword(model))
                    SuccessNotification("New Password changed successfully");
                else
                    ErrorNotification("Error Occur record not saved successfully");
            }
            else
            {
                ModelState.AddModelError("OldPassword", "Old password not match.");
            }

            return View("Index", model);
        }
    }
    #endregion
}