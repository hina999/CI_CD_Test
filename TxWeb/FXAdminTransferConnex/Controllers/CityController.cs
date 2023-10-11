using System.Web.Mvc;
using FXAdminTransferConnex.Business.City;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using FXAdminTransferConnex.Entities.LocalizationResource;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FXAdminTransferConnexApp.Controllers
{
    public class CityController : BaseAdminController
    {
        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly ICityService _CityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public CityController()
        {
            _CityService = EngineContext.Resolve<ICityService>();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetCity([DataSourceRequest]DataSourceRequest request, string search = "")
        {
            List<CityMasterModel> CityList = _CityService.GetCity(0, search);
            return Json(CityList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        public ActionResult DeleteCity([DataSourceRequest]DataSourceRequest request, CityMasterModel obj)
        {
            bool result = _CityService.DeleteCity(obj.CityID);
            return Json(result
                ? new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }
                : new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                , JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ManageCity(int id = -1)
        {
            CityMasterModel model = new CityMasterModel();
            List<CityMasterModel> CityModel = _CityService.GetCity(id, "");
            if (CityModel != null)
            {
                model = CityModel.FirstOrDefault();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ManageCity(CityMasterModel model)
        {
            if (model.CityID > 0)
            {
                SuccessNotification("City Updated Successfully");
                model.UpdateBy = ProjectSession.LoginUserDetails.UserId;
                _CityService.UpdateCity(model);
            }
            else
            {
                SuccessNotification("City Saved Successfully");
                model.CreatedBy = ProjectSession.LoginUserDetails.UserId;
                _CityService.AddCity(model);
            }

            return RedirectToAction("Index");
        }
    }
}