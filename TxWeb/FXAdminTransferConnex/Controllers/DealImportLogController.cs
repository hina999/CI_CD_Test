using FXAdminTransferConnex.Business.Deal;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FXAdminTransferConnexApp.Controllers
{
    public class DealImportLogController : Controller
    {

        private readonly IDealService _DealService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public DealImportLogController()
        {
            _DealService = EngineContext.Resolve<IDealService>();
        }
        // GET: DealImportLog
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetImportDealList([DataSourceRequest]DataSourceRequest request)
        {
            List<FXAdminTransferConnex.Entities.ImportLogModel> userList = _DealService.GetDealImportData();
            return Json(userList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
    }
}