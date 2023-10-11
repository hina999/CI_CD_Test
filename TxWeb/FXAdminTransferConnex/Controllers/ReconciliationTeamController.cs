using FXAdminTransferConnex.Business.ReconciliationTeam;
using FXAdminTransferConnex.Business.Trader;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnex.Entities.LocalizationResource;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FXAdminTransferConnexApp.Controllers
{
    public class ReconciliationTeamController : BaseAdminController
    {
        private readonly IReconciliationTeamService _teamService;
        private readonly ITraderService _traderService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ReconciliationTeamController()
        {
            _teamService = EngineContext.Resolve<IReconciliationTeamService>();
            _traderService = EngineContext.Resolve<ITraderService>();
        }

        // GET: ReconciliationManageTeam
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ManageReconciliationTeam([DataSourceRequest] DataSourceRequest request, long? TraderId, DateTime RecordDate)
        {
            ReconciliationTeamModel model = new ReconciliationTeamModel();
            List<ReconciliationTeamMemberModel> response = _teamService.GetReconciliationTeamData(TraderId, RecordDate);
            if (model.MemberList == null)
            {
                model.MemberList = new List<ReconciliationTeamMemberModel>();
            }
            if (response != null && response.Count > 0)
            {
                model.MemberList = response;
            }
            else
            {
                //var TraderModel = _traderService.GetTraderDetailsByTraderId(TraderId);
                List<UserModel> salesPErson = _teamService.GetSalesPersoListByTraderId(TraderId);
                //model.MemberList.Add(new ReconciliationTeamMemberModel()
                //{
                //    UserId = TraderModel.UserId,
                //    FullName = TraderModel.FirstName + " " + TraderModel.LastName
                //});
                foreach (UserModel item in salesPErson)
                {
                    model.MemberList.Add(new ReconciliationTeamMemberModel
                    {
                        UserId = item.UserId,
                        FullName = item.FullName
                    });
                }
            }
            model.TraderId = TraderId;
            model.RecordDate = RecordDate;
            return Json(model.MemberList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveReconciliationTeamData(string strData, string strRecordDate)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                IEnumerable<ReconciliationTeamMemberModel> teamMemberList = serializer.Deserialize<IEnumerable<ReconciliationTeamMemberModel>>(strData);

                DateTime RecordDate = DateTime.ParseExact(strRecordDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                DataTable dtMember = new DataTable("tt_ReconciliationTeam");
                dtMember.Columns.Add("UserId");
                dtMember.Columns.Add("LeadCount");
                dtMember.Columns.Add("RNCCount");
                dtMember.Columns.Add("REGCount");
                dtMember.Columns.Add("DingCount");
                dtMember.Columns.Add("Commission");

                foreach (ReconciliationTeamMemberModel objTeamMember in teamMemberList)
                {
                    DataRow dtrow = dtMember.NewRow();
                    dtrow["UserId"] = objTeamMember.UserId;
                    dtrow["LeadCount"] = objTeamMember.LeadCount;
                    dtrow["RNCCount"] = objTeamMember.RNCCount;
                    dtrow["REGCount"] = objTeamMember.REGCount;
                    dtrow["DingCount"] = objTeamMember.DingCount;
                    dtrow["Commission"] = objTeamMember.Commission;
                    dtMember.Rows.Add(dtrow);
                }

                _teamService.AddUpdateReconciliationTeamData(ProjectSession.LoginUserDetails.UserId, RecordDate, dtMember);

                return Json(new { Message = FXAdminTransferConnexResource.SaveSuccess, IsSuccess = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Message = FXAdminTransferConnexResource.Failed, IsSuccess = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ReconciliationTeamReport()
        {
            return View();
        }

        public PartialViewResult ReconciliationTeamReportData(string strFromDate, string strToDate)
        {
            DateTime? FromDate = null;
            DateTime? ToDate = null;

            if (!string.IsNullOrEmpty(strFromDate))
            {
                FromDate = DateTime.ParseExact(strFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(strToDate))
            {
                ToDate = DateTime.ParseExact(strToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            ReconciliationTeamModel model = new ReconciliationTeamModel();
            List<ReconciliationTeamMemberModel> response = _teamService.GetReconciliationTeamReport(FromDate, ToDate);
            model.MemberList = response;
            ViewData["ReconciliationTeamReport"] = model;
            return PartialView("~/Views/ReconciliationTeam/ReconciliationTeamReportData.cshtml");
        }
        public ActionResult TeamTopReport()
        {
            List<ReconciliationTeamMemberModel> response = new List<ReconciliationTeamMemberModel>();

            try
            {
                ReconciliationTeamModel modelmonth = new ReconciliationTeamModel();
                ReconciliationTeamModel modelyear = new ReconciliationTeamModel();
                response = _teamService.GetReconciliationTopReport();
                List<ReconciliationTeamMemberModel> Month = _teamService.GetReconciliationTeamCurrentMonthReport();
                List<ReconciliationTeamMemberModel> Year = _teamService.GetReconciliationTeamCurrentYearReport();
                modelmonth.MemberList = Month;
                modelyear.MemberList = Year;
                ViewData["ReconciliationTeamCurrentMonthReport"] = modelmonth;
                ViewData["ReconciliationTeamCurrentYearReport"] = modelyear;
            }
            catch (Exception ex)
            {
                _logger.Error("TeamTopReport: " + ex.Message);
            }

            return View("~/Views/ReconciliationTeam/TeamTopReport.cshtml", response);
        }

    }
}