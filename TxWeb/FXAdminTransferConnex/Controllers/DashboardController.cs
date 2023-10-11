using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FXAdminTransferConnex.Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using FXAdminTransferConnex.Business.Dashboard;
using FXAdminTransferConnex.Entities.LocalizationResource;
using FXAdminTransferConnex.Common;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FXAdminTransferConnexApp.Controllers
{
    public class DashboardController : BaseAdminController
    {
        #region Initializations

        /// <summary>
        /// The dashboardService service
        /// </summary>
        private readonly IDashboardService _dashboardService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        public DashboardController()
        {
            _dashboardService = EngineContext.Resolve<IDashboardService>();
        }
        #endregion

        // GET: Dashboard
        public ActionResult Home()
        {
            UserDashboardConfigurationModel model = new UserDashboardConfigurationModel();
            //model.listdata = _dashboardService.GetDashboardConfigurationData(ProjectSession.LoginUserDetails.UserId);
            return View(model);
        }


        public PartialViewResult ReconciliationTeamReportData()
        {
            ReconciliationTeamModel model = new ReconciliationTeamModel();
            List<ReconciliationTeamMemberModel> response = _dashboardService.GetReconciliationTeamCurrentMonthReport();
            List<IGrouping<string, ReconciliationTeamMemberModel>> fd = response.GroupBy(x => x.TraderName).ToList();
            model.MemberList1 = fd;
            ViewData["ReconciliationTeamReport"] = model;
            return PartialView("~/Views/Dashboard/_ReconciliationTeamReportData.cshtml");
        }

        /// <summary>
        /// By Mayank
        /// 27 March 2018
        /// Gets the list of 12 Months Commission
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLast12MonthsCommission()
        {
            long traderId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
            return Json(_dashboardService.GetLast12MonthsProfitList(ProjectSession.LoginUserDetails.UserId, traderId), JsonRequestBehavior.AllowGet);
        }
      
        /// <summary>
        /// Gets the list of 12 Months Company Commission
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLast12MonthsCompanyCommission()
        {
            return Json(_dashboardService.GetLast12MonthsCompanyCommission(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 27 March 2018
        /// Gets the list of Trade Commission
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTraderCommissions()
        {
            return Json(_dashboardService.GetTraderCommissionsList(ProjectSession.LoginUserDetails.UserId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 27 March 2018
        /// Gets the list of Sales Person Commision
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSalesPersonCommissions()
        {
            return Json(_dashboardService.GetSalesPersonCommissionsList(ProjectSession.LoginUserDetails.UserId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 28 March 2018
        /// Gets the list of To 10 Client Commission
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetTop10ClientCommissions([DataSourceRequest]DataSourceRequest request)
        {
            long traderId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
            long salespersonId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode() ? ProjectSession.LoginUserDetails.UserId : 0;

            List<Top10ClientCommissions> list = _dashboardService.GetTop10ClientCommissionsList(traderId, salespersonId);
            return Json(list.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 17 April 2018
        /// Gets the list of 30 Days Commission
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLast30DaysCommission()
        {
            long traderId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
            return Json(_dashboardService.GetLast30DaysProfitList(ProjectSession.LoginUserDetails.UserId, traderId), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Gets the list of 30 Days Company Commission
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLast30DaysCompanyCommission()
        {
            return Json(_dashboardService.GetLast30DaysCompanyCommission(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 17 April 2018
        /// Gets the list of 5 Days Commission
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLast5DaysCommission()
        {
            long traderId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
            return Json(_dashboardService.GetLast5DaysProfitList(ProjectSession.LoginUserDetails.UserId, traderId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the list of 5 Days Company Commission
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLast5DaysCompanyCommission()
        {
            return Json(_dashboardService.GetLast5DaysCompanyCommission(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 18 April 2018
        /// Gets the list of 6 Weeks Commission
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLast6WeeksCommission()
        {
            long traderId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
            return Json(_dashboardService.GetLast6WeeksCommissionList(ProjectSession.LoginUserDetails.UserId, traderId), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Gets the list of 6 Weeks Company Commission
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLast6WeeksCompanyCommission()
        {
            return Json(_dashboardService.GetLast6WeeksCompanyCommission(), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// By Mayank
        /// 27 March 2018
        /// Gets KAmount
        /// </summary>
        /// <param name="UserTypeId"></param>
        /// <returns></returns>
        public JsonResult gettotalkamount(int UserTypeId = 0)
        {
            decimal kamount;
            string amount = "";
         
            if (UserTypeId == SystemEnum.UserType.Trader.GetHashCode())
            {
                List<TraderCommissions> list = _dashboardService.GetTraderCommissionsList(ProjectSession.LoginUserDetails.UserId);
                kamount = list.Sum(item => item.Kamount);

                amount = string.Format("{0:n}", Convert.ToDouble(kamount));
                return Json(new { totalkamount = amount }, JsonRequestBehavior.AllowGet);
            }

            if (UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode())
            {
                List<SalesPersonCommissions> list = _dashboardService.GetSalesPersonCommissionsList(ProjectSession.LoginUserDetails.UserId);
                kamount = list.Sum(item => item.Kamount);
               
                amount = string.Format("{0:n}", Convert.ToDouble(kamount));
                return Json(new { totalkamount = amount }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { totalkamount = amount }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAwaitingActionCount()
        {
            try
            {
                List<ClientMasterModel> result = _dashboardService.GetAwaitingActionCount();
                int count = result.Count;
                return Json(count, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult GetMarketOrderCount()
        {
            try
            {
                long SalesPersonId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode() ? ProjectSession.LoginUserDetails.UserId : 0;
                //var SalesPersonId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
                int TraderId = 0;
                List<MarketOrderSettingModel> result = _dashboardService.GetMarketOrderCount(TraderId, SalesPersonId);
                int count = result.Count;
                return Json(count, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetTodayTaskRemiderPopUpList([DataSourceRequest]DataSourceRequest request, string SysDate = null)
        {
            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime dateTime1;
            if (DateTime.TryParseExact(SysDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime1))
            {

                if (ProjectSession.LoginUserDetails == null)
                    return Json(new List<TaskReminderModel>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                List<TaskReminderModel> list = _dashboardService.GetTodayPopupTaskRemider(dateTime1);
                list.ForEach(x =>
                {
                    //if (DateTime.Now.Date > x.Start.Date.Date)
                    //{
                    //    x.ThemeColor = "#e42307";
                    //}
                    //else
                    //{
                    //    x.ThemeColor = "green";
                    //}
                    if (x.ClientId > 0)
                    {
                       // x.ThemeColor = "#E42307";
                        switch (x.ClientLeadCategoryId)
                        {
                            case 1:
                                // Lead
                                x.ThemeColor = "#0000FF";
                                break;
                            case 2:
                                // Hot Lead
                                x.ThemeColor = "#FFA500";
                                break;
                            case 3:
                                // Registered Client
                                x.ThemeColor = "#008000";
                                break;
                            case 4:
                                // Need Documents
                                x.ThemeColor = "#800080";
                                break;
                            case 5:
                                // DING
                                x.ThemeColor = "#E42307";
                                break;
                            case 6:
                                //Sales Lead
                                x.ThemeColor = "#fe2469";
                                break;
                            default:
                                // code block
                                x.ThemeColor = "#E42307";
                                break;
                        }
                    }
                    if (x.ProspectId > 0)
                    {
                        switch (x.ProspectLeadCategoryId)
                        {
                            case 1:
                                // Lead
                                x.ThemeColor = "#0000FF";
                                break;
                            case 2:
                                // Hot Lead
                                x.ThemeColor = "#FFA500";
                                break;
                            case 3:
                                // Registered Client
                                x.ThemeColor = "#008000";
                                break;
                            case 4:
                                // Need Documents
                                x.ThemeColor = "#800080";
                                break;
                            case 5:
                                // DING
                                x.ThemeColor = "#E42307";
                                break;
                            case 6:
                                //Sales Lead
                                x.ThemeColor = "#fe2469";
                                break;
                            default:
                            // code block
                                x.ThemeColor = "#008000";
                               break;
                        }
                    }

                });
                 List<TaskReminderModel> descList = list.OrderByDescending(x => x.TaskReminderDatetime).ToList();
                DataSourceResult DataSourcelist = descList.ToDataSourceResult(request);
                

                if (DataSourcelist != null)
                    return Json(DataSourcelist, JsonRequestBehavior.AllowGet);
                else
                    return Json(new List<TaskReminderModel>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
               
            }
            return Json(new List<TaskReminderModel>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTaskRemiderPopUpList([DataSourceRequest]DataSourceRequest request)
        {
            if (ProjectSession.LoginUserDetails == null)
            {
                return Json(new List<TaskReminderModel>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }

            DataSourceResult list = _dashboardService.GetPopupTaskRemider().ToDataSourceResult(request);

            if (list != null)
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new List<TaskReminderModel>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult TaskSnooze(int TaskId, string SysDate = null)
        {
            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime dateTime1;
            if (DateTime.TryParseExact(SysDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime1))
            {
                int IsSuccess = _dashboardService.TaskSnooze(TaskId, dateTime1);
                return new JsonResult { Data = IsSuccess, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = 0, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult TaskSnoozeAll(string SysDate = null)
        {
            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime dateTime1;
            if (DateTime.TryParseExact(SysDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime1))
            {
                int result = _dashboardService.TaskSnoozeAll(ProjectSession.LoginUserDetails.UserId, dateTime1);

                bool IsSuccess = false;

                if (result >= 1)
                {
                    IsSuccess = true;
                }

                return new JsonResult { Data = IsSuccess, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = 0, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult TaskDismiss(int TaskId)
        {
            int IsSuccess = _dashboardService.TaskDismiss(TaskId);
            return new JsonResult { Data = IsSuccess, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult RescheduleTaskReminder([DataSourceRequest]DataSourceRequest request, DashboardTaskReminderModel obj, string TaskDateTime)
        {
            string format = "dd/MM/yyyy HH:mm";
            DateTime dateTime1;
            if (DateTime.TryParseExact(TaskDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime1))
            {
                obj.TaskReminderDatetime = dateTime1;
                obj.TaskReminderDatetimeString = TaskDateTime;
                int result = _dashboardService.RescheduleTaskReminder(obj);

                switch (result)
                {
                    case -1:
                        return Json(new { Message = "Record already exist", IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                    case 0:
                        return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                    default:
                        return Json(new { Message = FXAdminTransferConnexResource.SaveSuccess, IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetDetailedExchangeRate(string buy_currency, string sell_currency, string amount, string fixed_side)
        {
            await CommonController.GetCurrencyCloudToken();
            string uriRates = "rates/detailed?buy_currency=" + buy_currency + "&sell_currency=" + sell_currency + "&amount=" + amount + "&fixed_side=" + fixed_side;

            object resultTransactions = await WebApiHelper.HttpClientRequestResponseString(uriRates, ProjectSession.CurrencyCloudToken);

            if (resultTransactions != null)
            {
                string strResult = resultTransactions.ToString();
                return new JsonResult { Data = strResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #region Market Order Notification
        public ActionResult GetMarketOrderNotificationList([DataSourceRequest]DataSourceRequest request)
        {
            if (ProjectSession.LoginUserDetails == null)
            {
                return Json(new List<MarketValueNotificationModel>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            long SalesPersonId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode() ? ProjectSession.LoginUserDetails.UserId : 0;
            //var TraderId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
            int TraderId = 0;

            DataSourceResult list = _dashboardService.GetMarketOrderNotificationList(TraderId, SalesPersonId).ToDataSourceResult(request);

            if (list != null)
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new List<MarketValueNotificationModel>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult MarketOrderNotificationUpdate(long NotificationId)
        {

            bool IsSuccess = _dashboardService.MarketOrderNotificationUpdate(NotificationId);
            return new JsonResult { Data = IsSuccess, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        #endregion

        #region RingCentral
        public JsonResult GetLast5DayInCallLog()
        {
            return Json(_dashboardService.GetLast5DayInCallLog());
        }

        public JsonResult GetLast5DayOutCallLog()
        {
            return Json(_dashboardService.GetLast5DayOutCallLog());
        }

        public JsonResult GetYesterdaysOutCallLog()
        {
            return Json(_dashboardService.GetYesterdaysOutCallLog());
        }

        public JsonResult GetYesterdaysInCallLog()
        {
            return Json(_dashboardService.GetYesterdaysInCallLog());
        }
        #endregion

        public JsonResult GetSearchData(string searchString)
        {
            JsonResult result = Json(_dashboardService.GetGlobalSearchData(searchString), JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
    }
}