using FXAdminTransferConnex.Business.Trader;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnex.Entities.LocalizationResource;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;

namespace FXAdminTransferConnexApp.Controllers
{
    public class TraderController : BaseAdminController
    {
        #region Initializations

        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly ITraderService _traderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraderController"/> class.
        /// </summary>
        public TraderController()
        {
            _traderService = EngineContext.Resolve<ITraderService>();
        }
        #endregion

        #region Trader

        // GET: Trader
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// By Mayank
        /// 20 March 2018
        /// Get Trader list
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Search"></param>
        /// <returns></returns>
        public ActionResult GetTraderList([DataSourceRequest]DataSourceRequest request, string Search)
        {
            int pageNo = request.Page;
            int pageSize = request.PageSize;
            string sortColumn = string.Empty;
            string sortDir = string.Empty;

            foreach (SortDescriptor sort in request.Sorts)
            {
                sortColumn = sort.Member;

                if (sort.SortDirection == System.ComponentModel.ListSortDirection.Ascending)
                {
                    sortDir = "ASC";
                }
                else
                {
                    sortDir = "DESC";
                }
            }

            List<TraderModel> traderList = _traderService.GetTraderlist(pageNo, pageSize, sortColumn, sortDir, Search);
            if (traderList != null && traderList.Count > 0)
            {
                DataSourceResult obj = new DataSourceResult();
                obj.Data = traderList;
                if (traderList.Count > 0)
                {
                    TraderModel traderData = traderList.FirstOrDefault();
                    if (traderData != null){
                        obj.Total = traderList.FirstOrDefault().TotalCount;
                    }
                    
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<TraderModel>(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 20 March 2018
        /// Get Trader details by TraderId
        /// </summary>
        /// <param name="TraderId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageTrader(long TraderId = 0)
        {
            TraderModel model = new TraderModel();
            TraderModel TraderModel = _traderService.GetTraderDetailsByTraderId(TraderId);
            if (TraderModel != null)
            {
                TraderModel.Password = Security.Decrypt(TraderModel.Password);
                model = TraderModel;
            }
            return View(model);
        }

        /// <summary>
        /// By Mayank
        /// 20 March 2018
        /// Save Trader Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ManageTrader(TraderModel model, FormCollection formdata)
        {
            if(model!=null && model.ImageFile!=null)
            {
                string FileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                string FileExtension = Path.GetExtension(model.ImageFile.FileName);
                Guid guidFile = Guid.NewGuid();
                FileName = guidFile + "-" + FileName.Trim() + FileExtension;

                string UploadPath = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["UserImagePath"].ToString());

                if (!Directory.Exists(UploadPath))
                {
                    Directory.CreateDirectory(UploadPath);
                }

                model.ImageName = FileName;

                model.ImageFile.SaveAs(Path.Combine(UploadPath, Regex.Replace(System.Web.HttpUtility.UrlDecode(FileName), @"\s+", String.Empty)));
                
               
            }

            if (model != null && formdata["IsClose"] != null)
                model.IsClose = true;

            long result = _traderService.SaveTrader(model);

            switch (result)
            {
                case -2:
                    ErrorNotification("Email Address already exist.");
                    return RedirectToAction("ManageTrader", new { TraderId = model.TraderId });
                case 0:
                    ErrorNotification("Error Occur record not saved successfully");
                    return RedirectToAction("ManageTrader", new { TraderId = model.TraderId });
                default:
                    if (model.TraderId > 0)
                        SuccessNotification("Trader Updated Successfully");
                    else
                        SuccessNotification("Trader Saved Successfully");
                    break;
            }

            return RedirectToAction("Index", "Trader");
        }

        /// <summary>
        /// By Mayank
        /// 20 March 2018
        /// Delete Trader details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult DeleteTrader(long traderId)
        {
            int result = _traderService.DeleteTrader(traderId);

            switch (result)
            {
                case 0:
                    return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        
        #region Trader Commission

        /// <summary>
        /// By Mayank
        /// 22 March 2018
        /// Gets the list of Trader Commission
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetTraderCommissionList([DataSourceRequest]DataSourceRequest request, long TraderId = 0)
        {
            List<TraderCommissionModel> commList = _traderService.GetTraderCommissionList(TraderId);
            return Json(commList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// By Mayanks
        /// 22 March 2018
        /// Save Trader Commission details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SaveTraderCommission(TraderCommissionModel model)
        {
            int result = _traderService.SaveTraderCommission(model);
            if(result == -2)
            {
                return Json(new { Message = FXAdminTransferConnexResource.RangedExisted, IsError = Enums.NotifyType.Error }
                    , JsonRequestBehavior.AllowGet);
            }
            if (result == 0)
            {
                return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                    , JsonRequestBehavior.AllowGet);
            }
            return Json(new { Message = FXAdminTransferConnexResource.SaveSuccess, IsError = Enums.NotifyType.Success }                    
                    , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 22 March 2018
        /// Delete Trader Commission details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult DeleteTraderCommission([DataSourceRequest]DataSourceRequest request, TraderCommissionModel model)
        {
            bool result = _traderService.DeleteTraderCommission(model.TraderCommissionId);
            return Json(result
                ? new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }
                : new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                , JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}