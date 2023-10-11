using FXAdminTransferConnex.Business.Client;
using FXAdminTransferConnex.Business.Deal;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using System.Threading;

using System.IO;
using System.Data.OleDb;
using System.Data;
using Kendo.Mvc.Extensions;
using FXAdminTransferConnex.Data.Repository;
using FXAdminTransferConnex.Data.Models;
using FXAdminTransferConnex.Entities.LocalizationResource;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using log4net;
using System.Reflection;
using System.Text;

namespace FXAdminTransferConnexApp.Controllers
{
    public class DealController : BaseAdminController
    {
        #region Initializations

        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly IDealService _DealService;

        public Dictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DealController"/> class.
        /// </summary>
        public DealController()
        {
            _DealService = EngineContext.Resolve<IDealService>();
        }
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        // GET: Deal
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Get deal list
        /// </summary>
        /// <param name="request"></param>
        /// <param name="DealNo"></param>
        /// <param name="CompanyName"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public ActionResult GetDealList([DataSourceRequest] DataSourceRequest request, string DealNo, string CompanyName, DateTime? FromDate = null, DateTime? ToDate = null, long SalesPersonId = 0, long JuniorSalesPersonId = 0, long TraderId = 0, string TStatus = null, string Source = null)
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


            List<DealModel> userList = _DealService.GetDeallist(pageNo, pageSize, sortColumn, sortDir, DealNo, CompanyName, FromDate, ToDate, SalesPersonId, JuniorSalesPersonId, TraderId, TStatus, Source);

