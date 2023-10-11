using System.Web.Mvc;
using FXAdminTransferConnex.Business.Region;
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
    public class RegionController : BaseAdminController
    {
        

        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly IRegionService _RegionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public RegionController()
        {
            _RegionService = EngineContext.Resolve<IRegionService>();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetRegion([DataSourceRequest]DataSourceRequest request, string search="")
        {
            List<RegionMasterModel> RegionList = _RegionService.GetRegion(0,search);
            return Json(RegionList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetRegionListForDisplay()
        {
            List<RegionMasterModel> RegionList = _RegionService.GetRegion(0, "");
            return new JsonResult
            {
                Data = RegionList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult DeleteRegion([DataSourceRequest]DataSourceRequest request, RegionMasterModel obj)
        {
            bool result = _RegionService.DeleteRegion(obj.RegionID);
            return Json(result
                ? new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }
                : new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                , JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult ManageRegion(int id = -1)
        {
            RegionMasterModel model = new RegionMasterModel();
            List<RegionMasterModel> RegionModel = _RegionService.GetRegion(id,"");
            if (RegionModel != null)
            {
                model = RegionModel.FirstOrDefault();
            }            
            return View(model);
        }

        [HttpPost]
        public ActionResult ManageRegion(RegionMasterModel model)
        {
            if (model.RegionID > 0)
            {
                SuccessNotification("Region Updated Successfully");
                model.UpdatedBy = ProjectSession.LoginUserDetails.UserId;
                _RegionService.UpdateRegion(model);
            }
            else
            {
                SuccessNotification("User Saved Successfully");
                model.CreatedBy = ProjectSession.LoginUserDetails.UserId;
                _RegionService.AddRegion(model);
            }
            
            return RedirectToAction("Index");
        }        
    }
}