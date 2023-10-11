using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FXAdminTransferConnex.Business.User;
using FXAdminTransferConnex.Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.Owin.Security.Provider;
using FXAdminTransferConnex.Data.Models;

namespace FXAdminTransferConnexApp.Controllers
{
    public class UserRightsController : BaseAdminController
    {
        #region Initializations

        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly IUserService _usersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public UserRightsController()
        {
            _usersService = EngineContext.Resolve<IUserService>();
        }
        #endregion

        [Route("UserRights")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the list of user types
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult KendoRead([DataSourceRequest]DataSourceRequest request)
        {
            List<UserType> userList = _usersService.GetUserTypelist();
            return Json(userList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserTypeMenusView()
        {
            return PartialView("~/Views/UserRights/_UserTypeMenuAssignment.cshtml");
        }

        public ActionResult ReadUserTypeAction(DataSourceRequest request, short userTypeId)
        {
            List<UserTypeMenuSearch> userList = _usersService.GetUserTypeMenuSearchlist(userTypeId);
            return Json(userList.ToDataSourceResult(request));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Save(string model)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                IEnumerable<UserTypeMenuSearch> userTypeMenuSearchList = serializer.Deserialize<IEnumerable<UserTypeMenuSearch>>(model);

                #region DataTable
                DataTable dtUserTypeMenus = new DataTable("UserTypeMenusTable");
                dtUserTypeMenus.Columns.Add("UserTypeMenuId");
                dtUserTypeMenus.Columns.Add("MenuId");
                dtUserTypeMenus.Columns.Add("UserTypeId");
                dtUserTypeMenus.Columns.Add("IsView");
                dtUserTypeMenus.Columns.Add("IsAdd");
                dtUserTypeMenus.Columns.Add("IsDelete");
                dtUserTypeMenus.Columns.Add("IsEdit");
                #endregion

                foreach (UserTypeMenuSearch objUserTypeMenuSearch in userTypeMenuSearchList)
                {
                    DataRow dtrow = dtUserTypeMenus.NewRow();
                    dtrow["UserTypeMenuId"] = objUserTypeMenuSearch.UserTypeMenuId;
                    dtrow["MenuId"] = objUserTypeMenuSearch.MenuId;
                    dtrow["UserTypeId"] = objUserTypeMenuSearch.UserTypeId;
                    dtrow["IsView"] = objUserTypeMenuSearch.IsView;
                    dtrow["IsAdd"] = objUserTypeMenuSearch.IsAdd;
                    dtrow["IsDelete"] = objUserTypeMenuSearch.IsDelete;
                    dtrow["IsEdit"] = objUserTypeMenuSearch.IsEdit;
                    dtUserTypeMenus.Rows.Add(dtrow);
                }

               int count = _usersService.SaveUserTypeActions(dtUserTypeMenus);

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}