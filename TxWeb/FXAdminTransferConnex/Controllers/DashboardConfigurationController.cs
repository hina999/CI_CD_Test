using FXAdminTransferConnex.Business.DashboardConfiguration;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using FXAdminTransferConnex.Entities;

namespace FXAdminTransferConnexApp.Controllers
{
    public class DashboardConfigurationController : BaseAdminController
    {

        #region Initializations

        /// <summary>
        /// The DashboardConfiguration Service
        /// </summary>
        private readonly IDashboardConfigurationService _dashboardConfigService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        public DashboardConfigurationController()
        {

            _dashboardConfigService = EngineContext.Resolve<IDashboardConfigurationService>();

        }
        #endregion

        #region Dashboard Configuration
        // GET: DashboardConfiguration

        [Route("DashboardConfiguration")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUserList()
        {

            List<UserDashboardConfigurationModel> userList = _dashboardConfigService.GetAllUserList().ToList();
            return new JsonResult { Data = userList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetWizardList([DataSourceRequest] DataSourceRequest request, int UserId = 0)
        {

            List<UserDashboardConfigurationModel> wizardList = _dashboardConfigService.GetWizardListById(UserId).ToList();
            return Json(wizardList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);



        }

        public ActionResult SwapWizardDisplayOrder(int WizardId, bool IsMoveUp, int UserId)
        {
            int result = _dashboardConfigService.SwapWizardDisplayOrder(WizardId, IsMoveUp, UserId);
            // 1 for success & 0 for error 
            return new JsonResult { Data = result > 0, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult VisibilityChangeWizard(int WizardId, bool status, int UserId)
        {
            int result = _dashboardConfigService.VisibilityChangeWizard(UserId, WizardId, status);
            // 1 for success & 0 for error 
            return new JsonResult { Data = result > 0, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        #endregion
    }
}