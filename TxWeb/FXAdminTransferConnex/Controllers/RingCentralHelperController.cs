using FXAdminTransferConnex.Business.ReconciliationTeam;
using FXAdminTransferConnex.Business.Ringcentral;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnexApp.Common;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FXAdminTransferConnexApp.Controllers
{
    public class RingCentralHelperController : BaseAdminController
    {
        private readonly IRingcentralService _ringcentralService;
        private readonly IReconciliationTeamService _teamService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
       

        public RingCentralHelperController()
        {
            _teamService = EngineContext.Resolve<IReconciliationTeamService>();
            _ringcentralService = EngineContext.Resolve<IRingcentralService>();
        }

        // GET: RingCentralHelper
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetRingcentralCallList([DataSourceRequest]DataSourceRequest request, DateTime? FromDate, DateTime? ToDate, string MobileNo, string Name, string CallDirection)
        {
            int pageNo = request.Page;
            int pageSize = request.PageSize;

            List<RingCentralModel> result = _ringcentralService.GetRingcentralCallList(FromDate, ToDate, MobileNo, Name, CallDirection, pageNo, pageSize);

            if (result != null && result.Count > 0)
            {
                DataSourceResult obj = new DataSourceResult();
                obj.Data = result;
                if (result.Count > 0)
                {
                    RingCentralModel ringCentral = result.FirstOrDefault();
                    if(ringCentral != null)
                    {
                        obj.Total = result.FirstOrDefault().TotalCount;
                    }
                    
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }

            return Json(new List<RingCentralModel>(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RingcentralCharts()
        {
            try
            {
                ReconciliationTeamModel modelmonth = new ReconciliationTeamModel();
                List<ReconciliationTeamMemberModel> Month = _teamService.GetReconciliationTeamCurrentMonthReport();
                modelmonth.MemberList = Month;
                ViewData["ReconciliationTeamCurrentMonthReport"] = modelmonth;
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return View();
        }

        public ActionResult GetEmployeeWiseCallCount([DataSourceRequest]DataSourceRequest request)
        {
            List<RingCentralModel> result = _ringcentralService.GetEmployeeWiseCallCount();
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllCounts()
        {
            decimal TotalCommission = 0;int InboundCount = 0; int OutboundCount = 0;
            List<RingCentralModel> RingCentralTodaysCommission = _ringcentralService.GetTodaysCommission();
            if(RingCentralTodaysCommission != null && RingCentralTodaysCommission.Count > 0)
            {
                RingCentralModel dataRingCentral = RingCentralTodaysCommission.FirstOrDefault();
                if(dataRingCentral != null)
                {
                    TotalCommission = dataRingCentral.TotalCommission;
                }
                
            }
            List<RingCentralModel> RingCentralInboundCount = _ringcentralService.GetInboundCount();
            if(RingCentralInboundCount != null && RingCentralInboundCount.Count > 0)
            {
                RingCentralModel dataRingCentral = RingCentralInboundCount.FirstOrDefault();
                if(dataRingCentral != null)
                {
                    InboundCount = RingCentralInboundCount.FirstOrDefault().TotalInCount;
                }
                
            }

            List<RingCentralModel> RingCentralOutboundCount = _ringcentralService.GetOutboundCount();
            if (RingCentralOutboundCount != null && RingCentralOutboundCount.Count > 0)
            {
                RingCentralModel dataRingCentral = RingCentralOutboundCount.FirstOrDefault();
                if(dataRingCentral != null)
                {
                    OutboundCount = RingCentralOutboundCount.FirstOrDefault().TotalOutCount;
                }
                
            }
            RingCentralModel callPercentageData = _ringcentralService.GetCallAcceptancePercentage().FirstOrDefault();
            int MissedCallCount = 0;
            double AcceptancePercentage = 0;
            if (callPercentageData != null)
            {
                MissedCallCount = callPercentageData.MissedCallCount;
                AcceptancePercentage = callPercentageData.AcceptancePercentage;
            }
            List<RingCentralModel> Top3Caller = _ringcentralService.GetTop3Caller();
            return Json(new { TotalCommission, InboundCount, OutboundCount, MissedCallCount, AcceptancePercentage, Top3Caller, Success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Last5DayInOutCallLog()
        {
            return Json(_ringcentralService.Last5DayInOutCallLog());
        }

        public ActionResult GetClientProspectCallLogList([DataSourceRequest] DataSourceRequest request, string MobileNo, string AltMobileNo)
        {
            List<RingCentralModel> result = _ringcentralService.GetClientProspectCallLogList(MobileNo, AltMobileNo);
            DataSourceResult obj = new DataSourceResult();
            obj.Data = result;
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public async System.Threading.Tasks.Task<ActionResult> GetCurrencyRateAsync()
        {
            CurrencyPairModel datatoReturn = new CurrencyPairModel();

            Type myType = datatoReturn.GetType();
            IList<PropertyInfo> Currencies = new List<PropertyInfo>(myType.GetProperties());
            await CommonController.GetCurrencyCloudToken();


            foreach (PropertyInfo CurrencyPair in Currencies)
            {
                string uriRates = "rates/find?currency_pair=" + CurrencyPair.Name;

                var resultTransactions = await WebApiHelper.HttpClientRequestResponseString(uriRates, ProjectSession.CurrencyCloudToken);

                dynamic data = JObject.Parse(resultTransactions.ToString());
                object Value1 = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)data).First).First).First).First).First).Value;
                object Value2 = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)data).First).First).First).First).Last).Value;

                decimal rate = (Convert.ToDecimal(Value1) + Convert.ToDecimal(Value2)) / 2;

                datatoReturn.GetType().GetProperty(CurrencyPair.Name).SetValue(datatoReturn, rate, null);

                switch (CurrencyPair.Name)
                {
                    case "GBPUSD":
                        datatoReturn.GBPUSD = rate;
                        break;
                    case "GBPEUR":
                        datatoReturn.GBPEUR = rate;
                        break;
                    case "EURUSD":
                        datatoReturn.EURUSD = rate;
                        break;
                    case "GBPCNY":
                        datatoReturn.GBPCNY = rate;
                        break;
                    case "GBPAUD":
                        datatoReturn.GBPAUD = rate;
                        break;
                    case "GBPCAD":
                        datatoReturn.GBPCAD = rate;
                        break;
                    case "GBPJPY":
                        datatoReturn.GBPJPY = rate;
                        break;
                    case "GBPNOK":
                        datatoReturn.GBPNOK = rate;
                        break;
                    case "GBPDKK":
                        datatoReturn.GBPDKK = rate;
                        break;
                    default:
                        break;
                }
            }
            return Json(new { data = datatoReturn, Success = true }, JsonRequestBehavior.AllowGet);
        }


        //added for custom import call log 



        public ActionResult RingcentralCallLogImportView()
        {
            return View("RingCentralCustomDownloadLog");
        }
     

        public async Task<ActionResult> ImportCallLogAsync(string FromDate, string ToDate)
        {
            WebApiHelper.WriteCallLogger("ImportCallLogAsync -> From Date: " + FromDate + "To Date: " + ToDate);

            DateTime fromdate = Convert.ToDateTime(FromDate);
            DateTime toDate = Convert.ToDateTime(ToDate);
            MarketValueNotification callLog = new MarketValueNotification();
             await callLog.GetRingCentralCallLogCustom(true, 5, fromdate, toDate);
            return Json(new { Message = "Imported Successfully", IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
           
        }


    }
}