            if (userList != null && userList.Count > 0)
            {
                DataSourceResult obj = new DataSourceResult();
                obj.Data = userList;
                if (userList.Count > 0)
                {
                    obj.Total = userList.FirstOrDefault().TotalCount;
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<DealModel>(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Get Deal details by DealId
        /// </summary>
        /// <param name="DealId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageDeal(long DealId = 0)
        {
            DealModel model = new DealModel();
            DealModel DealModel = _DealService.GetDealDetailsByDealId(DealId);
            if (DealModel != null)
            {
                model = DealModel;
            }
            return View(model);
        }

        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Save deal Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ManageDeal(DealModel model)
        {
            int result = _DealService.SaveDeal(model);
            if (result > 0)
            {
                SuccessNotification("Record Saved Successfully");
            }
            else
            {
                ErrorNotification("Error Occur record not saved successfully");
            }

            return RedirectToAction("Index", "Deal");
        }

        /// <summary>
        /// By Mayank
        /// 29 Nov 2018
        /// Currency cloud API Integration
        /// </summary>
        public async Task<JsonResult> ImportClientDeals()
        {
            try
            {
                //Generate Token
                await CommonController.GetCurrencyCloudToken();

                int ImportClientCount = 0;
                int ExistClientCount = 0;

                CurrencyCloudContactModel resultContactsAPI = new CurrencyCloudContactModel();
                List<ContactModel> lstcontact = new List<ContactModel>();
                System.Text.StringBuilder logsb = new System.Text.StringBuilder();

                try
                {


                    for (int page = 1; page < 100000000; page++)
                    {
                        string uriContact = "contacts/find?page=" + page;
                        var resultContacts = await WebApiHelper.HttpClientRequestResponse(resultContactsAPI, uriContact, ProjectSession.CurrencyCloudToken);
                        lstcontact.AddRange(resultContacts.contacts);
                        if (resultContacts != null)
                        {
                            foreach (ContactModel contact in resultContacts.contacts)
                            {
                                AccountModel resultAccountAPI = new AccountModel();
                                string uriAccountClient = "accounts/" + Convert.ToString(contact.account_id);
                                AccountModel account = await WebApiHelper.HttpClientRequestResponse(resultAccountAPI, uriAccountClient, ProjectSession.CurrencyCloudToken);


                                long result = 0;
                                if (account != null)
                                {
                                    try
                                    {
                                        result = _DealService.ImportClient(contact, account, "CurrencyCloud");
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Error(ex.Message);
                                    }

                                    switch (result)
                                    {
                                        case 0:
                                            break;
                                        case 1:
                                            ImportClientCount = ImportClientCount + 1;
                                            break;
                                        case -2:
                                            ExistClientCount = ExistClientCount + 1;
                                            break;
                                    }
                                }

                            }
                        }

                        if (resultContacts.pagination.next_page == -1)
                            break;
                    }
                }
                catch (Exception exclient)
                {
                    throw new Exception("Exception", exclient);
                }


                int ImportDealCount = 0;
                int ExistDealCount = 0;

                try
                {

                    CurrencyCloudConversionModel resultTransactionAPI = new CurrencyCloudConversionModel();

                    for (int page = 1; page < 100000000; page++)
                    {
                        string uriTransaction = "conversions/find?scope=all&page=" + page;
                        var resultTransactions = await WebApiHelper.HttpClientRequestResponse(resultTransactionAPI, uriTransaction, ProjectSession.CurrencyCloudToken);

                        if (resultTransactions != null)
                        {
                            foreach (ConversionModel conversion in resultTransactions.conversions)
                            {
                                //string uricontact = "contacts/find?account_id=" + conversion.account_id;
                                IEnumerable<ContactModel> singlecontact = lstcontact.Where(t => t.account_id == conversion.account_id);
                                ContactModel contact = new ContactModel();
                                if (singlecontact != null)
                                {
                                    try
                                    {
                                        contact = singlecontact.FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Error(ex.Message);
                                    }

                                }

                                if (contact == null)
                                {
                                    logsb.AppendLine(conversion.account_id);
                                    continue;
                                }

                                string TradeType = string.Empty;
                                string ConverstatinStatus = Convert.ToString(conversion.status);

                                if (ConverstatinStatus == "awaiting_funds" || ConverstatinStatus == "funds_sent")
                                    TradeType = "Traded";
                                if (ConverstatinStatus == "funds_arrived" || ConverstatinStatus == "trade_settled" || ConverstatinStatus == "closed")
                                    TradeType = "Settled";


                                decimal clientsellamount = 0;
                                if (!string.IsNullOrEmpty(conversion.client_sell_amount))
                                {
                                    clientsellamount = Convert.ToDecimal(conversion.client_sell_amount);
                                }

                                decimal partnersellamount = 0;
                                if (!string.IsNullOrEmpty(conversion.partner_sell_amount))
                                {
                                    partnersellamount = Convert.ToDecimal(conversion.partner_sell_amount);
                                }

                                conversion.GrossCommGBP = clientsellamount - partnersellamount;
                                if (conversion.sell_currency != "GBP")
                                {

                                    decimal resultRate = await GetRates(conversion.sell_currency, "GBP");
                                    conversion.GBPConversationRate = resultRate.ToString();
                                    decimal x = resultRate * Convert.ToDecimal(conversion.client_sell_amount);

                                    //var x = Convert.ToDecimal(conversion.mid_market_rate) * Convert.ToDecimal(conversion.client_sell_amount);
                                    //conversion.client_sell_amount_In_GBP = (Convert.ToDecimal(conversion.mid_market_rate) * Convert.ToInt64(conversion.client_sell_amount)).ToString();
                                    conversion.client_sell_amount_In_GBP = Convert.ToString(x);
                                }
                                else
                                {
                                    conversion.client_sell_amount_In_GBP = conversion.client_sell_amount;
                                }
                                int result = 0;
                                try
                                {
                                    result = _DealService.ImportDealData(conversion, contact, TradeType, "CurrencyCloud");
                                }
                                catch (Exception ex)
                                {
                                    _logger.Error(ex.Message);
                                }

                                switch (result)
                                {
                                    case 0:
                                        break;
                                    case 1:
                                        ImportDealCount = ImportDealCount + 1;
                                        break;
                                    case -2:
                                        ExistDealCount = ExistDealCount + 1;
                                        break;
                                }
                            }
                        }

                        if (resultTransactions != null && resultTransactions.pagination.next_page == -1)
                            break;
                    }
                }
                catch (Exception)
                {
                    throw ;
                }


                //Logout
                //await CommonController.GetCurrencyCloudTokenLogout();

                return Json(new { ImportClientCount = ImportClientCount, ExistClientCount = ExistClientCount, ImportDealCount = ImportDealCount, ExistDealCount = ExistDealCount }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// By Mayank
        /// 29 Nov 2018
        /// Currency cloud API Integration
        /// </summary>
        [HttpPost]
        public async Task<JsonResult> ImportDeal(string FromDate, string ToDate, string ImportBy = "Manual")
        {
            int FailedDealCount = 0;
            int ImportDealCount = 0;
            int ExistDealCount = 0;
            StringBuilder DealShortReference = new StringBuilder();

            int ImportClientCount = 0;
            int ExistClientCount = 0;

            string short_reference = "";

            DateTime FromDate1;
            DateTime ToDate1;
            string format = "yyyy-MM-dd";

            if (DateTime.TryParseExact(FromDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate1))
            {

            }
            if (DateTime.TryParseExact(ToDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate1))
            {

            }

            try
            {
                if (ImportBy.ToLower().Contains("auto"))
                {
                    string result = _DealService.DatabaseBackup();
                    string DatabaseBackupLogPath = Server.MapPath("~/DatabaseBackupLog.txt");

                    if (!System.IO.File.Exists(DatabaseBackupLogPath))
                    {
                        System.IO.File.Create(DatabaseBackupLogPath).Dispose();
                    }
                    System.IO.File.AppendAllText(DatabaseBackupLogPath, "---------------------" + DateTime.Now.ToShortDateString() + "---------------------" + Environment.NewLine);
                    System.IO.File.AppendAllText(DatabaseBackupLogPath, result == string.Empty ? "DB Backup Success" : result + Environment.NewLine);
                    System.IO.File.AppendAllText(DatabaseBackupLogPath, "------------------------------------------" + Environment.NewLine);

                    if (ProjectSession.LoginUserDetails == null)
                    {
                        User.LoginUser model = new User.LoginUser();
                        model.UserId = 0;
                        ProjectSession.LoginUserDetails = model;
                    }
                }

                ////GC Partner Client and Deal Import
                long loggedUserId = ProjectSession.LoginUserDetails.UserId;
                int GCPartnerDealCount = await GCPartnerImportClient(FromDate, ToDate, ImportBy, loggedUserId); //GC Partner API

                ImportDealCount = ImportDealCount + GCPartnerDealCount;
                //Generate Token
                await CommonController.GetCurrencyCloudToken();

                exchangeRates.Clear();

                CurrencyCloudConversionModel resultTransactionAPI = new CurrencyCloudConversionModel();
                List<ClientMasterModel> listContacts = _DealService.GetCC_Clientlist();

                for (int page = 1; page < 100000000; page++)
                {

                    //string uriTransaction = "conversions/find?short_reference=20190802-RCKZCP&scope=all";
                    //string uriTransaction = "conversions/find?short_reference=20190729-PKTHKP&scope=all";
                    //string uriTransaction = "conversions/find?short_reference=20190722-RGCHVR&scope=all";
                    //string uriTransaction = "conversions/find?page=" + page + "&conversion_date_from=" + FromDate + "&conversion_date_to=" + ToDate + "&scope=all";
                    string uriTransaction = "conversions/find?page=" + page + "&created_at_from=" + FromDate + "&created_at_to=" + ToDate + "&scope=all";
                    var resultTransactions = await WebApiHelper.HttpClientRequestResponse(resultTransactionAPI, uriTransaction, ProjectSession.CurrencyCloudToken);

                    if (resultTransactions == null)
                    {
                        System.Threading.Thread.Sleep(5000);
                        resultTransactions = await WebApiHelper.HttpClientRequestResponse(resultTransactionAPI, uriTransaction, ProjectSession.CurrencyCloudToken);
                    }


                    if (resultTransactions != null)
                    {
                        foreach (ConversionModel conversion in resultTransactions.conversions)
                        {
                            ConversionModel originalDealResponse = conversion;
                            short_reference = conversion.short_reference;

                            //ClientMasterModel singlecontact = listContacts.Where(t => t.CCAccount_id == conversion.account_id).FirstOrDefault();
                            ClientMasterModel singlecontact = listContacts.FirstOrDefault(t => t.CCAccount_id == conversion.account_id);
                            if (singlecontact == null)
                            {
                                CurrencyCloudContactModel resultContactsAPI = new CurrencyCloudContactModel();
                                string uriContact = "contacts/find?account_id=" + conversion.account_id;

                                var resultContacts = await WebApiHelper.HttpClientRequestResponse(resultContactsAPI, uriContact, ProjectSession.CurrencyCloudToken);

                                if (resultContacts != null)
                                {
                                    foreach (ContactModel contacts in resultContacts.contacts)
                                    {
                                        AccountModel resultAccountAPI = new AccountModel();
                                        string uriAccountClient = "accounts/" + Convert.ToString(contacts.account_id);

                                        var account = await WebApiHelper.HttpClientRequestResponse(resultAccountAPI, uriAccountClient, ProjectSession.CurrencyCloudToken);

                                        if (account != null)
                                        {
                                            try
                                            {
                                                long resultImportClient = _DealService.ImportClient(contacts, account, "CurrencyCloud");

                                                if (resultImportClient > 0)
                                                {
                                                    ClientMasterModel contactModel = new ClientMasterModel();

                                                    contactModel.ClientId = resultImportClient;
                                                    contactModel.FullName = contacts.first_name + ' ' + contacts.last_name;
                                                    contactModel.CCAccount_id = contacts.account_id;
                                                    contactModel.CompanyName = contacts.account_name;
                                                    contactModel.EmailAddress = contacts.email_address;

                                                    listContacts.Add(contactModel);

                                                    ImportClientCount = ImportClientCount + 1;
                                                }
                                                else if (resultImportClient == -2)
                                                {
                                                    ExistClientCount = ExistClientCount + 1;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                _logger.Error(ex.Message);
                                                //return Json(new { ImportDealCount = ImportDealCount, ExistDealCount = ExistDealCount, ErrorMessage = conversion.short_reference + " - " + ex.Message }, JsonRequestBehavior.AllowGet);
                                            }
                                        }
                                    }
                                }
                            }

                            try
                            {
                                string TradeType = string.Empty;
                                string ConverstatinStatus = Convert.ToString(conversion.status);

                                if (ConverstatinStatus == "awaiting_funds" || ConverstatinStatus == "funds_sent")
                                    TradeType = "Traded";
                                if (ConverstatinStatus == "funds_arrived" || ConverstatinStatus == "trade_settled")
                                    TradeType = "Settled";


                                decimal clientsellamount = 0; decimal partnersellamount = 0; decimal clientbuyamount = 0; decimal partnerbuyamount = 0;

                                if (!string.IsNullOrEmpty(conversion.client_sell_amount))
                                    clientsellamount = Convert.ToDecimal(conversion.client_sell_amount);

                                if (!string.IsNullOrEmpty(conversion.partner_sell_amount) && conversion.partner_sell_amount != "0.00")
                                {
                                    partnersellamount = Convert.ToDecimal(conversion.partner_sell_amount);
                                }
                                else
                                {
                                    if (conversion.fixed_side == "sell")
                                        partnersellamount = Convert.ToDecimal(conversion.client_sell_amount);
                                    else
                                        partnersellamount = Convert.ToDecimal(conversion.client_buy_amount) * Convert.ToDecimal(conversion.mid_market_rate);
                                }

                                if (clientsellamount != partnersellamount)
                                {
                                    if ((clientsellamount - partnersellamount) < 0)
                                        conversion.GrossComm = 0;
                                    else
                                        conversion.GrossComm = clientsellamount - partnersellamount;

                                    conversion.GrossCommCurrency = conversion.sell_currency;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(conversion.client_buy_amount))
                                        clientbuyamount = Convert.ToDecimal(conversion.client_buy_amount);

                                    if (!string.IsNullOrEmpty(conversion.partner_buy_amount) && conversion.partner_buy_amount != "0.00")
                                        partnerbuyamount = Convert.ToDecimal(conversion.partner_buy_amount);
                                    else
                                    {
                                        if (conversion.fixed_side == "buy")
                                            partnerbuyamount = Convert.ToDecimal(conversion.client_buy_amount);
                                        else
                                            partnerbuyamount = Convert.ToDecimal(conversion.client_sell_amount) * Convert.ToDecimal(conversion.mid_market_rate);
                                    }

                                    //                                conversion.GrossComm = partnerbuyamount - clientbuyamount;
                                    if ((partnerbuyamount - clientbuyamount) < 0)
                                        conversion.GrossComm = 0;
                                    else
                                        conversion.GrossComm = partnerbuyamount - clientbuyamount;


                                    conversion.GrossCommCurrency = conversion.buy_currency;
                                }

                                //Gross Comm GBP
                                if (conversion.GrossCommCurrency.ToUpper() == "GBP")
                                    conversion.GrossCommGBP = conversion.GrossComm;
                                //else if (conversion.buy_currency == "GBP") //This means the profit currency is sell currency
                                //{
                                //    conversion.GrossCommGBP = conversion.GrossComm * Convert.ToDecimal(conversion.mid_market_rate); // TODO check - may be we will have to do 1/MarketRate
                                //}
                                //else if (conversion.sell_currency == "GBP") //This means the profit currency is buy currency
                                //{
                                //    conversion.GrossCommGBP = conversion.GrossComm * (1 / Convert.ToDecimal(conversion.mid_market_rate)); // TODO check - may be we will have to do MarketRate
                                //}
                                else
                                {
                                    //Get GBP Rate via Currency Cloud API
                                    decimal resultRate = await GetRates(conversion.GrossCommCurrency, "GBP");
                                    if (resultRate > 0)
                                        conversion.GrossCommGBP = conversion.GrossComm * resultRate;
                                }

                                int resultImportDeal = 0;
                                if (conversion.sell_currency != "GBP")
                                {
                                    decimal resultRate = await GetRates(conversion.sell_currency, "GBP");
                                    decimal Rate = Math.Round(resultRate, 4);
                                    if (resultRate > 0)
                                    {
                                        decimal x = Rate * Convert.ToDecimal(conversion.client_sell_amount);
                                        conversion.client_sell_amount_In_GBP = Convert.ToString(x);
                                        conversion.GBPConversationRate = Rate.ToString();
                                    }

                                    //conversion.GBPConversationRate = resultRate.ToString();

                                    // var x = Convert.ToDecimal(conversion.mid_market_rate) * Convert.ToDecimal(conversion.client_sell_amount);
                                    //conversion.client_sell_amount_In_GBP = (Convert.ToDecimal(conversion.mid_market_rate) * Convert.ToInt64(conversion.client_sell_amount)).ToString();

                                }
                                else
                                {
                                    conversion.client_sell_amount_In_GBP = conversion.client_sell_amount;
                                }
                                CurrencyCloudProfitLossModel resultProfitLossAPI = new CurrencyCloudProfitLossModel();
                                //string uriProfitLoss = "conversions/profit_and_loss?account_id=341e2851-b074-0133-8e72-0022194273c7&conversion_id=8a50b432-d013-4d2b-b34f-8f6852939d0e";
                                string uriProfitLoss = "conversions/profit_and_loss?account_id=" + conversion.account_id + "&conversion_id=" + conversion.id;

                                var resultProfitLoss = await WebApiHelper.HttpClientRequestResponse(resultProfitLossAPI, uriProfitLoss, ProjectSession.CurrencyCloudToken);

                                List<ProfitLossModel> ProfitLoss = resultProfitLoss.conversion_profit_and_losses;
                                if (ProfitLoss.Count > 0)

                                {
                                    foreach (ProfitLossModel profit_loss in resultProfitLoss.conversion_profit_and_losses)
                                    {

                                        if (profit_loss.event_type == "back_office_drawdown")
                                        {
                                            conversion.amount_profit_loss = (profit_loss.amount == null || profit_loss.amount == "") ? 0 : Convert.ToDecimal(profit_loss.amount);
                                            conversion.event_date = (profit_loss.event_date_time == null || profit_loss.ToString() == "01/01/0001 00:00:00") ? DateTime.Now : profit_loss.event_date_time;
                                        }
                                    }
                                }
                                else
                                {
                                    conversion.amount_profit_loss = 0;
                                    conversion.event_date = DateTime.Now;
                                }


                                //conversion.amount_profit_loss = Convert.ToDecimal(resultProfitLoss.amount) == null ? 0 : Convert.ToDecimal(resultProfitLoss.amount);
                                //conversion.event_date = (resultProfitLoss.event_date_time == null || resultProfitLoss.event_date_time.ToString() == "01/01/0001 00:00:00")? DateTime.Now : resultProfitLoss.event_date_time;
                                resultImportDeal = _DealService.ImportDeal(conversion, TradeType, "CurrencyCloud");
                                if (resultImportDeal > 0)
                                {
                                    DealJSONData dealjsonModel = new DealJSONData();
                                    dealjsonModel.DealId = resultImportDeal;
                                    dealjsonModel.DealJsonStr = JsonConvert.SerializeObject(originalDealResponse);
                                    dealjsonModel.UserId = ProjectSession.LoginUserDetails.UserId;
                                    _DealService.ImportGCPartnerDealJSON(dealjsonModel);
                                    resultImportDeal = 1;
                                }

                                switch (resultImportDeal)
                                {
                                    case 0:
                                        break;
                                    case 1:
                                        ImportDealCount = ImportDealCount + 1;
                                        break;
                                    case -2:
                                        ExistDealCount = ExistDealCount + 1;
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                FailedDealCount = FailedDealCount + 1;
                                DealShortReference.Append(conversion.short_reference).Append(",");
                                _logger.Error(ex.Message);
                                //_DealService.InsertImportLog(FromDate1, ToDate1, FailedDealCount, false, ImportBy, "CurrencyCloud");
                                //return Json(new { ImportDealCount = ImportDealCount, ExistDealCount = ExistDealCount, ErrorMessage = conversion.short_reference + " - " + ex.Message }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        if (resultTransactions.pagination.next_page == -1)
                            break;
                    }
                    else
                    {
                        return Json(new { ImportDealCount = ImportDealCount, ExistDealCount = ExistDealCount, ErrorMessage = "Cloudcurrency API not responding well manner.Please try again later with same dates." }, JsonRequestBehavior.AllowGet);
                    }

                    // added code to get the deals by the updated date  - start

                    string uriTransactionUpdate = "conversions/find?page=" + page + "&updated_at_from=" + FromDate + "&updated_at_to=" + ToDate + "&scope=all";
                    var resultTransactionsUpdate = await WebApiHelper.HttpClientRequestResponse(resultTransactionAPI, uriTransactionUpdate, ProjectSession.CurrencyCloudToken);

                    if (resultTransactionsUpdate == null)
                    {
                        System.Threading.Thread.Sleep(5000);
                        resultTransactionsUpdate = await WebApiHelper.HttpClientRequestResponse(resultTransactionAPI, uriTransactionUpdate, ProjectSession.CurrencyCloudToken);
                    }

                    if (resultTransactionsUpdate != null)
                    {
                        foreach (ConversionModel conversion in resultTransactionsUpdate.conversions)
                        {
                            ConversionModel originalDealResponse = conversion;
                            short_reference = conversion.short_reference;

                            ClientMasterModel singlecontact = listContacts.FirstOrDefault(t => t.CCAccount_id == conversion.account_id);
                            
                            if (singlecontact == null)
                            {
                                CurrencyCloudContactModel resultContactsAPI = new CurrencyCloudContactModel();
                                string uriContact = "contacts/find?account_id=" + conversion.account_id;

                                var resultContacts = await WebApiHelper.HttpClientRequestResponse(resultContactsAPI, uriContact, ProjectSession.CurrencyCloudToken);

                                if (resultContacts != null)
                                {
                                    foreach (ContactModel contacts in resultContacts.contacts)
                                    {
                                        AccountModel resultAccountAPI = new AccountModel();
                                        string uriAccountClient = "accounts/" + Convert.ToString(contacts.account_id);

                                        var account = await WebApiHelper.HttpClientRequestResponse(resultAccountAPI, uriAccountClient, ProjectSession.CurrencyCloudToken);

                                        if (account != null)
                                        {
                                            try
                                            {
                                                long resultImportClient = _DealService.ImportClient(contacts, account, "CurrencyCloud");

                                                if (resultImportClient > 0)
                                                {
                                                    ClientMasterModel contactModel = new ClientMasterModel();

                                                    contactModel.ClientId = resultImportClient;
                                                    contactModel.FullName = contacts.first_name + ' ' + contacts.last_name;
                                                    contactModel.CCAccount_id = contacts.account_id;
                                                    contactModel.CompanyName = contacts.account_name;
                                                    contactModel.EmailAddress = contacts.email_address;

                                                    listContacts.Add(contactModel);

                                                    ImportClientCount = ImportClientCount + 1;
                                                }
                                                else if (resultImportClient == -2)
                                                {
                                                    ExistClientCount = ExistClientCount + 1;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                _logger.Error(ex.Message);
                                                //return Json(new { ImportDealCount = ImportDealCount, ExistDealCount = ExistDealCount, ErrorMessage = conversion.short_reference + " - " + ex.Message }, JsonRequestBehavior.AllowGet);
                                            }
                                        }
                                    }
                                }
                            }

                            try
                            {
                                string TradeType = string.Empty;
                                string ConverstatinStatus = Convert.ToString(conversion.status);

                                if (ConverstatinStatus == "awaiting_funds" || ConverstatinStatus == "funds_sent")
                                    TradeType = "Traded";
                                if (ConverstatinStatus == "funds_arrived" || ConverstatinStatus == "trade_settled")
                                    TradeType = "Settled";


                                decimal clientsellamount = 0; decimal partnersellamount = 0; decimal clientbuyamount = 0; decimal partnerbuyamount = 0;

                                if (!string.IsNullOrEmpty(conversion.client_sell_amount))
                                    clientsellamount = Convert.ToDecimal(conversion.client_sell_amount);

                                if (!string.IsNullOrEmpty(conversion.partner_sell_amount) && conversion.partner_sell_amount != "0.00")
                                {
                                    partnersellamount = Convert.ToDecimal(conversion.partner_sell_amount);
                                }
                                else
                                {
                                    if (conversion.fixed_side == "sell")
                                        partnersellamount = Convert.ToDecimal(conversion.client_sell_amount);
                                    else
                                        partnersellamount = Convert.ToDecimal(conversion.client_buy_amount) * Convert.ToDecimal(conversion.mid_market_rate);
                                }

                                if (clientsellamount != partnersellamount)
                                {
                                    if ((clientsellamount - partnersellamount) < 0)
                                        conversion.GrossComm = 0;
                                    else
                                        conversion.GrossComm = clientsellamount - partnersellamount;

                                    conversion.GrossCommCurrency = conversion.sell_currency;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(conversion.client_buy_amount))
                                        clientbuyamount = Convert.ToDecimal(conversion.client_buy_amount);

                                    if (!string.IsNullOrEmpty(conversion.partner_buy_amount) && conversion.partner_buy_amount != "0.00")
                                        partnerbuyamount = Convert.ToDecimal(conversion.partner_buy_amount);
                                    else
                                    {
                                        if (conversion.fixed_side == "buy")
                                            partnerbuyamount = Convert.ToDecimal(conversion.client_buy_amount);
                                        else
                                            partnerbuyamount = Convert.ToDecimal(conversion.client_sell_amount) * Convert.ToDecimal(conversion.mid_market_rate);
                                    }

                                    //                                conversion.GrossComm = partnerbuyamount - clientbuyamount;
                                    if ((partnerbuyamount - clientbuyamount) < 0)
                                        conversion.GrossComm = 0;
                                    else
                                        conversion.GrossComm = partnerbuyamount - clientbuyamount;


                                    conversion.GrossCommCurrency = conversion.buy_currency;
                                }

                                //Gross Comm GBP
                                if (conversion.GrossCommCurrency.ToUpper() == "GBP")
                                    conversion.GrossCommGBP = conversion.GrossComm;
                                //else if (conversion.buy_currency == "GBP") //This means the profit currency is sell currency
                                //{
                                //    conversion.GrossCommGBP = conversion.GrossComm * Convert.ToDecimal(conversion.mid_market_rate); // TODO check - may be we will have to do 1/MarketRate
                                //}
                                //else if (conversion.sell_currency == "GBP") //This means the profit currency is buy currency
                                //{
                                //    conversion.GrossCommGBP = conversion.GrossComm * (1 / Convert.ToDecimal(conversion.mid_market_rate)); // TODO check - may be we will have to do MarketRate
                                //}
                                else
                                {
                                    //Get GBP Rate via Currency Cloud API
                                    decimal resultRate = await GetRates(conversion.GrossCommCurrency, "GBP");
                                    if (resultRate > 0)
                                        conversion.GrossCommGBP = conversion.GrossComm * resultRate;
                                }

                                int resultImportDeal = 0;
                                if (conversion.sell_currency != "GBP")
                                {
                                    decimal resultRate = await GetRates(conversion.sell_currency, "GBP");
                                    decimal Rate = Math.Round(resultRate, 4);
                                    if (resultRate > 0)
                                    {
                                        decimal x = Rate * Convert.ToDecimal(conversion.client_sell_amount);
                                        conversion.client_sell_amount_In_GBP = Convert.ToString(x);
                                        conversion.GBPConversationRate = Rate.ToString();
                                    }

                                    //conversion.GBPConversationRate = resultRate.ToString();

                                    // var x = Convert.ToDecimal(conversion.mid_market_rate) * Convert.ToDecimal(conversion.client_sell_amount);
                                    //conversion.client_sell_amount_In_GBP = (Convert.ToDecimal(conversion.mid_market_rate) * Convert.ToInt64(conversion.client_sell_amount)).ToString();

                                }
                                else
                                {
                                    conversion.client_sell_amount_In_GBP = conversion.client_sell_amount;
                                }

                                CurrencyCloudProfitLossModel resultProfitLossAPI = new CurrencyCloudProfitLossModel();
                                string uriProfitLoss = "conversions/profit_and_loss?account_id=" + conversion.account_id + "&conversion_id=" + conversion.id;

                                var resultProfitLoss = await WebApiHelper.HttpClientRequestResponse(resultProfitLossAPI, uriProfitLoss, ProjectSession.CurrencyCloudToken);
                                List<ProfitLossModel> ProfitLoss = resultProfitLoss.conversion_profit_and_losses;
                                if (ProfitLoss.Count > 0)
                                {
                                    foreach (ProfitLossModel profit_loss in resultProfitLoss.conversion_profit_and_losses)
                                    {

                                        if (profit_loss.event_type == "back_office_drawdown")
                                        {
                                            conversion.amount_profit_loss = (profit_loss.amount == null || profit_loss.amount == "") ? 0 : Convert.ToDecimal(profit_loss.amount);
                                            conversion.event_date = (profit_loss.event_date_time == null || profit_loss.ToString() == "01/01/0001 00:00:00") ? DateTime.Now : profit_loss.event_date_time;
                                        }
                                    }
                                }
                                else
                                {
                                    conversion.amount_profit_loss = 0;
                                    conversion.event_date = DateTime.Now;
                                }

                                //conversion.amount_profit_loss = conversion.amount_profit_loss == null ? 0 : Convert.ToDecimal(resultProfitLoss.amount);
                                // conversion.event_date = resultProfitLoss.event_date_time == null ? DateTime.Now : resultProfitLoss.event_date_time;

                                resultImportDeal = _DealService.ImportDeal(conversion, TradeType, "CurrencyCloud");
                                if (resultImportDeal > 0)
                                {
                                    DealJSONData dealjsonModel = new DealJSONData();
                                    dealjsonModel.DealId = resultImportDeal;
                                    dealjsonModel.DealJsonStr = JsonConvert.SerializeObject(originalDealResponse);
                                    dealjsonModel.UserId = ProjectSession.LoginUserDetails.UserId;
                                    _DealService.ImportGCPartnerDealJSON(dealjsonModel);
                                    resultImportDeal = 1;
                                }

                                switch (resultImportDeal)
                                {
                                    case 0:
                                        break;
                                    case 1:
                                        ImportDealCount = ImportDealCount + 1;
                                        break;
                                    case -2:
                                        ExistDealCount = ExistDealCount + 1;
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                FailedDealCount = FailedDealCount + 1;
                                DealShortReference.Append(conversion.short_reference).Append(",");
                                _logger.Error(ex.Message);
                            }
                        }

                        if (resultTransactionsUpdate.pagination.next_page == -1)
                            break;
                    }
                    else
                    {
                        return Json(new { ImportDealCount = ImportDealCount, ExistDealCount = ExistDealCount, ErrorMessage = "Cloudcurrency API not responding well manner.Please try again later with same dates." }, JsonRequestBehavior.AllowGet);
                    }

                    // added if condition to get the deals by the updated date  - end
                }
            }
            catch (Exception ex)
            {
                FailedDealCount = FailedDealCount + 1;
                _logger.Error(ex.Message);
            }

            try
            {
                if (FailedDealCount > 0)
                {
                    _DealService.InsertImportLog(FromDate1, ToDate1, FailedDealCount, DealShortReference.ToString().TrimEnd(','), false, ImportBy, "CurrencyCloud");
                }
                if (ImportDealCount > 0)
                {
                    _DealService.InsertImportLog(FromDate1, ToDate1, ImportDealCount, "", true, ImportBy, "CurrencyCloud");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return Json(new { ImportDealCount = ImportDealCount, ExistDealCount = ExistDealCount, ErrorMessage = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        public async Task<decimal> GetRates(string fromCurrency, string toCurrency)
        {
            await CommonController.GetCurrencyCloudToken();

            string pair = fromCurrency + toCurrency;
            if (exchangeRates.ContainsKey(pair))
                return exchangeRates[pair];

            string uriRates = "rates/find?currency_pair=" + pair;

            var resultTransactions = await WebApiHelper.HttpClientRequestResponseString(uriRates, ProjectSession.CurrencyCloudToken);

            if (resultTransactions != null)
            {
                dynamic data = JObject.Parse(resultTransactions.ToString());
                object Value1 = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)data).First).First).First).First).First).Value;
                object Value2 = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)data).First).First).First).First).Last).Value;

                exchangeRates.Add(pair, (Convert.ToDecimal(Value1) + Convert.ToDecimal(Value2)) / 2);

                //var x = (Convert.ToDecimal(Value1) + Convert.ToDecimal(Value2)) / 2;
                //return x;
                return (Convert.ToDecimal(Value1) + Convert.ToDecimal(Value2)) / 2;
            }

            return 0;
        }

        public class Rootobject
        {
            public Rates rates { get; set; }
            public object[] unavailable { get; set; }
        }

        public class Rates
        {
            public dynamic[] USDGBP { get; set; }
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        /// <summary>
        /// By Krupesh
        /// 03 april 2020
        /// Import Client from GCPartner API Integration
        /// </summary>
        [HttpGet]
        public async Task<int> GCPartnerImportClient(string FromDate, string ToDate, string ImportBy, long loggedInUserId)
        {
            int dealSuccess = 0;
            try
            {
                GCPartnerClientModel model = new GCPartnerClientModel();

                string url = "GetClientDetails";

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("StartDate", FromDate);
                dic.Add("EndDate", ToDate);
                var objClientList = await WebApiHelper.HttpClientPostGCPartnerToken(model, url, dic);
                if (objClientList != null && objClientList.ReturnObject.Count > 0)
                {
                    foreach (GCPartnerClient item in objClientList.ReturnObject)
                    {

                        ClientMasterModel clientModel = new ClientMasterModel();
                        clientModel.AddressLine1 = item.AddressLine1;
                        clientModel.AddressLine2 = item.AddressLine2;
                        clientModel.EmailAddress = item.EmailAddress;
                        clientModel.FullName = item.FirstName + " " + item.LastName;
                        clientModel.CompanyName = item.CompanyName;
                        clientModel.City_Town = item.TownCity;
                        clientModel.City_Town = item.TownCity;
                        clientModel.Country = item.CountryId;
                        clientModel.MobileNo = item.Mobile;
                        clientModel.AltMobileNo = item.Telephone;
                        clientModel.RegiterDate = item.DateAdded;
                        clientModel.IsActive = item.EmploymentStatus == null ? false : true;
                        clientModel.GcPartnerClientId = Convert.ToString(item.ClientId);
                        clientModel.Source = "GCPartner";
                        clientModel.UserId = loggedInUserId;
                        _DealService.ImportGCPartnerClient(clientModel);
                    }
                }

                dealSuccess = await GCPartnerImportDeal(FromDate, ToDate, loggedInUserId, ImportBy);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return dealSuccess;
            }

            return dealSuccess;

        }

        /// <summary>
        /// By Krupesh
        /// 03 april 2020
        /// Import Deal from GCPartner API Integration
        /// </summary>
        [HttpGet]
        public async Task<int> GCPartnerImportDeal(string FromDate, string ToDate, long loggedInUserId, string ImportBy = "Manual")
        {
            int SuccessDealCount = 0;
            int FailureDealCount = 0;

            try
            {
                GCPartnerDealModel model = new GCPartnerDealModel();
                string url = "v2/GetDealHistory";
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("StartDate", FromDate);
                dic.Add("EndDate", ToDate);
                var objClientDealList = await WebApiHelper.HttpClientPostGCPartnerToken(model, url, dic);
                if (objClientDealList != null && objClientDealList.ReturnObject.Count > 0)
                {
                    foreach (GCPartnerDeal item in objClientDealList.ReturnObject)
                    {
                        try
                        {
                            DealModel dealModel = new DealModel();
                            dealModel.ClientId = item.ClientId;
                            dealModel.DealNo = item.DealReference;
                            dealModel.TradeDate = Convert.ToDateTime(item.TransactionDate);
                            dealModel.ValueDate = Convert.ToDateTime(item.MaturityDate);
                            dealModel.ClientSoldCCY = item.SourceCurrency;
                            dealModel.ClientBoughtCCY = item.PurchaseCurrency;
                            dealModel.ClientSoldAmt = item.SellAmount;
                            dealModel.ClientBoughtAmt = item.BuyAmount;
                            dealModel.TStatus = item.Status;
                            dealModel.TradeType = item.Type;
                            dealModel.DealSource = "GCPartner";
                            dealModel.UserId = loggedInUserId;
                            long dealId = _DealService.ImportGCPartnerDealData(dealModel);
                            if (dealId > 0)
                            {
                                DealJSONData dealjsonModel = new DealJSONData();
                                dealjsonModel.DealId = dealId;
                                dealjsonModel.DealJsonStr = JsonConvert.SerializeObject(item);
                                dealjsonModel.UserId = loggedInUserId;
                                _DealService.ImportGCPartnerDealJSON(dealjsonModel);
                                SuccessDealCount = SuccessDealCount + 1;
                            }



                        }
                        catch (Exception e)
                        {
                            FailureDealCount = FailureDealCount + 1;
                            _logger.Error(e.Message);
                        }
                    }

                    try
                    {
                        DateTime FromDate1;
                        DateTime ToDate1;
                        string format = "yyyy-MM-dd";

                        if (DateTime.TryParseExact(FromDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate1))
                        {

                        }
                        if (DateTime.TryParseExact(ToDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate1))
                        {

                        }
                        if (SuccessDealCount > 0)
                            _DealService.InsertImportLog(FromDate1, ToDate1, SuccessDealCount, "", true, ImportBy, "GCPartner");
                        if (FailureDealCount > 0)
                            _DealService.InsertImportLog(FromDate1, ToDate1, FailureDealCount, "", false, ImportBy, "GCPartner");

                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return SuccessDealCount;
            }

            return SuccessDealCount;
        }

        // GET: ReconciliationDeal
        public ActionResult ReconciliationDeal()
        {

            return View();
        }

        public ActionResult SaveReconciliation(HttpPostedFileBase ReconciliationFile, ReconciliationModel model)
        {
            try
            {
                string directoryPath = System.Web.HttpContext.Current.Server.MapPath("~/TempFiles");
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }
                string filePath = directoryPath + @"\" + Guid.NewGuid().ToString() + ".xlsx";
                if (ReconciliationFile != null)
                {
                    string contentType = ReconciliationFile.ContentType;
                    //string extension = contentType.Substring(contentType.IndexOf('/') + 1, contentType.Length - contentType.IndexOf('/') - 1);
                    byte[] fileData = null;
                    using (BinaryReader binaryReader = new BinaryReader(ReconciliationFile.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(ReconciliationFile.ContentLength);
                        System.IO.File.WriteAllBytes(filePath, fileData);
                    }
                }

                string excelfilepath = filePath;
                string strConnectionString = "";

                if (excelfilepath.ToLower().Trim().EndsWith(".xlsx") || excelfilepath.ToLower().Trim().EndsWith(".xls") || filePath.ToLower().Trim().EndsWith(".csv"))
                {
                    strConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", excelfilepath);
                }

                OleDbConnection OleDbConn = new OleDbConnection(strConnectionString);

                OleDbConn.Open();
                DataTable dtSheets = OleDbConn.GetSchema("Tables");
                OleDbDataAdapter OleDbAdapter = new OleDbDataAdapter();

                string sql = "SELECT * FROM [" + dtSheets.Rows[0]["Table_name"] + "]";
                DataTable dtXLS = new DataTable(Convert.ToString(dtSheets.Rows[0]["Table_name"]).Replace("$", ""));
                dtXLS.TableName = "Sheet1";
                OleDbCommand oleDbcommand = new OleDbCommand(sql, OleDbConn);
                oleDbcommand.CommandTimeout = 120000000;
                OleDbAdapter.SelectCommand = oleDbcommand;
                OleDbAdapter.Fill(dtXLS);
                OleDbConn.Close();
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                //Match columns with uploaded sheet
                string[] ColumnsName = new string[] { "Account Name", "Parent Trade", "Original Trade", "Account ID", "Deal Ref",
                    "Trade Date", "Value Date", "Client Bought Amount", "Client Bought CCY", "Client Sold Amount",
                    "Client Sold CCY", "Client Sold Amount GBP","Gross Comms GBP", "Our Payment Fees",
                    "Net Comms", "IB Comms Percent", "IB Commission due", "Introducer Broker"};
                foreach (string colName in ColumnsName)
                {

                    bool IsColExist = dtXLS.Columns.Contains(colName);

                    if (!IsColExist)
                    {
                        ErrorNotification("Column name " + colName + " is missing in uploaded sheet");
                        return RedirectToAction("ReconciliationDeal", "Deal");
                    }
                }

                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("AccountName");
                dtResult.Columns.Add("ParentTrade");
                dtResult.Columns.Add("OriginalTrade");
                dtResult.Columns.Add("AccountId");
                dtResult.Columns.Add("DealRef");
                dtResult.Columns.Add("TradeDate");
                dtResult.Columns.Add("ValueDate");
                dtResult.Columns.Add("ClientBoughtAmount");
                dtResult.Columns.Add("ClientBoughtCCY");
                dtResult.Columns.Add("ClientSoldAmount");
                dtResult.Columns.Add("ClientSoldCCY");
                dtResult.Columns.Add("ClientSoldAmountGBP");
                dtResult.Columns.Add("GrossCommsGBP");
                dtResult.Columns.Add("OurPaymentFees");
                dtResult.Columns.Add("NetComms");
                dtResult.Columns.Add("IBCommsPercent");
                dtResult.Columns.Add("IBCommissiondue");
                dtResult.Columns.Add("IntroducerBroker");

                decimal test;
                if (dtXLS != null)
                {
                    for (int i = 0; i < dtXLS.Rows.Count; i++)
                    {
                        DataRow dtrow = dtResult.NewRow();
                        dtrow["AccountName"] = Convert.ToString(dtXLS.Rows[i]["Account Name"]);
                        dtrow["ParentTrade"] = Convert.ToString(dtXLS.Rows[i]["Parent Trade"]);
                        dtrow["OriginalTrade"] = Convert.ToString(dtXLS.Rows[i]["Original Trade"]);
                        dtrow["AccountId"] = Convert.ToString(dtXLS.Rows[i]["Account ID"]);
                        dtrow["DealRef"] = Convert.ToString(dtXLS.Rows[i]["Deal Ref"]);
                        dtrow["TradeDate"] = DateTime.Parse(dtXLS.Rows[i]["Trade Date"].ToString()).ToString("MM/dd/yyyy");
                        dtrow["ValueDate"] = DateTime.Parse(dtXLS.Rows[i]["Value Date"].ToString()).ToString("MM/dd/yyyy");
                        if (decimal.TryParse(Convert.ToString(dtXLS.Rows[i]["Client Bought Amount"]), out test))
                        {
                            dtrow["ClientBoughtAmount"] = Convert.ToString(dtXLS.Rows[i]["Client Bought Amount"]);
                        }
                        else
                        {
                            ErrorNotification("Client Bought Amount is not valid at row " + (i + 2));
                            return RedirectToAction("ReconciliationDeal", "Deal");
                        }
                        dtrow["ClientBoughtCCY"] = Convert.ToString(dtXLS.Rows[i]["Client Bought CCY"]);
                        if (decimal.TryParse(Convert.ToString(dtXLS.Rows[i]["Client Sold Amount"]), out test))
                        {
                            dtrow["ClientSoldAmount"] = Convert.ToString(dtXLS.Rows[i]["Client Sold Amount"]);
                        }
                        else
                        {
                            ErrorNotification("Client Sold Amount is not valid at row " + (i + 2));
                            return RedirectToAction("ReconciliationDeal", "Deal");
                        }
                        dtrow["ClientSoldCCY"] = Convert.ToString(dtXLS.Rows[i]["Client Sold CCY"]);
                        if (decimal.TryParse(Convert.ToString(dtXLS.Rows[i]["Client Sold Amount GBP"]), out test))
                        {
                            dtrow["ClientSoldAmountGBP"] = Convert.ToString(dtXLS.Rows[i]["Client Sold Amount GBP"]);
                        }
                        else
                        {
                            ErrorNotification("Client Sold Amount GBP is not valid at row " + (i + 2));
                            return RedirectToAction("ReconciliationDeal", "Deal");
                        }
                        if (decimal.TryParse(Convert.ToString(dtXLS.Rows[i]["Gross Comms GBP"]), out test))
                        {
                            dtrow["GrossCommsGBP"] = Convert.ToString(dtXLS.Rows[i]["Gross Comms GBP"]);
                        }
                        else
                        {
                            ErrorNotification("Gross Comms GBP is not valid at row " + (i + 2));
                            return RedirectToAction("ReconciliationDeal", "Deal");
                        }
                        if (decimal.TryParse(Convert.ToString(dtXLS.Rows[i]["Our Payment Fees"]), out test))
                        {
                            dtrow["OurPaymentFees"] = Convert.ToString(dtXLS.Rows[i]["Our Payment Fees"]);
                        }
                        else
                        {
                            ErrorNotification("Our Payment Fees is not valid at row " + (i + 2));
                            return RedirectToAction("ReconciliationDeal", "Deal");
                        }
                        if (decimal.TryParse(Convert.ToString(dtXLS.Rows[i]["Net Comms"]), out test))
                        {
                            dtrow["NetComms"] = Convert.ToString(dtXLS.Rows[i]["Net Comms"]);
                        }
                        else
                        {
                            ErrorNotification("Net Comms is not valid at row " + (i + 2));
                            return RedirectToAction("ReconciliationDeal", "Deal");
                        }
                        if (decimal.TryParse(Convert.ToString(dtXLS.Rows[i]["IB Comms Percent"]), out test))
                        {
                            dtrow["IBCommsPercent"] = Convert.ToString(dtXLS.Rows[i]["IB Comms Percent"]);
                        }
                        else
                        {
                            ErrorNotification("IB Comms Percent is not valid at row " + (i + 2));
                            return RedirectToAction("ReconciliationDeal", "Deal");
                        }
                        if (decimal.TryParse(Convert.ToString(dtXLS.Rows[i]["IB Commission due"]), out test))
                        {
                            dtrow["IBCommissiondue"] = Convert.ToString(dtXLS.Rows[i]["IB Commission due"]);
                        }
                        else
                        {
                            ErrorNotification("IB Commission due is not valid at row " + (i + 2));
                            return RedirectToAction("ReconciliationDeal", "Deal");
                        }
                        dtrow["IntroducerBroker"] = Convert.ToString(dtXLS.Rows[i]["Introducer Broker"]);
                        dtResult.Rows.Add(dtrow);
                    }
                }

                long result = _DealService.SaveReconciliation(dtResult, model);
                if (result > 0)
                {
                    SuccessNotification("Record Saved Successfully");
                }
                else
                {
                    ErrorNotification("Error Occur record not saved successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ErrorNotification("Something went wrong");
                return RedirectToAction("ReconciliationDeal", "Deal");
            }
            return RedirectToAction("ReconciliationDeal", "Deal");
        }

        public ActionResult GetMissingReconciliationDeal([DataSourceRequest] DataSourceRequest request)
        {
            List<ReconciliationMissingOrMismatchModel> result = _DealService.GetMissingReconciliationDeal();
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteMissingRecords(long reconciliationId)
        {
            int result = _DealService.DeleteMissingRecords(reconciliationId);

            switch (result)
            {
                case 0:
                    return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteAllMissingRecords()
        {
            int result = _DealService.DeleteAllMissingRecords();

            switch (result)
            {
                case 0:
                    return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetMismatchReconciliationDeal([DataSourceRequest] DataSourceRequest request)
        {
            int pageNo = request.Page;
            int pageSize;
            if (request.PageSize == 0)
            {
                pageSize = 10;
            }
            else
            {
                pageSize = request.PageSize;
            }

            string dealRefNo = string.Empty;
            if (request.Filters != null) /*request.Filters.Any()*/
            {
                foreach (IFilterDescriptor filter in request.Filters)
                {
                    FilterDescriptor descriptor = filter as FilterDescriptor;
                    if (descriptor != null && descriptor.Member == "DealRef")
                    {
                        dealRefNo = Convert.ToString(descriptor.Value);

                    }

                }

            }
            List<ReconciliationMissingOrMismatchModel> result = _DealService.GetMismatchReconciliationDeal(pageNo, pageSize, dealRefNo);


            if (result != null && result.Count > 0)
            {
                DataSourceResult obj = new DataSourceResult();
                obj.Data = result;
                if (result.Count > 0)
                {

                    obj.Total = result.FirstOrDefault().TotalCount;

                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<ReconciliationMissingOrMismatchModel>(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MergerReconciliationData(string DealRef, string MismatchColumn)
        {
            int result = _DealService.MergerReconciliationData(DealRef, MismatchColumn);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MergeAllReconciliationData(List<ReconciliationMissingOrMismatchModel> MismatchReconciliationDeal)
        {
            int result = 0;

            foreach (ReconciliationMissingOrMismatchModel item in MismatchReconciliationDeal)
            {
                if (item.Source == "Excel")
                {
                    result = _DealService.MergerReconciliationData(item.DealRef, item.MismatchColumn);
                }

            }
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReconciliationDetail()
        {
            return View();
        }

        public ActionResult UploadDeals()
        {
            List<DealModel> tempDealData = _DealService.GetAllTempDealList();
            ViewBag.CompleteData = tempDealData;
            return View();
        }
        public async Task<ActionResult> SaveUploadDealAsync(HttpPostedFileBase UploadFormFile, DealModel model)
        {
            try
            {
                string directoryPath = System.Web.HttpContext.Current.Server.MapPath("~/TempFiles");
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }
                string filePath = directoryPath + @"\" + Guid.NewGuid().ToString() + DateTime.UtcNow.ToString("f") + ".xlsx";
                if (UploadFormFile != null)
                {
                    string contentType = UploadFormFile.ContentType;
                    //string extension = contentType.Substring(contentType.IndexOf('/') + 1, contentType.Length - contentType.IndexOf('/') - 1);
                    byte[] fileData = null;
                    using (BinaryReader binaryReader = new BinaryReader(UploadFormFile.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(UploadFormFile.ContentLength);
                        System.IO.File.WriteAllBytes(filePath, fileData);
                    }
                }

                string excelfilepath = filePath;
                string strConnectionString = "";

                if (excelfilepath.ToLower().Trim().EndsWith(".xlsx") || excelfilepath.ToLower().Trim().EndsWith(".xls") || excelfilepath.ToLower().Trim().EndsWith(".csv"))
                {
                    strConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", excelfilepath);
                }

                OleDbConnection OleDbConn = new OleDbConnection(strConnectionString);

                OleDbConn.Open();
                DataTable dtSheets = OleDbConn.GetSchema("Tables");
                OleDbDataAdapter OleDbAdapter = new OleDbDataAdapter();

                string sql = null;
                if (model.DealSource == "GCPartner")
                {
                    sql = "SELECT * FROM [" + dtSheets.Rows[0]["Table_name"] + "]";
                }
                else if (model.DealSource == "Eburry")
                {
                    //sql = "SELECT * FROM [Sheet1$a4:ai]";
                    //SELECT* FROM[Sheet1$a4:ai]

                    if (dtSheets.Rows[0]["Table_name"].ToString().Contains("'"))
                    {
                        string name = dtSheets.Rows[0]["Table_name"].ToString().Replace("'", "");
                        sql = "SELECT* FROM[" + name + "a4:ai]";
                    }
                    else
                    {
                        sql = "SELECT* FROM[" + dtSheets.Rows[0]["Table_name"] + "a4:ai]";
                    }

                }
                else if (model.DealSource == "ScioPay")
                {
                    sql = "SELECT * FROM [" + dtSheets.Rows[0]["Table_name"] + "]";
                }


                DataTable dtXLS = new DataTable(Convert.ToString(dtSheets.Rows[0]["Table_name"]).Replace("$", ""));
                dtXLS.TableName = "Sheet1";
                OleDbCommand oleDbcommand = new OleDbCommand(sql, OleDbConn);
                oleDbcommand.CommandTimeout = 120000000;
                OleDbAdapter.SelectCommand = oleDbcommand;
                OleDbAdapter.Fill(dtXLS);
                OleDbConn.Close();

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                if (model.DealSource == "GCPartner")
                {
                    DataTable dtResult = new DataTable();
                    dtResult.Columns.Add("DealNo");
                    dtResult.Columns.Add("ClientId");
                    dtResult.Columns.Add("ClientName");//CompanyName
                    dtResult.Columns.Add("TradeDate");
                    dtResult.Columns.Add("ValueDate");
                    dtResult.Columns.Add("ProfitType");
                    dtResult.Columns.Add("ClientSoldCCY");
                    dtResult.Columns.Add("ClientSoldAmt");
                    dtResult.Columns.Add("ClientBoughtCCY");
                    dtResult.Columns.Add("ClientBoughtAmt");
                    dtResult.Columns.Add("GrossCommCurrency");
                    dtResult.Columns.Add("EstNetProfit");
                    dtResult.Columns.Add("Commission");
                    dtResult.Columns.Add("AccountId");
                    dtResult.Columns.Add("ContactName");//FullName
                    dtResult.Columns.Add("ExsistingCompanyName");
                    dtResult.Columns.Add("IsMatch");


                    if (dtXLS != null)
                    {
                        if (dtXLS.Rows.Count > 0)
                        {
                            dtXLS = dtXLS.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull)).CopyToDataTable();
                        }
                        foreach (DataColumn col in dtXLS.Columns)
                        {
                            col.ColumnName = col.ColumnName.Trim();
                        }
                        for (int i = 0; i < dtXLS.Rows.Count; i++)
                        {
                            DataRow dtrow = dtResult.NewRow();
                            dtrow["DealNo"] = Convert.ToString(dtXLS.Rows[i]["Deal Code"]);
                            string companyName = Convert.ToString(dtXLS.Rows[i]["Client Name"]);
                            dtrow["ClientName"] = companyName;
                            string tdate = dtXLS.Rows[i]["Trade Date"].ToString();
                            string vdate = dtXLS.Rows[i]["Value Date"].ToString();
                            if (tdate != "")
                            {
                                dtrow["TradeDate"] = DateTime.Parse(dtXLS.Rows[i]["Trade Date"].ToString()).ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                ErrorNotification("Trade Date cannot be null at row " + (i + 1));
                                return RedirectToAction("UploadDeals", "Deal");
                            }
                            if (vdate != "")
                            {
                                dtrow["ValueDate"] = DateTime.Parse(dtXLS.Rows[i]["Value Date"].ToString()).ToString("MM/dd/yyyy");
                            }
                            string AccountId = Convert.ToString(dtXLS.Rows[i]["Account Id"]);
                            //if (AccountId != "" && AccountId != null)
                            //{
                            //    dtrow["AccountId"] = Convert.ToString(dtXLS.Rows[i]["Account Id"]);
                            //}
                            //else
                            //{
                            //    ErrorNotification("Account Id cannot be null at row " + (i + 1));
                            //    return RedirectToAction("UploadDeals", "Deal");
                            //}


                            ClientMasterModel Client = _DealService.GetClientIdbyCompanyName(companyName, model.DealSource, AccountId);

                            dtrow["ClientId"] = Client.ClientId;
                            dtrow["ContactName"] = Client.ContactName;
                            dtrow["ExsistingCompanyName"] = Client.CompanyName;
                            dtrow["IsMatch"] = Client.Is_match;
                            dtrow["ProfitType"] = Convert.ToString(dtXLS.Rows[i]["Profit Type"]);
                            dtrow["ClientSoldCCY"] = Convert.ToString(dtXLS.Rows[i]["Currency Sold"]);
                            dtrow["ClientSoldAmt"] = Convert.ToString(dtXLS.Rows[i]["Amount Sold"]);
                            dtrow["ClientBoughtCCY"] = Convert.ToString(dtXLS.Rows[i]["Currency Bought"]);
                            dtrow["ClientBoughtAmt"] = Convert.ToString(dtXLS.Rows[i]["Amount Bought"]);
                            dtrow["GrossCommCurrency"] = Convert.ToString(dtXLS.Rows[i]["Currency"]);
                            dtrow["EstNetProfit"] = Convert.ToString(dtXLS.Rows[i]["Est Net Profit"]);
                            dtrow["Commission"] = Convert.ToString(dtXLS.Rows[i]["Commission"]);

                            dtResult.Rows.Add(dtrow);
                        }

                        List<string> valueDatefilter = dtResult.AsEnumerable().Where(x => x.Field<string>("ValueDate") == " " || x.Field<string>("ValueDate") == null).Select(x => x.Field<string>("DealNo")).ToList();
                        foreach (string i in valueDatefilter)
                        {
                            string valueDate = dtResult.AsEnumerable().Where(x => x.Field<string>("DealNo") == i && x.Field<string>("ValueDate") != null).Select(x => x.Field<string>("ValueDate")).FirstOrDefault();
                            dtResult.AsEnumerable().Where(row => row.Field<string>("DealNo") == i && row.Field<string>("ValueDate") == " " || row.Field<string>("ValueDate") == null).ToList().ForEach(r =>
                            {
                                r["ValueDate"] = valueDate;
                            });
                        }
                    }
                    List<string> deal_code = (from r in dtResult.AsEnumerable() select Convert.ToString(r["DealNo"])).Distinct().ToList();
                    foreach (string code in deal_code)
                    {
                        List<string> EstNetProfitfilter = dtResult.AsEnumerable().Where(x => x.Field<string>("DealNo") == code).Select(x => x.Field<string>("EstNetProfit")).ToList();
                        List<string> Commissionfilter = dtResult.AsEnumerable().Where(x => x.Field<string>("DealNo") == code).Select(x => x.Field<string>("Commission")).ToList();

                        double sumOfNetProfit = 0;
                        double sumOfCommission = 0;
                        foreach (string netProfit in EstNetProfitfilter)
                        {
                            sumOfNetProfit = sumOfNetProfit + Convert.ToDouble(netProfit);
                        }
                        foreach (string commission in Commissionfilter)
                        {
                            sumOfCommission += Convert.ToDouble(commission);
                        }
                        dtResult.AsEnumerable().Where(row => row.Field<string>("DealNo") == code).ToList().ForEach(r =>
                        {
                            r["EstNetProfit"] = sumOfNetProfit;
                            r["Commission"] = sumOfCommission;
                        });
                    }

                    IEnumerable<DataRow> distinct_dtrow = dtResult.AsEnumerable().GroupBy(x => x.Field<string>("DealNo")).Select(x => x.FirstOrDefault());
                    DataTable distinct_dtResult = new DataTable();
                    distinct_dtResult = distinct_dtrow.CopyToDataTable();
                    string result = _DealService.SaveUploadGCPartner(distinct_dtResult, model);
                    if (result == "")
                    {
                        SuccessNotification("Record Saved Successfully");
                    }
                    else
                    {
                        ErrorNotification("Error Occur record not saved successfully");
                    }
                }
                else if (model.DealSource == "Eburry")
                {
                    DataTable dtResult = new DataTable();
                    dtResult.Columns.Add("DealNo");
                    dtResult.Columns.Add("ClientId");
                    dtResult.Columns.Add("ContactName");
                    dtResult.Columns.Add("TradeType");
                    dtResult.Columns.Add("TStatus");
                    dtResult.Columns.Add("CompanyName");
                    dtResult.Columns.Add("TradeDate");
                    dtResult.Columns.Add("ValueDate");
                    dtResult.Columns.Add("ClientSoldCCY");
                    dtResult.Columns.Add("ClientSoldAmt");
                    dtResult.Columns.Add("ClientBoughtCCY");
                    dtResult.Columns.Add("ClientBoughtAmt");
                    dtResult.Columns.Add("GBPConversationRate");
                    dtResult.Columns.Add("AccountId");
                    dtResult.Columns.Add("ExsistingCompanyName");
                    dtResult.Columns.Add("IsMatch");


                    if (dtXLS != null)
                    {
                        if (dtXLS.Rows.Count > 0)
                        {
                            dtXLS = dtXLS.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull)).CopyToDataTable();
                        }
                        foreach (DataColumn col in dtXLS.Columns)
                        {
                            col.ColumnName = col.ColumnName.Trim();
                        }
                        for (int i = 0; i < dtXLS.Rows.Count; i++)
                        {
                            DataRow dtrow = dtResult.NewRow();
                            string DealNo = Convert.ToString(dtXLS.Rows[i]["Transaction Receipt"]);
                            if (DealNo != "" && !string.IsNullOrEmpty(DealNo))
                            {
                                dtrow["DealNo"] = Convert.ToString(dtXLS.Rows[i]["Transaction Receipt"]);
                            }
                            else
                            {
                                ErrorNotification("Transaction Receipt cannot be null at row " + (i + 1));
                                return RedirectToAction("UploadDeals", "Deal");
                            }

                            dtrow["TradeType"] = Convert.ToString(dtXLS.Rows[i]["Type"]);
                            dtrow["TStatus"] = Convert.ToString(dtXLS.Rows[i]["Status"]);
                            string CompanyName = Convert.ToString(dtXLS.Rows[i]["Account Name"]);
                            dtrow["CompanyName"] = CompanyName;
                            string AccountId = Convert.ToString(dtXLS.Rows[i]["Account Number"]);
                            if (AccountId != "" && AccountId != null)
                            {
                                dtrow["AccountId"] = Convert.ToString(dtXLS.Rows[i]["Account Number"]);
                            }
                            else
                            {
                                ErrorNotification("Account Number cannot be null at row " + (i + 1));
                                return RedirectToAction("UploadDeals", "Deal");
                            }

                            ClientMasterModel Client = _DealService.GetClientIdbyCompanyName(CompanyName, model.DealSource, AccountId);
                            dtrow["ClientId"] = Client.ClientId;
                            dtrow["ContactName"] = Client.ContactName;
                            dtrow["ExsistingCompanyName"] = Client.CompanyName;
                            dtrow["IsMatch"] = Client.Is_match;


                            string TradeDate = dtXLS.Rows[i]["Order Date"].ToString();

                            if (TradeDate != "")
                            {
                                dtrow["TradeDate"] = DateTime.Parse(dtXLS.Rows[i]["Order Date"].ToString()).ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                ErrorNotification("Order Date cannot be null at row " + (i + 1));
                                return RedirectToAction("UploadDeals", "Deal");
                            }
                            string ValueDate = dtXLS.Rows[i]["Maturity Date"].ToString();
                            if (ValueDate != "" && !string.IsNullOrEmpty(ValueDate))
                            {
                                dtrow["ValueDate"] = DateTime.Parse(dtXLS.Rows[i]["Maturity Date"].ToString()).ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                ErrorNotification("Maturity Date cannot be null at row " + (i + 1));
                                return RedirectToAction("UploadDeals", "Deal");
                            }

                            //dtrow["ProfitType"] = Convert.ToString(dtXLS.Rows[i]["Profit Type"]);
                            dtrow["ClientSoldCCY"] = Convert.ToString(dtXLS.Rows[i]["Sell Ccy"]);
                            dtrow["ClientSoldAmt"] = Convert.ToString(dtXLS.Rows[i]["Sell Amount"]);
                            dtrow["ClientBoughtCCY"] = Convert.ToString(dtXLS.Rows[i]["Buy Ccy"]);
                            dtrow["ClientBoughtAmt"] = Convert.ToString(dtXLS.Rows[i]["Buy Amount"]);
                            dtrow["GBPConversationRate"] = Convert.ToString(dtXLS.Rows[i]["Client Rate"]);

                            dtResult.Rows.Add(dtrow);
                        }
                    }
                    string result = _DealService.SaveUploadEburry(dtResult, model);
                    if (result == "")
                    {
                        SuccessNotification("Record Saved Successfully");
                    }
                    else
                    {
                        ErrorNotification("Error Occur record not saved successfully");
                    }
                }
                else if (model.DealSource == "ScioPay")
                {
                    DataTable dtResult = new DataTable();
                    dtResult.Columns.Add("DealNo");

                    //added

                    dtResult.Columns.Add("ClientId");

                    dtResult.Columns.Add("ClientName");//account name
                    dtResult.Columns.Add("TradeDate");
                    dtResult.Columns.Add("ValueDate");
                    dtResult.Columns.Add("ClientSoldCCY");
                    dtResult.Columns.Add("ClientSoldAmt");
                    dtResult.Columns.Add("ClientBoughtCCY");
                    dtResult.Columns.Add("ClientBoughtAmt");
                    dtResult.Columns.Add("CompanyName");
                    dtResult.Columns.Add("TStatus"); //conversation status
                    dtResult.Columns.Add("MarketConversionRate");
                    dtResult.Columns.Add("ProfitOrLoss");
                    dtResult.Columns.Add("EventDate");
                    dtResult.Columns.Add("ScioPayAccountId");
                    dtResult.Columns.Add("GrossComm");
                    dtResult.Columns.Add("GrossCommCurrency");
                    dtResult.Columns.Add("GrossCommGBP");
                    dtResult.Columns.Add("ClientSoldGBP");
                    dtResult.Columns.Add("GBPConversationRate");
                    dtResult.Columns.Add("ProfitGBPEst");
                    dtResult.Columns.Add("ProfitCCY");
                    dtResult.Columns.Add("Spread");
                    dtResult.Columns.Add("VolumeGBPEst");
                    dtResult.Columns.Add("WLCommsCCY");
                    dtResult.Columns.Add("WLCommsInCCY");
                    dtResult.Columns.Add("WLRevShare");
                    dtResult.Columns.Add("WLTotalCommGBP");
                    dtResult.Columns.Add("Brand");
                    dtResult.Columns.Add("AccountCountry");
                    dtResult.Columns.Add("Owner");
                    dtResult.Columns.Add("Contact");
                    dtResult.Columns.Add("Creator");
                    dtResult.Columns.Add("AccountId");
                    dtResult.Columns.Add("ExsistingCompanyName");
                    dtResult.Columns.Add("IsMatch");

                    if (dtXLS != null)
                    {
                        if (dtXLS.Rows.Count > 0)
                        {
                            dtXLS = dtXLS.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull)).CopyToDataTable();
                        }
                        foreach (DataColumn col in dtXLS.Columns)
                        {
                            col.ColumnName = col.ColumnName.Trim();
                        }
                        for (int i = 0; i < dtXLS.Rows.Count; i++)
                        {
                            DataRow dtrow = dtResult.NewRow();
                            //added
                            dtrow["DealNo"] = Convert.ToString(dtXLS.Rows[i]["Reference"]);
                            string companyName = Convert.ToString(dtXLS.Rows[i]["Account"]);
                            dtrow["CompanyName"] = Convert.ToString(dtXLS.Rows[i]["Account"]);

                            string TradeDate = dtXLS.Rows[i]["Created at"].ToString();
                            if (TradeDate != "" || !string.IsNullOrEmpty(TradeDate))
                            {
                                dtrow["TradeDate"] = DateTime.Parse(dtXLS.Rows[i]["Created at"].ToString()).ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                ErrorNotification("Created at cannot be null at row " + (i + 1));
                                return RedirectToAction("UploadDeals", "Deal");
                            }
                            string valueDate = dtXLS.Rows[i]["Settlement"].ToString();
                            if (valueDate != "" || !string.IsNullOrEmpty(valueDate))
                            {
                                dtrow["ValueDate"] = DateTime.Parse(dtXLS.Rows[i]["Settlement"].ToString()).ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                ErrorNotification("Settlement cannot be null at row " + (i + 1));
                                return RedirectToAction("UploadDeals", "Deal");
                            }
                            //dtrow["TradeDate"] = DateTime.Parse(dtXLS.Rows[i]["Created at"].ToString()).ToString("MM/dd/yyyy");
                            // dtrow["ValueDate"] = DateTime.Parse(dtXLS.Rows[i]["Settlement"].ToString()).ToString("MM/dd/yyyy");
                            dtrow["TStatus"] = Convert.ToString(dtXLS.Rows[i]["Status"]);
                            string clientSoldAmt = Convert.ToString(dtXLS.Rows[i]["Sold"]);
                            dtrow["ClientSoldAmt"] = clientSoldAmt;
                            string partnerSoldAmount = Convert.ToString(dtXLS.Rows[i]["Core Sold"]);
                            string clientBoughtAmt = Convert.ToString(dtXLS.Rows[i]["Bought"]);
                            dtrow["ClientBoughtAmt"] = clientBoughtAmt;
                            string midMarketRate = Convert.ToString(dtXLS.Rows[i]["Core Rate"]);
                            if (midMarketRate != "" && !string.IsNullOrEmpty(midMarketRate))
                            {
                                dtrow["MarketConversionRate"] = midMarketRate;
                            }
                            else
                            {
                                ErrorNotification("Core Rate cannot be null at row " + (i + 1));
                                return RedirectToAction("UploadDeals", "Deal");
                            }

                            string clientSoldCurrency = Convert.ToString(dtXLS.Rows[i]["Sold CCY"]);
                            dtrow["ClientSoldCCY"] = clientSoldCurrency;
                            string partnerBoughtAmt = Convert.ToString(dtXLS.Rows[i]["Core Bought"]);
                            string fixedSide = Convert.ToString(dtXLS.Rows[i]["Clients fixed side"]);
                            string clientBoughtCurrency = Convert.ToString(dtXLS.Rows[i]["Bought CCY"]);
                            dtrow["ClientBoughtCCY"] = clientBoughtCurrency;

                            dtrow["ProfitGBPEst"] = Convert.ToString(dtXLS.Rows[i]["Profit GBP Est"]);
                            dtrow["ProfitCCY"] = Convert.ToString(dtXLS.Rows[i]["Profit CCY"]);
                            dtrow["Spread"] = Convert.ToString(dtXLS.Rows[i]["Spread"]);
                            dtrow["VolumeGBPEst"] = Convert.ToString(dtXLS.Rows[i]["Volume GBP Est"]);
                            dtrow["WLCommsCCY"] = Convert.ToString(dtXLS.Rows[i]["WL Comms CCY"]);
                            dtrow["WLCommsInCCY"] = Convert.ToString(dtXLS.Rows[i]["WL Comms (in CCY)"]);
                            dtrow["GrossCommGBP"] = Convert.ToString(dtXLS.Rows[i]["Comms GBP"]);
                            dtrow["WLRevShare"] = Convert.ToString(dtXLS.Rows[i]["WL Rev Share"]);
                            dtrow["WLTotalCommGBP"] = Convert.ToString(dtXLS.Rows[i]["WL Comms"]);
                            dtrow["Brand"] = Convert.ToString(dtXLS.Rows[i]["Brand"]);
                            dtrow["AccountCountry"] = Convert.ToString(dtXLS.Rows[i]["Account Country"]);
                            dtrow["Owner"] = Convert.ToString(dtXLS.Rows[i]["Owner"]);
                            dtrow["Contact"] = Convert.ToString(dtXLS.Rows[i]["Contact"]);
                            dtrow["Creator"] = Convert.ToString(dtXLS.Rows[i]["Creator"]);
                            string ProfitOrLoss = Convert.ToString(dtXLS.Rows[i]["Profit"]);
                            if (string.IsNullOrEmpty(ProfitOrLoss) || ProfitOrLoss == "")
                            {
                                dtrow["ProfitOrLoss"] = 0;
                            }
                            else
                            {
                                dtrow["ProfitOrLoss"] = ProfitOrLoss;
                            }

                            dtrow["EventDate"] = null;
                            dtrow["ScioPayAccountId"] = null;

                            string AccountId = Convert.ToString(dtXLS.Rows[i]["Account Id"]);
                            if (AccountId != "" && AccountId != null)
                            {
                                dtrow["AccountId"] = Convert.ToString(dtXLS.Rows[i]["Account Id"]);
                            }
                            else
                            {
                                ErrorNotification("Account Number cannot be null at row " + (i + 1));
                                return RedirectToAction("UploadDeals", "Deal");
                            }

                            ClientMasterModel Client = _DealService.GetClientIdbyCompanyName(companyName, model.DealSource, AccountId);
                            dtrow["ClientId"] = Client.ClientId;
                            dtrow["ClientName"] = Client.ContactName;
                            dtrow["ExsistingCompanyName"] = Client.CompanyName;
                            dtrow["IsMatch"] = Client.Is_match;


                            decimal partnersellamount = 0; decimal clientsellamount = 0; decimal clientbuyamount = 0; decimal partnerbuyamount = 0;
                            if (!string.IsNullOrEmpty(clientSoldAmt))
                                clientsellamount = Convert.ToDecimal(clientSoldAmt);
                            if (!string.IsNullOrEmpty(partnerSoldAmount) && partnerSoldAmount != "0.00")
                            {
                                partnersellamount = Convert.ToDecimal(partnerSoldAmount);
                            }
                            else
                            {
                                if (fixedSide == "sell")
                                    partnersellamount = Convert.ToDecimal(clientSoldAmt);
                                else
                                    partnersellamount = Convert.ToDecimal(clientBoughtAmt) * Convert.ToDecimal(midMarketRate);
                            }

                            if (clientsellamount != partnersellamount)
                            {
                                if ((clientsellamount - partnersellamount) < 0)
                                    dtrow["GrossComm"] = 0;
                                else
                                    dtrow["GrossComm"] = clientsellamount - partnersellamount;

                                dtrow["GrossCommCurrency"] = clientSoldCurrency;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(clientBoughtAmt))
                                    clientbuyamount = Convert.ToDecimal(clientBoughtAmt);

                                if (!string.IsNullOrEmpty(partnerBoughtAmt) && partnerBoughtAmt != "0.00")
                                    partnerbuyamount = Convert.ToDecimal(partnerBoughtAmt);
                                else
                                {
                                    if (fixedSide == "buy")
                                        partnerbuyamount = Convert.ToDecimal(clientBoughtAmt);
                                    else
                                        partnerbuyamount = Convert.ToDecimal(clientSoldAmt) * Convert.ToDecimal(midMarketRate);
                                }

                                //                                conversion.GrossComm = partnerbuyamount - clientbuyamount;
                                if ((partnerbuyamount - clientbuyamount) < 0)
                                    dtrow["GrossComm"] = 0;
                                else
                                    dtrow["GrossComm"] = partnerbuyamount - clientbuyamount;


                                dtrow["GrossCommCurrency"] = clientBoughtCurrency;

                                //Gross Comm GBP

                                //if (Convert.ToString(dtrow["GrossCommCurrency"]).ToUpper() == "GBP")
                                //    dtrow["GrossCommGBP"] = dtrow["GrossComm"];
                                //else if (clientBoughtCurrency == "GBP") //This means the profit currency is sell currency
                                //{
                                //    dtrow["GrossCommGBP"] = Convert.ToDecimal(dtrow["GrossComm"]) * Convert.ToDecimal(midMarketRate); // TODO check - may be we will have to do 1/MarketRate
                                //}
                                //else if (clientSoldCurrency == "GBP") //This means the profit currency is buy currency
                                //{
                                //    dtrow["GrossCommGBP"] = Convert.ToDecimal(dtrow["GrossComm"]) * (1 / Convert.ToDecimal(midMarketRate)); // TODO check - may be we will have to do MarketRate
                                //}
                                //else
                                //{
                                //    //Get GBP Rate via Currency Cloud API
                                //    var resultRate = await GetRates(Convert.ToString(dtrow["GrossCommCurrency"]), "GBP");
                                //    if (resultRate > 0)
                                //        dtrow["GrossCommGBP"] = Convert.ToDecimal(dtrow["GrossComm"]) * resultRate;
                                //}


                                if (clientSoldCurrency != "GBP")
                                {
                                    decimal resultRate = await GetRates(clientSoldCurrency, "GBP");
                                    decimal Rate = Math.Round(resultRate, 4);
                                    if (resultRate > 0)
                                    {
                                        decimal x = Rate * Convert.ToDecimal(clientSoldAmt);
                                        dtrow["ClientSoldGBP"] = Convert.ToString(x);
                                        dtrow["GBPConversationRate"] = Rate.ToString();
                                    }

                                    //conversion.GBPConversationRate = resultRate.ToString();

                                    // var x = Convert.ToDecimal(conversion.mid_market_rate) * Convert.ToDecimal(conversion.client_sell_amount);
                                    //conversion.client_sell_amount_In_GBP = (Convert.ToDecimal(conversion.mid_market_rate) * Convert.ToInt64(conversion.client_sell_amount)).ToString();

                                }
                                else
                                {
                                    dtrow["ClientSoldGBP"] = clientSoldAmt;
                                }
                            }



                            dtResult.Rows.Add(dtrow);

                        }
                    }

                    string result = _DealService.SaveUploadScioPay(dtResult, model);

                    if (result == "")
                    {
                        SuccessNotification("Record Saved Successfully");
                    }
                    else
                    {
                        ErrorNotification("Error Occur record not saved successfully");
                    }

                }
            }


            catch (Exception ex)
            {
                string path = Server.MapPath("~/ImportScioPayDealLog.txt");

                if (!System.IO.File.Exists(path))
                {
                    System.IO.File.Create(path).Dispose();
                }
                System.IO.File.AppendAllText(Server.MapPath("~/ImportScioPayDealLog.txt"), "---------------------" + Environment.NewLine);
                System.IO.File.AppendAllText(Server.MapPath("~/ImportScioPayDealLog.txt"), "SaveUploadDealAsync method" + ex.Message + Environment.NewLine);

                System.IO.File.AppendAllText(Server.MapPath("~/ImportScioPayDealLog.txt"), "RequestDate: " + DateTime.Now.ToString() + Environment.NewLine);
                System.IO.File.AppendAllText(Server.MapPath("~/ImportScioPayDealLog.txt"), "---------------------" + Environment.NewLine);

                ErrorNotification("Something went wrong");
                return RedirectToAction("UploadDeals", "Deal");
            }
            return RedirectToAction("UploadDeals", "Deal");
        }

        public ActionResult GetUnmatchReconciliationDeal([DataSourceRequest] DataSourceRequest request, string Source)
        {
            int pageNo = request.Page;
            int pageSize = request.PageSize;
            string sortColumn = string.Empty;
            string sortDir = string.Empty;

            List<ReconciliationMissingOrMismatchModel> result = _DealService.GetUnmatchReconciliationDeal(Source, pageNo, pageSize, sortColumn, sortDir);
            DataSourceResult obj = new DataSourceResult();
            obj.Data = result;
            if (result != null && result.Count > 0)
            {
                obj.Total = result.FirstOrDefault().TotalCount;
            }
            //return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTempDealList([DataSourceRequest] DataSourceRequest request, string DealNo, string CompanyName, string AccountId, string Source = null, string IsMatch = null)
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

            List<DealModel> pagedDealList = _DealService.GetTempDeallist(pageNo, pageSize, sortColumn, sortDir, DealNo, CompanyName, AccountId, Source, IsMatch);
            if (pagedDealList != null && pagedDealList.Count > 0)
            {
                DataSourceResult obj = new DataSourceResult();
                obj.Data = pagedDealList;
                if (pagedDealList.Count > 0)
                {
                    obj.Total = pagedDealList.FirstOrDefault().TotalCount;
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<DealModel>(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImportTempDeal(long ImportDealId)
        {
            DealModel result = _DealService.GetTempDealById(ImportDealId);
            if (result != null)
            {
                int response = _DealService.ImportTempDeal(result);
                if (response != 0)
                {
                    SuccessNotification("Deal imported Successfully");
                }
                else
                {
                    ErrorNotification("Error Occur while importing deal");
                }
            }

            return RedirectToAction("UploadDeals", "Deal");
        }
        public ActionResult ImportAllDeals(bool allCompletlyMatchDeal, bool allPartiallyMatchDeal)
        {

            int result = _DealService.ImportAllDeal(allCompletlyMatchDeal, allPartiallyMatchDeal);

            switch (result)
            {
                case 0:
                    return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { Message = "All deals imported successfully.", IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
            }

            //return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ImportSelectedDeals(string[] Deal)
        {
            try
            {
                int result = 0;
                string deal = string.Empty;
                deal = string.Join(",", Deal);
                result = _DealService.ImportSelectedDeal(deal);
                switch (result)
                {
                    case 0:
                        return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                    default:
                        return Json(new { Message = "All deals imported successfully.", IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ErrorNotification("Something went wrong");
                return Json(new { Message = "Something went wrong", IsError = Enums.NotifyType.Error }
                            , JsonRequestBehavior.AllowGet);
            }
        }

    }
}

