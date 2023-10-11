using FXAdminTransferConnex.Business.Commission;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnex.Entities.LocalizationResource;
using Humanizer;
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
    public class CommissionController : BaseAdminController
    {
        #region Initializations

        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly ICommissionService _commissionService;

        /// <summary>
        /// </summary>
        /// 
        public CommissionController()
        {
            _commissionService = EngineContext.Resolve<ICommissionService>();
        }
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        // GET: Commission
        public ActionResult Index()
        {
            TargetCommissionModel model = new TargetCommissionModel();
            return View(model);
        }
        public ActionResult GetTargetCommissionList([DataSourceRequest]DataSourceRequest request)
        {
            List<TargetCommissionModel> targetCommissionList = _commissionService.GetTargetCommissionlist();
            return Json(targetCommissionList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ManageTargetCommission(long TargetCommissionId = 0)
        {
            TargetCommissionModel model = new TargetCommissionModel();

            if (TargetCommissionId > 0)
            {
                model = _commissionService.GetTargetCommissionById(TargetCommissionId);
            }
            else
            {
                model.TargetCommissionId = 0;
                model.ClientId = 0;
                model.TraderId = 0;
                model.SalesPersonId = 0;
            }
            return View(model);
        }


        public ActionResult ImportTargetCommission()
        {
            long result = _commissionService.ImportTargetCommisions(ProjectSession.LoginUserDetails.UserId);
            switch (result)
            {
                case 0:
                    return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { Message = "Records Imported Successfully.", IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult TargetDurationList([DataSourceRequest]DataSourceRequest request, int SelectedYear, string Type, long TargetCommissionId)
        {
            List<TargetDurationModel> durationList = new List<TargetDurationModel>();
            if (!string.IsNullOrEmpty(Type) && Type.ToLower() == "monthly")
            {
                List<TargetDurationModel> targetDurationList = _commissionService.GetMonthlyTargetCommissionById(TargetCommissionId, SelectedYear);

                durationList.Insert(0, new TargetDurationModel { Month = "January", TargetCommission = TargetCommissionId > 0 ? targetDurationList.Where(x => x.Month == "January")?.FirstOrDefault()?.TargetCommission : 0 });
                durationList.Insert(1, new TargetDurationModel { Month = "February", TargetCommission = TargetCommissionId > 0 ? targetDurationList.Where(x => x.Month == "February")?.FirstOrDefault()?.TargetCommission : 0 });
                durationList.Insert(2, new TargetDurationModel { Month = "March", TargetCommission = TargetCommissionId > 0 ? targetDurationList.Where(x => x.Month == "March")?.FirstOrDefault()?.TargetCommission : 0 });
                durationList.Insert(3, new TargetDurationModel { Month = "April", TargetCommission = TargetCommissionId > 0 ? targetDurationList.Where(x => x.Month == "April")?.FirstOrDefault()?.TargetCommission : 0 });
                durationList.Insert(4, new TargetDurationModel { Month = "May", TargetCommission = TargetCommissionId > 0 ? targetDurationList.Where(x => x.Month == "May")?.FirstOrDefault()?.TargetCommission : 0 });
                durationList.Insert(5, new TargetDurationModel { Month = "June", TargetCommission = TargetCommissionId > 0 ? targetDurationList.Where(x => x.Month == "June")?.FirstOrDefault()?.TargetCommission : 0 });
                durationList.Insert(6, new TargetDurationModel { Month = "July", TargetCommission = TargetCommissionId > 0 ? targetDurationList.Where(x => x.Month == "July")?.FirstOrDefault()?.TargetCommission : 0 });
                durationList.Insert(7, new TargetDurationModel { Month = "August", TargetCommission = TargetCommissionId > 0 ? targetDurationList.Where(x => x.Month == "August")?.FirstOrDefault()?.TargetCommission : 0 });
                durationList.Insert(8, new TargetDurationModel { Month = "September", TargetCommission = TargetCommissionId > 0 ? targetDurationList.Where(x => x.Month == "September")?.FirstOrDefault()?.TargetCommission : 0 });
                durationList.Insert(9, new TargetDurationModel { Month = "October", TargetCommission = TargetCommissionId > 0 ? targetDurationList.Where(x => x.Month == "October")?.FirstOrDefault()?.TargetCommission : 0 });
                durationList.Insert(10, new TargetDurationModel { Month = "November", TargetCommission = TargetCommissionId > 0 ? targetDurationList.Where(x => x.Month == "November")?.FirstOrDefault()?.TargetCommission : 0 });
                durationList.Insert(11, new TargetDurationModel { Month = "December", TargetCommission = TargetCommissionId > 0 ? targetDurationList.Where(x => x.Month == "December")?.FirstOrDefault()?.TargetCommission : 0 });
            }
            else if (!string.IsNullOrEmpty(Type) && Type.ToLower() == "weekly")
            {
                List<TargetDurationModel> targetDurationList = new List<TargetDurationModel>();
                if (TargetCommissionId > 0)
                {
                    targetDurationList = _commissionService.GetWeeklyTargetCommissionById(TargetCommissionId, SelectedYear);
                }
                DateTime jan1 = new DateTime(SelectedYear, 1, 1);
                DateTime startOfFirstWeek = jan1.AddDays(1 - (int)(jan1.DayOfWeek));
                var weeks =
                    Enumerable
                        .Range(0, 54)
                        .Select(i => new
                        {
                            weekStart = startOfFirstWeek.AddDays(i * 7),
                            TargetCommission = (TargetCommissionId > 0 && targetDurationList != null) ? targetDurationList.Where(m => m.weekNum == i)?.FirstOrDefault()?.TargetCommission : 0
                        })
                        .TakeWhile(x => x.weekStart.Year <= jan1.Year)
                        .Select(x => new
                        {
                            x.weekStart,
                            x.TargetCommission,
                            weekFinish = x.weekStart.AddDays(4)
                        })
                        .SkipWhile(x => x.weekFinish < jan1.AddDays(1))
                        .Select((x, i) => new
                        {
                            x.weekStart,
                            x.weekFinish,
                            weekNum = i + 1,
                            x.TargetCommission
                        });


                return Json(weeks.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
            else if (!string.IsNullOrEmpty(Type) && Type.ToLower() == "daily")
            {
                List<TargetDurationModel> targetDurationList = new List<TargetDurationModel>();

                List<TargetDurationModel> daysList = new List<TargetDurationModel>();
                List<DateTime> dateList = new List<DateTime>();

                if (TargetCommissionId > 0)
                {
                    targetDurationList = _commissionService.GetDailyTargetCommissionById(TargetCommissionId, SelectedYear);
                }

                for (DateTime date = new DateTime(SelectedYear, 1, 1); date <= new DateTime(SelectedYear, 12, 31); date = date.AddDays(1))
                    dateList.Add(date);
                for (int i = 0; i < dateList.Count; i++)
                {
                    daysList.Insert(i,
                        new TargetDurationModel
                        {
                            DTDay = dateList[i].Date,
                            StrDay = dateList[i].Date.ToString("dddd"),
                            TargetCommission = TargetCommissionId > 0 ? targetDurationList.Where(x => x.DTDay == dateList[i].Date)?.FirstOrDefault()?.TargetCommission : 0
                        });
                }
                if (daysList != null && daysList.Count > 0)
                {
                    daysList = daysList.Where(x => x.StrDay != "Saturday" && x.StrDay != "Sunday").ToList();
                }
                return Json(daysList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
            return Json(durationList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);



        }


        [HttpPost]
        public ActionResult ManageTargetCommission(long TargetCommissionId, string strData, string TargetType, string TargetFor, int TargetYear, long? TraderId, long? SalePersonId)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                IEnumerable<TargetDurationModel> targetDurationList = serializer.Deserialize<IEnumerable<TargetDurationModel>>(strData);

                foreach (TargetDurationModel item in targetDurationList)
                {
                    if (item.TargetCommission == null)
                    {
                        item.TargetCommission = 0;
                    }
                }

                DataTable dtTargetDuration = new DataTable("tt_TargetDurationList");
                dtTargetDuration.Columns.Add("Day");
                dtTargetDuration.Columns.Add("DayDate");
                dtTargetDuration.Columns.Add("weekNum");
                dtTargetDuration.Columns.Add("weekStart");
                dtTargetDuration.Columns.Add("weekFinish");
                dtTargetDuration.Columns.Add("Month");
                dtTargetDuration.Columns.Add("TargetCommission");

                foreach (TargetDurationModel obj in targetDurationList)
                {
                    DataRow dtrow = dtTargetDuration.NewRow();
                    dtrow["Day"] = obj.StrDay;
                    dtrow["DayDate"] =Convert.ToDateTime(obj.strDayDT);
                    dtrow["weekNum"] = obj.weekNum;
                    dtrow["weekStart"] = Convert.ToDateTime(obj.strweekStart);
                    dtrow["weekFinish"] = Convert.ToDateTime(obj.strweekFinish);
                    dtrow["Month"] = obj.Month;
                    dtrow["TargetCommission"] = obj.TargetCommission;
                    dtTargetDuration.Rows.Add(dtrow);
                }
                TargetCommissionModel model = new TargetCommissionModel();
                model.TargetCommissionId = TargetCommissionId;
                model.TraderId = TraderId;
                model.SalesPersonId = SalePersonId;
                model.TargetType = TargetType;
                model.TargetYear = TargetYear;
                model.LoggedinUserId = ProjectSession.LoginUserDetails.UserId;

                long result = _commissionService.SaveTargetCommison(model, dtTargetDuration);
                if (result > 0)
                {
                    return Json(new { Message = FXAdminTransferConnexResource.SaveSuccess, IsSuccess = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = FXAdminTransferConnexResource.Failed, IsSuccess = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Json(new { Message = FXAdminTransferConnexResource.Failed, IsSuccess = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteTargetCommission(long targetCommissionId)
        {
            long result = _commissionService.DeleteTargetCommission(targetCommissionId);           
            switch (result)
            {
                case 0:
                    return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ActivateDeActivateCommision(long targetCommissionId)
        {
            long result = _commissionService.ActivateDeactivateTargetCommission(targetCommissionId);
            switch (result)
            {
                case 0:
                    return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { Message = FXAdminTransferConnexResource.SaveSuccess, IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}