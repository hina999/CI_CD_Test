using FXAdminTransferConnex.Business.Deal;
using FXAdminTransferConnex.Business.Report;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FXAdminTransferConnexApp.Controllers
{
    public class ReportController : BaseAdminController
    {

        #region Initializations
        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly IReportService _ReportService;
        private readonly IDealService _DealService;

        public Dictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DealController"/> class.
        /// </summary>
        public ReportController()
        {
            _ReportService = EngineContext.Resolve<IReportService>();
            _DealService = EngineContext.Resolve<IDealService>();
        }
        #endregion

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClientReproductionReport()
        {
            //ViewData["Month"] = 2019;
            return View();
        }

        /// <summary>
        /// By Mayank
        /// 09 Nov 2017
        /// Gets the list of Client Reproduction Report
        /// </summary>
        /// <returns></returns>
        public ActionResult GetClientReproductionReportList([DataSourceRequest] DataSourceRequest request, string ClientName, string CompanyName, int Month, int Year, decimal? fromReductionRate = null, decimal? toReductionRate = null, long SalesPersonId = 0, long? JuniorSalesPersonId = 0, long TraderId = 0)
        {
            string Col1Name = GetColumnName(Year, Month, 6);
            string Col2Name = GetColumnName(Year, Month, 5);
            string Col3Name = GetColumnName(Year, Month, 4);
            string Col4Name = GetColumnName(Year, Month, 3);
            string Col5Name = GetColumnName(Year, Month, 2);
            string Col6Name = GetColumnName(Year, Month, 1);

            if (SalesPersonId == 0)
            {
                SalesPersonId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode() ? ProjectSession.LoginUserDetails.UserId : 0;
            }

            //if (TraderId == 0)
            //{
            //    TraderId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
            //}

            List<ClientReproductionReport> List = _ReportService.GetClientReproductionList(Month, Year, ClientName, CompanyName, TraderId, SalesPersonId,JuniorSalesPersonId);

            foreach (ClientReproductionReport item in List)
            {

                item.Avg = Convert.ToDecimal((Convert.ToDouble(item.col1) + Convert.ToDouble(item.col2) + Convert.ToDouble(item.col3) + Convert.ToDouble(item.col4) + Convert.ToDouble(item.col5) + Convert.ToDouble(item.col6)) / 6);
                if (item.col6 == 0)
                {
                    item.ReductionChange = 0;
                }
                else
                {
                    item.ReductionChange = Convert.ToDecimal(100 - ((Convert.ToDouble(item.col6) * 100) / Convert.ToDouble(item.Avg)));
                }


            }

            if (fromReductionRate != null)
            {
                List = List.Where(x => Convert.ToDecimal(string.Format("{0:0.00}", x.ReductionChange)) >= fromReductionRate).ToList();
            }
            if (toReductionRate != null)
            {
                List = List.Where(x => Convert.ToDecimal(string.Format("{0:0.00}", x.ReductionChange)) <= toReductionRate).ToList();
            }


            return Json(List.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        public ActionResult ClientRevenueReport()
        {
            //ViewData["Month"] = 2019;
            return View();
        }
        public ActionResult NewClientRevenueReport()
        {
            return View();
        }

        /// <summary>
        /// By Mayank
        /// 09 Nov 2017
        /// Gets the list of Client Revenue Report
        /// </summary>
        /// <returns></returns>
        public ActionResult GetClientRevenueReportList([DataSourceRequest] DataSourceRequest request, string ClientName, string CompanyName, int Month = 0, int Year = 0, decimal? fromReductionRate = null, decimal? toReductionRate = null, long SalesPersonId = 0, long? JuniorSalesPersonId = 0, long TraderId = 0)
        {
            string Col1Name = GetColumnName(Year, Month, 6);
            string Col2Name = GetColumnName(Year, Month, 5);
            string Col3Name = GetColumnName(Year, Month, 4);
            string Col4Name = GetColumnName(Year, Month, 3);
            string Col5Name = GetColumnName(Year, Month, 2);
            string Col6Name = GetColumnName(Year, Month, 1);

            if (SalesPersonId == 0)
            {
                SalesPersonId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode() ? ProjectSession.LoginUserDetails.UserId : 0;
            }

            //if (TraderId == 0)
            //{
            //    TraderId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
            //}

            List<ClientRevenueReport> List = _ReportService.GetClientRevenueList(Month, Year, ClientName, CompanyName, TraderId, SalesPersonId,JuniorSalesPersonId);

            foreach (ClientRevenueReport item in List)
            {
                item.Avg = Convert.ToDecimal((Convert.ToDouble(item.col1) + Convert.ToDouble(item.col2) + Convert.ToDouble(item.col3) + Convert.ToDouble(item.col4) + Convert.ToDouble(item.col5) + Convert.ToDouble(item.col6)) / 6);
                if (item.col6 == 0)
                    item.ReductionChange = 0;
                else
                    item.ReductionChange = Convert.ToDecimal(100 - ((Convert.ToDouble(item.col6) * 100) / Convert.ToDouble(item.Avg)));
            }

            if (fromReductionRate != null)
            {
                List = List.Where(x => Convert.ToDecimal(string.Format("{0:0.00}", x.ReductionChange)) >= fromReductionRate).ToList();
            }
            if (toReductionRate != null)
            {
                List = List.Where(x => Convert.ToDecimal(string.Format("{0:0.00}", x.ReductionChange)) <= toReductionRate).ToList();
            }
            return Json(List.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetNewClientRevenueReportList_TradeDate([DataSourceRequest] DataSourceRequest request, string ClientName, string CompanyName, int Month = 0, int Year = 0, long SalesPersonId = 0, long? JuniorSalesPersonId = 0, long TraderId = 0)
        {
            if (SalesPersonId == 0)
            {
                SalesPersonId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode() ? ProjectSession.LoginUserDetails.UserId : 0;
            }

            //if (TraderId == 0)
            //{
            //    TraderId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
            //}

            List<ClientRevenueReport> List = _ReportService.GetNewClientRevenueList(Month, Year, ClientName, CompanyName, TraderId, SalesPersonId, JuniorSalesPersonId);

            foreach (ClientRevenueReport item in List)
            {
                item.Avg = Convert.ToDecimal((Convert.ToDouble(item.col1) + Convert.ToDouble(item.col2) + Convert.ToDouble(item.col3) + Convert.ToDouble(item.col4) + Convert.ToDouble(item.col5) + Convert.ToDouble(item.col6)) / 6);
                if (item.col6 == 0)
                    item.ReductionChange = 0;
                else
                    item.ReductionChange = Convert.ToDecimal(100 - ((Convert.ToDouble(item.col6) * 100) / Convert.ToDouble(item.Avg)));
            }
            foreach (ClientRevenueReport item in List)
            {
                item.Total = Convert.ToDecimal((Convert.ToDouble(item.col1) + Convert.ToDouble(item.col2) + Convert.ToDouble(item.col3) + Convert.ToDouble(item.col4) + Convert.ToDouble(item.col5) + Convert.ToDouble(item.col6)));
               
            }


            return Json(List.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetNewClientRevenueReportList_ValueDate([DataSourceRequest] DataSourceRequest request, string ClientName, string CompanyName, int Month = 0, int Year = 0, long SalesPersonId = 0, long? JuniorSalesPersonId = 0, long TraderId = 0)
        {
            if (SalesPersonId == 0)
            {
                SalesPersonId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode() ? ProjectSession.LoginUserDetails.UserId : 0;
            }

            //if (TraderId == 0)
            //{
            //    TraderId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
            //}

            List<ClientRevenueReport> List = _ReportService.GetNewClientRevenueList_settledfigures(Month, Year, ClientName, CompanyName, TraderId, SalesPersonId,JuniorSalesPersonId);

            foreach (ClientRevenueReport item in List)
            {
                item.Avg = Convert.ToDecimal((Convert.ToDouble(item.col1) + Convert.ToDouble(item.col2) + Convert.ToDouble(item.col3) + Convert.ToDouble(item.col4) + Convert.ToDouble(item.col5) + Convert.ToDouble(item.col6)) / 6);
                if (item.col6 == 0)
                    item.ReductionChange = 0;
                else
                    item.ReductionChange = Convert.ToDecimal(100 - ((Convert.ToDouble(item.col6) * 100) / Convert.ToDouble(item.Avg)));
            }
            foreach (ClientRevenueReport item in List)
            {
                item.Total = Convert.ToDecimal((Convert.ToDouble(item.col1) + Convert.ToDouble(item.col2) + Convert.ToDouble(item.col3) + Convert.ToDouble(item.col4) + Convert.ToDouble(item.col5) + Convert.ToDouble(item.col6)));
            }


            return Json(List.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetColumnNameList(int Year = 0, int Month = 0)
        {
            List<string> myCollection = new List<string>();
            myCollection.Add(GetColumnName(Year, Month, 6));
            myCollection.Add(GetColumnName(Year, Month, 5));
            myCollection.Add(GetColumnName(Year, Month, 4));
            myCollection.Add(GetColumnName(Year, Month, 3));
            myCollection.Add(GetColumnName(Year, Month, 2));
            myCollection.Add(GetColumnName(Year, Month, 1));


            return Json(myCollection, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNewColumnNameList(int Year = 0, int Month = 0)
        {
            List<string> myCollection = new List<string>();
            myCollection.Add(GetColumnName(Year, Month, 7));
            myCollection.Add(GetColumnName(Year, Month, 6));
            myCollection.Add(GetColumnName(Year, Month, 5));
            myCollection.Add(GetColumnName(Year, Month, 4));
            myCollection.Add(GetColumnName(Year, Month, 3));
            myCollection.Add(GetColumnName(Year, Month, 2));
            myCollection.Add(GetColumnName(Year, Month, 1));


            return Json(myCollection, JsonRequestBehavior.AllowGet);
        }

        public string GetColumnName(int Year, int Month, int val = 0)
        {
            DateTime selecteddate = new DateTime(Year, Month, 1);

            string strMonth;
            switch (val)
            {
                case 1:
                    strMonth = selecteddate.ToString("MMM yy");
                    break;
                case 2:
                    strMonth = selecteddate.AddMonths(-1).ToString("MMM yy");
                    break;
                case 3:
                    strMonth = selecteddate.AddMonths(-2).ToString("MMM yy");
                    break;
                case 4:
                    strMonth = selecteddate.AddMonths(-3).ToString("MMM yy");
                    break;
                case 5:
                    strMonth = selecteddate.AddMonths(-4).ToString("MMM yy");
                    break;
                case 6:
                    strMonth = selecteddate.AddMonths(-5).ToString("MMM yy");
                    break;
                case 7:
                    strMonth = selecteddate.AddMonths(-6).ToString("MMM yy");
                    break;
                default:
                    strMonth = "undefined column";
                    break;
            }
            return strMonth;
        }


        public ActionResult CurrencyCloudClientReport()
        {
            return View();
        }

        public ActionResult GetCurrencyCloudClientList([DataSourceRequest] DataSourceRequest request, string ClientName, string CompanyName, string AccountNo, DateTime? NoTradeFromDate = null, DateTime? NoTradeToDate = null, long SalesPersonId = 0, long? JuniorSalesPersonId = 0, long TraderId = 0)
        {
            List<CurrencyCloudReportModel> clientDealList = _ReportService.GetCurrencyCloudClientList(ClientName, CompanyName, AccountNo, NoTradeFromDate, NoTradeToDate, TraderId, SalesPersonId,JuniorSalesPersonId);
            return Json(clientDealList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// By Mayank
        /// 29 Nov 2018
        /// Currency cloud API Integration
        /// </summary>
        public async Task<JsonResult> ImportCurrencyCloudClients()
        {
            try
            {
                //Generate Token
                await CommonController.GetCurrencyCloudToken();

                int ImportClientCount = 0;
                int ExistClientCount = 0;

                string ClientId = "";

                CurrencyCloudContactModel resultContactsAPI = new CurrencyCloudContactModel();
                List<ContactModel> lstcontact = new List<ContactModel>();

                try
                {
                    List<ClientMasterModel> listContacts = _DealService.GetCC_Clientlist();

                    for (int page = 1; page < 100000000; page++)
                    {

                        //order = created_at & order_asc_desc = desc &
                        string uriContact = "contacts/find?page=" + page;
                        var resultContacts = await WebApiHelper.HttpClientRequestResponse(resultContactsAPI, uriContact, ProjectSession.CurrencyCloudToken);

                        if (resultContacts == null)
                        {
                            System.Threading.Thread.Sleep(5000);
                            resultContacts = await WebApiHelper.HttpClientRequestResponse(resultContactsAPI, uriContact, ProjectSession.CurrencyCloudToken);
                        }


                        if (resultContacts != null)
                        {
                            foreach (ContactModel contact in resultContacts.contacts)
                            {
                                if (contact.account_id != null)
                                {
                                    ClientMasterModel singlecontact = listContacts.FirstOrDefault(t => t.CCAccount_id == contact.account_id);
                                    //ClientMasterModel singlecontact = listContacts.Where(t => t.CCAccount_id == contact.account_id).FirstOrDefault();
                                    {
                                        if (singlecontact == null)
                                        {
                                            AccountModel resultAccountAPI = new AccountModel();
                                            string uriAccountClient = "accounts/" + Convert.ToString(contact.account_id);
                                            //ClientId = Convert.ToString(contact.account_id);
                                            AccountModel account = await WebApiHelper.HttpClientRequestResponse(resultAccountAPI, uriAccountClient, ProjectSession.CurrencyCloudToken);


                                            long result = 0;
                                            if (account != null)
                                            {

                                                result = _DealService.ImportClient(contact, account, "CurrencyCloud");

                                                if (result > 0)
                                                    ImportClientCount = ImportClientCount + 1;
                                                else if (result == -2)
                                                    ExistClientCount = ExistClientCount + 1;
                                            }
                                        }
                                        else
                                        {
                                            ExistClientCount = ExistClientCount + 1;
                                        }
                                    }
                                }
                            }
                        }

                        if (resultContacts != null && resultContacts.pagination.next_page == -1)
                            break;
                    }
                }
                catch (Exception exclient)
                {
                    throw exclient;
                }

                //Logout
                //await CommonController.GetCurrencyCloudTokenLogout();

                return Json(new { ImportClientCount = ImportClientCount, ExistClientCount = ExistClientCount }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region Margin Report

        public ActionResult ClientMarginReport()
        {
            return View();
        }

        public ActionResult GetClientMarginList([DataSourceRequest] DataSourceRequest request, string ClientName, string CompanyName, string AccountNo, long SalesPersonId = 0, long? JuniorSalesPersonId = 0, long TraderId = 0)
        {
            List<ClientMarginReportModel> clientMarginList = _ReportService.GetClientMarginList(ClientName, CompanyName, AccountNo, TraderId, SalesPersonId, JuniorSalesPersonId);
            return Json(clientMarginList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region P&L Report

        public ActionResult ProfitLossReport()
        {
            return View();
        }

        public ActionResult GetProfitLossReportList([DataSourceRequest] DataSourceRequest request, string ClientName, string CompanyName, string AccountNo, string DealNo, DateTime? FromDate = null, DateTime? ToDate = null, long SalesPersonId = 0, long? JuniorSalesPersonId = 0, long TraderId = 0)
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

            List<ProfitLossReportModel> clientProfitLossList = _ReportService.GetProfitLossReportList(pageNo, pageSize, sortColumn, sortDir, ClientName, CompanyName, AccountNo, DealNo, FromDate, ToDate, TraderId, SalesPersonId, JuniorSalesPersonId);
            if (clientProfitLossList != null && clientProfitLossList.Count > 0)
            {
                DataSourceResult obj = new DataSourceResult();
                obj.Data = clientProfitLossList;
                if (clientProfitLossList.Count > 0)
                {
                    if(clientProfitLossList.FirstOrDefault() != null)
                    {
                        obj.Total = clientProfitLossList.FirstOrDefault().TotalCount;
                    }
                    
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<ProfitLossReportModel>(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region New Clients Report

        public ActionResult NewClientsReport()
        {
            return View();
        }
        //[HttpPost]
        public ActionResult GetNewClientListbyId([DataSourceRequest] DataSourceRequest request, int? Month, int? Year, List<long> client_arr)
        {
            List<ClientMasterModel> clientList = _ReportService.GetClientListByClientId(client_arr);
            foreach (ClientMasterModel client in clientList)
            {
                if (client != null)
                {
                    NewClientReportModel NewClientsReport = _ReportService.GetNewIndividualClientsReportList(Month, Year, client.ClientId);
                    NewClientReportModel newclient = new NewClientReportModel();
                    if (NewClientsReport != null)
                    {
                        
                        newclient.Count30Day = NewClientsReport.Count30Day;
                        newclient.Count180Day = NewClientsReport.Count180Day;
                        newclient.Count1Year = NewClientsReport.Count1Year;
                        newclient.Avg180Day = NewClientsReport.Avg180Day;
                        newclient.Avg1Year = NewClientsReport.Avg1Year;
                        newclient.CountTillToday = NewClientsReport.CountTillToday;

                        if(NewClientsReport.TradeCountAfter6MO <= 0)
                        {
                            newclient.TradeCountAfter6MO = 1;
                           
                        }
                        else
                        {
                            newclient.TradeCountAfter6MO = 0;
                            
                        }
                        if(NewClientsReport.TradeCountAfter1Y <= 0)
                        {
                            newclient.TradeCountAfter1Y = 1;
                        }
                        else
                        {
                            newclient.TradeCountAfter1Y = 0;
                        }
                        client.newclientReport = newclient;
                    }
                    else
                    {
                        newclient.TradeCountAfter6MO = 1;
                        newclient.TradeCountAfter1Y = 1;
                        client.newclientReport = newclient;
                    }


                    if (client.TraderId != null)
                    {
                        TraderModel Trader = _ReportService.GetTraderDetailsByTraderId((long)client.TraderId);
                        client.TraderName = Trader.FirstName + " " + Trader.LastName;
                    }
                    else
                    {
                        client.TraderName = " ";
                    }
                    if (client.SalesPersonId != null)
                    {
                        UserModel SalesPerson = _ReportService.GetSalesPersonDetailsById((long)client.SalesPersonId);
                        client.SalesPersonName = SalesPerson.FirstName + " " + SalesPerson.LastName;
                    }
                    else
                    {
                        client.SalesPersonName = " ";
                    }

                }

            }

            DataSourceResult obj = new DataSourceResult();
            if(clientList[0] != null)
            {
                obj.Data = clientList;
            }
            else
            {
                obj.Data = new List<ClientMasterModel>();
            }
            return Json(obj.Data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNewClientsReportList(int? Month, int? Year, long TraderId, long SalesPersonId, long? JuniorSalesPersonId)
        {
            if (!Month.HasValue || Month <= 0)
            {
                Month = DateTime.Now.Month;
            }

            if (!Year.HasValue || Year <= 0)
            {
                Year = DateTime.Now.Year;
            }

            NewClientReportModel obj = new NewClientReportModel();
            List<NewClientReportModel> NewClientsReport = _ReportService.GetNewClientsReportList(Month, Year, TraderId, SalesPersonId,JuniorSalesPersonId);
            if (NewClientsReport != null && NewClientsReport.Count > 0)
            {
                obj.client_arr = NewClientsReport.Select(l => l.Client_Id).ToList();
                obj.Avg180Day = NewClientsReport.Sum(x => x.Avg180Day);
                obj.Avg1Year = NewClientsReport.Sum(x => x.Avg1Year);
                obj.AvgProfit180Day = NewClientsReport.Sum(x => x.AvgProfit180Day);
                obj.AvgProfit1Year = NewClientsReport.Sum(x => x.AvgProfit1Year);
                obj.ClientCount = NewClientsReport.Count;
                obj.Count180Day = NewClientsReport.Sum(x => x.Count180Day);
                obj.Count1Year = NewClientsReport.Sum(x => x.Count1Year);
                obj.Count30Day = NewClientsReport.Sum(x => x.Count30Day);
                obj.CountTillToday = NewClientsReport.Sum(x => x.CountTillToday);
                obj.Profit180Day = NewClientsReport.Sum(x => x.Profit180Day);
                obj.Profit1Year = NewClientsReport.Sum(x => x.Profit1Year);
                obj.Profit30Day = NewClientsReport.Sum(x => x.Profit30Day);
                obj.ProfitTillToday = NewClientsReport.Sum(x => x.ProfitTillToday);
                obj.TradeCountAfter1Y = NewClientsReport.Where(x => x.TradeCountAfter1Y <= 0).ToList().Count;
                obj.TradeCountAfter6MO = NewClientsReport.Where(x => x.TradeCountAfter6MO <= 0).ToList().Count;
                //obj.TradeCountAfter1Y = NewClientsReport.Sum(x => x.TradeCountAfter1Y);
                //obj.TradeCountAfter6MO = NewClientsReport.Sum(x => x.TradeCountAfter6MO);
            }

            return Json(new { Message = "", Success = true, Data = obj }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult VolumeReportByMonth()
        {
            return View();
        }
        public ActionResult VolumeReportByCategory()
        {
            return View();
        }
        public ActionResult GetVolumeReportByMonthList([DataSourceRequest] DataSourceRequest request, int? StartMonth, int? StartYear, int? EndMonth, int? EndYear)
        {
            if (!StartMonth.HasValue || StartMonth <= 0)
            {
                StartMonth = DateTime.Now.AddMonths(-5).Month;
            }
            if (!StartYear.HasValue || StartYear <= 0)
            {
                StartYear = DateTime.Now.AddMonths(-5).Year;
            }
            if (!EndMonth.HasValue || EndMonth <= 0)
            {
                EndMonth = DateTime.Now.Month;
            }
            if (!EndYear.HasValue || EndYear <= 0)
            {
                EndYear = DateTime.Now.Year;
            }

            List<VolumeReportByMonthodelMonthYear> clientDealList = _ReportService.GetVolumeReportByMonthList(StartMonth, StartYear, EndMonth, EndYear);
            foreach (VolumeReportByMonthodelMonthYear client in clientDealList)
            {
                client.AvgSpread = client.AvgSpread * 100;

            }
            return Json(clientDealList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVolumeReportByMonthListChart(int? StartMonth, int? StartYear, int? EndMonth, int? EndYear)
        {
            if (!StartMonth.HasValue || StartMonth <= 0)
            {
                StartMonth = DateTime.Now.AddMonths(-5).Month;
            }
            if (!StartYear.HasValue || StartYear <= 0)
            {
                StartYear = DateTime.Now.AddMonths(-5).Year;
            }
            if (!EndMonth.HasValue || EndMonth <= 0)
            {
                EndMonth = DateTime.Now.Month;
            }
            if (!EndYear.HasValue || EndYear <= 0)
            {
                EndYear = DateTime.Now.Year;
            }

            List<VolumeReportByMonthodelMonthYear> clientDealList = _ReportService.GetVolumeReportByMonthList(StartMonth, StartYear, EndMonth, EndYear);
            return Json(clientDealList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VolumeReportByTrader()
        {
            return View();
        }

        public ActionResult GetVolumeReportByTraderList([DataSourceRequest] DataSourceRequest request, int? Month, int? Year)
        {
            if (!Month.HasValue || Month <= 0)
            {
                Month = DateTime.Now.Month;
            }

            if (!Year.HasValue || Year <= 0)
            {
                Year = DateTime.Now.Year;
            }

            IOrderedEnumerable<VolumeReportByMonthodelMonthYear> clientDealList = _ReportService.GetVolumeReportByTraderList(Month, Year).OrderBy(x => x.TotalTrade);
            foreach (VolumeReportByMonthodelMonthYear client in clientDealList)
            {
                client.AvgSpread = client.AvgSpread * 100;

            }
            return Json(clientDealList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetVolumeReportByCategoryList([DataSourceRequest] DataSourceRequest request, int? Month, int? Year)
        {
            if (!Month.HasValue || Month < 0)
            {
                Month = DateTime.Now.Month;
            }

            if (!Year.HasValue || Year < 0)
            {
                Year = DateTime.Now.Year;
            }

            List<VolumeReportByMonthodelMonthYear> clientDealList = _ReportService.GetVolumeReportByCategoryList(Month, Year);
            foreach (VolumeReportByMonthodelMonthYear client in clientDealList)
            {
                client.AvgSpread = client.AvgSpread * 100;
            }
            return Json(clientDealList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClientListForDropdown()
        {
            List<ClientDDL> leadCategoryList = _ReportService.GetClientListForDropdown().ToList();
            return Json(leadCategoryList, JsonRequestBehavior.AllowGet);
        }
    }
}