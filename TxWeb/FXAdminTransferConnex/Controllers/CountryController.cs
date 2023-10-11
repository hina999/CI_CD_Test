using System.Web.Mvc;
using FXAdminTransferConnex.Business.Country;
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
    public class CountryController : BaseAdminController
    {
        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly ICountryServices  _CountryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public CountryController()
        {
            _CountryService = EngineContext.Resolve<ICountryServices>();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetCountry([DataSourceRequest]DataSourceRequest request, string search = "")
        {
            List<CountryMasterModel> CountryList = _CountryService.GetCountry(0, search);
            return Json(CountryList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCountryListForDisplay()
        {
            List<CountryMasterModel> CountryList = _CountryService.GetCountry(0, "");
            return new JsonResult
            {
                Data = CountryList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult DeleteCountry([DataSourceRequest]DataSourceRequest request, CountryMasterModel obj)
        {
            bool result = _CountryService.DeleteCountry(obj.CountryID);
            return Json(result
                ? new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }
                : new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                , JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ManageCountry(int id = -1)
        {
            CountryMasterModel model = new CountryMasterModel();
            List<CountryMasterModel> CountryModel = _CountryService.GetCountry(id, "");
            if (CountryModel != null)
            {
                model = CountryModel.FirstOrDefault();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ManageCountry(CountryMasterModel model)
        {
            if (model.CountryID > 0)
            {
                SuccessNotification("Country Updated Successfully");
                model.UpdateBy = ProjectSession.LoginUserDetails.UserId;
                _CountryService.UpdateCountry(model);
            }
            else
            {
                SuccessNotification("User Saved Successfully");
                model.CreatedBy = ProjectSession.LoginUserDetails.UserId;
                _CountryService.AddCountry(model);
            }

            return RedirectToAction("Index");
        }
    }
}