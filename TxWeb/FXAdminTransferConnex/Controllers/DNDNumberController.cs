using FXAdminTransferConnex.Business.Client;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnex.Entities.LocalizationResource;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FXAdminTransferConnexApp.Controllers
{
    public class DNDNumberController : BaseAdminController
    {
        private readonly IClientService _clientService;

        public DNDNumberController()
        {
            _clientService = EngineContext.Resolve<IClientService>();
        }

        // GET: DNDNumber
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDNDList([DataSourceRequest]DataSourceRequest request, string FullName, string MobileNo)
        {
            List<DNDNumbers> result = _clientService.GetDNDNumberlist(FullName, MobileNo);
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveDNDNumber(DNDNumbers model)
        {
            int result = _clientService.SaveDNDNumber(model);

            switch (result)
            {
                case -2:
                    ErrorNotification("Number already exist.");
                    return RedirectToAction("ManageNumber", new { DNDNumberID = model.DNDNumberID });
                case 0:
                    ErrorNotification("Error Occur record not saved successfully");
                    return RedirectToAction("ManageNumber", new { DNDNumberID = model.DNDNumberID });
                default:
                    if (model.DNDNumberID > 0)
                        SuccessNotification("Number Updated Successfully");
                    else
                        SuccessNotification("Number Saved Successfully");
                    break;
            }

            return RedirectToAction("Index", "DNDNumber");
        }

        public ActionResult DeleteDNDNumber(long dndNumberId)
        {
            int result = _clientService.DeleteDNDNumber(dndNumberId);

            switch (result)
            {
                case 0:
                    return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ManageNumber(long DNDNumberID = 0)
        {
            DNDNumbers model = new DNDNumbers();
            DNDNumbers responce = _clientService.GetDNDNumberById(DNDNumberID);
            if (responce != null)
            {
                model = responce;
            }
            return View(model);
        }

    }
}