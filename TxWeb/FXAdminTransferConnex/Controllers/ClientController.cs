using FXAdminTransferConnex.Business.Client;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnex.Entities.LocalizationResource;
using FXAdminTransferConnex.Common;
using System.Globalization;
using System.Threading.Tasks;
using System.Reflection;
using FXAdminTransferConnex.Business.Trader;
using FXAdminTransferConnex.Business.Report;
using System.IO;
using System.Data.OleDb;
using System.Data;
using log4net;
using System.Text;

namespace FXAdminTransferConnexApp.Controllers
{
    public class ClientController : BaseAdminController
    {
        #region Initializations

        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly IClientService _clientService;
        private readonly ITraderService _traderService;
        private readonly IReportService _reportService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientController"/> class.
        /// </summary>
        public ClientController()
        {
            _clientService = EngineContext.Resolve<IClientService>();
            _traderService = EngineContext.Resolve<ITraderService>();
            _reportService = EngineContext.Resolve<IReportService>();
        }
        #endregion

        // GET: Client
        public ActionResult Index()
        {
            ViewBag.IsAwaitingAction = "2";
            ViewBag.IsMarketOrder = "2";

            if (!string.IsNullOrEmpty(Request.QueryString["IsAwaitingAction"]))
                ViewBag.IsAwaitingAction = Request.QueryString["IsAwaitingAction"].ToString();

            if (!string.IsNullOrEmpty(Request.QueryString["IsMarketOrder"]))
                ViewBag.IsMarketOrder = Request.QueryString["IsMarketOrder"].ToString();

            return View();
        }
        // GET: ClientView
        public ActionResult ViewClient(long ClientId = 0)
        {
            ClientMasterModel model = new ClientMasterModel();
            ClientMasterModel clientModel = _clientService.GetClientDetailsByClientId(ClientId);
            CategoryModel categoryModel = _clientService.GetBusinessCategoryByCategoryId(clientModel.CategoryId);
            SectorModel sectorModel = _clientService.GetBusinessCategorySectorbySectorID(clientModel.SectorId);
            if (clientModel.TraderId != null)
            {
                TraderModel Trader = _traderService.GetTraderDetailsByTraderId((long)clientModel.TraderId);
                clientModel.TraderName = Trader.FirstName + " " + Trader.LastName;
            }
            if (clientModel.SalesPersonId != null)
            {
                UserModel SalesPerson = _reportService.GetSalesPersonDetailsById((long)clientModel.SalesPersonId);
                clientModel.SalesPersonName = SalesPerson.FullName;
            }
            if (clientModel.BoughtCurrencies != null)
            {
                string[] boughtCCID = clientModel.BoughtCurrencies.Split(new Char[] { ',' });
                //string boughtCurrencies = " ";
                StringBuilder boughtCurrencies = new StringBuilder();

                foreach (string CCID in boughtCCID)
                {
                    CurrencyModel Bought_currency = _clientService.GetCurrencyDetailById(Convert.ToInt32(CCID));
                    if (boughtCurrencies.ToString() != " ")
                    {
                        boughtCurrencies.Append(",").Append(Bought_currency.CCY);
                        //boughtCurrencies = boughtCurrencies + "," + Bought_currency.CCY;
                    }
                    else
                    {
                        boughtCurrencies.Append(Bought_currency.CCY);
                        //boughtCurrencies += Bought_currency.CCY;
                    }

                }
                clientModel.BoughtCurrencies = boughtCurrencies.ToString();
            }
            if (clientModel.SoldCurrencies != null)
            {
                string[] soldCCID = clientModel.SoldCurrencies.Split(new Char[] { ',' });
                //string soldCurrencies = " ";
                StringBuilder soldCurrencies = new StringBuilder();

                foreach (string CCID in soldCCID)
                {
                    CurrencyModel Sold_currency = _clientService.GetCurrencyDetailById(Convert.ToInt32(CCID));
                    if (soldCurrencies.ToString() != " ")
                    {
                        soldCurrencies.Append(",").Append(Sold_currency.CCY);
                        //soldCurrencies = soldCurrencies + "," + Sold_currency.CCY;
                    }
                    else
                    {
                        soldCurrencies.Append(Sold_currency.CCY);
                        //soldCurrencies += Sold_currency.CCY;
                    }

                }
                clientModel.SoldCurrencies = soldCurrencies.ToString();
            }
            if (clientModel.Currencies != null)
            {
                string[] tradeCCID = clientModel.Currencies.Split(new Char[] { ',' });
                //string tradeCurrencies = " ";
                StringBuilder tradeCurrencies = new StringBuilder();
                foreach (string CCID in tradeCCID)
                {
                    CurrencyModel Trade_currency = _clientService.GetCurrencyDetailById(Convert.ToInt32(CCID));
                    if (tradeCurrencies.ToString() != " ")
                    {
                        tradeCurrencies.Append(",").Append(Trade_currency.CCY);
                        //tradeCurrencies = tradeCurrencies + "," + Trade_currency.CCY;
                    }
                    else
                    {
                        tradeCurrencies.Append(Trade_currency.CCY);
                        //tradeCurrencies += Trade_currency.CCY;
                    }

                }
                clientModel.Currencies = tradeCurrencies.ToString();
            }
            if (clientModel.LeadCategoryId != null)
            {
                LeadCategoryModel LeadCategory = _clientService.GetLeadCategoryDetailById((long)clientModel.LeadCategoryId);
                clientModel.LeadCategory = LeadCategory.LeadCategory;
            }
            if (categoryModel != null)
            {
                clientModel.CategoryName = categoryModel.CategoryName;
            }
            if (sectorModel != null)
            {
                clientModel.SectorName = sectorModel.SectorName;
            }

            if (clientModel != null)
            {
                model = clientModel;
                ViewBag.isViewClient = true;
            }
            return View(model);
        }
        /// <summary>
        /// By Mayank
        /// 12 March 2018
        /// Get client list
        /// </summary>
        /// <param name="request"></param>
        /// <param name="FullName"></param>
        /// <param name="CompanyName"></param>
        /// <param name="AccountNo"></param>
        /// <param name="EmailAddress"></param>
        /// <param name="AwaitingAction"></param>
        /// <param name="MarketOrder"></param>
        /// <returns></returns>
        public ActionResult GetClientList([DataSourceRequest] DataSourceRequest request, string FullName, string CompanyName, string AccountNo, string EmailAddress, string CommunicationDetail, long? SearchSalesPersonId, long? SearchJuniorSalesPersonId, long? SearchTraderId, string BoughtCurrency, string SoldCurrency, long? LeadCategoryId, long? SectorCategoryId, long? BusinessCategoryId, string AwaitingAction = "2", string MarketOrder = "2", string ClientSource = null)
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

            long SalesPersonId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode() ? ProjectSession.LoginUserDetails.UserId : 0;
            //var traderId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
           // var traderId = 0;
            if (SalesPersonId > 0)
            {
                SearchSalesPersonId = SalesPersonId;
            }
            List<ClientMasterModel> clientList = _clientService.GetClientlist(pageNo, pageSize, sortColumn, sortDir, FullName, CompanyName, "", AccountNo, EmailAddress, CommunicationDetail, SearchTraderId, SearchSalesPersonId, SearchJuniorSalesPersonId, AwaitingAction, MarketOrder, BoughtCurrency, SoldCurrency, LeadCategoryId, SectorCategoryId, BusinessCategoryId,ClientSource);

            if (clientList != null && clientList.Count > 0)
            {
                DataSourceResult obj = new DataSourceResult();
                obj.Data = clientList;
                if (clientList.Count > 0)
                {
                    obj.Total = clientList.FirstOrDefault().TotalCount;
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }

            return Json(new List<ClientMasterModel>(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 12 March 2018
        /// Get client details by ClientId
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageClient(long ClientId = 0, bool displayOnlyMarketOrder = false,string CompanyName = "",string Source = "")
        {
            ClientMasterModel model = new ClientMasterModel();
            ClientMasterModel clientModel = _clientService.GetClientDetailsByClientId(ClientId);
            if (clientModel != null)
            {
                model = clientModel;
                ViewBag.isViewClient = false;
                model.ClientSource = model.Source;
            }
            else
            {
                model.CompanyName = CompanyName;
                model.ClientSource = Source;
            }
            return View(model);
        }

        /// <summary>
        /// By Mayank
        /// 12 March 2018
        /// Save Client Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ManageClient(ClientMasterModel model, FormCollection formdata)
        {
            if (formdata["IsActive"] != null)
            {
                model.IsActive = true;
            }

            if (formdata["Currencies"] != null)
            {
                model.Currencies = formdata["Currencies"].ToString();
            }
            if (formdata["BoughtClientCurrencies"] != null)
            {
                model.BoughtCurrencies = formdata["BoughtClientCurrencies"].ToString();
            }
            if (formdata["SoldClientCurrencies"] != null)
            {
                model.SoldCurrencies = formdata["SoldClientCurrencies"].ToString();
            }

            if (formdata["IsAwaitingAction"] != null)
            {
                model.IsAwaitingAction = true;
            }

            if (formdata["IsMarketOrder"] != null)
            {
                model.IsMarketOrder = true;
            }

            long result = _clientService.AddClient(model);


            switch (result)
            {
                case -2:
                    ErrorNotification("Client record already exist.");
                    break;
                case 0:
                    ErrorNotification("Error Occur record not saved successfully");
                    break;
                default:
                    if (model.ClientId > 0)
                        SuccessNotification("Client Updated Successfully");
                    else
                        SuccessNotification("Client Saved Successfully");
                    break;
            }
            return RedirectToAction("ManageClient", new { ClientId = result });

        }

        /// <summary>
        /// By Mayank
        /// 16 April 2018
        /// Save Awaiting Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveAwatingAction(ClientMasterModel model, FormCollection formdata)
        {
            if (formdata["IsAwaitingAction"] != null)
            {
                model.IsAwaitingAction = true;
            }
            if (formdata["IsMarketOrder"] != null)
            {
                model.IsMarketOrder = true;
            }
            if (formdata["Currencies"] != null)
            {
                model.Currencies = formdata["Currencies"].ToString();
            }
            // 
            if (formdata["BoughtClientCurrencies"] != null)
            {
                model.BoughtCurrencies = formdata["BoughtClientCurrencies"].ToString();
            }
            if (formdata["SoldClientCurrencies"] != null)
            {
                model.SoldCurrencies = formdata["SoldClientCurrencies"].ToString();
            }
            // 
            int result = _clientService.SaveAwaitingAction(model);

            if (result > 0)
            {
                SuccessNotification("Client Updated Successfully");
            }
            else
            {
                ErrorNotification("Error Occur record not saved successfully");
            }


            return RedirectToAction("ManageClient", new { ClientId = result });
        }

        /// <summary>
        /// By Mayank
        /// 16 March 2018
        /// Delete client details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult DeleteClient(long clientId)
        {
            int result = _clientService.DeleteClient(clientId);

            switch (result)
            {
                case -2:
                    return Json(new { Message = "Client link with staging deal", IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                case 0:
                    return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ChangeMarketOrderStatus(int ClientId, bool status)
        {
            bool result = _clientService.ChangeMarketOrderStatus(ClientId, status);
            return Json(result
                ? new { Message = FXAdminTransferConnexResource.StatusSuccess, IsError = Enums.NotifyType.Success }
                : new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 27 March 2018
        /// Gets the list of Client Deal
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetClientDealList([DataSourceRequest] DataSourceRequest request, long ClientId = 0)
        {
            List<DealModel> clientDealList = _clientService.GetClientDeallist(ClientId);
            return Json(clientDealList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// By Mayank
        /// 16 July 2019
        /// Get Task Reminder list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetTaskReminderList([DataSourceRequest] DataSourceRequest request, long ClientId = 0)
        {
            List<TaskReminderModel> clientDealList = _clientService.GetTaskReminderlist(ClientId);
            return Json(clientDealList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveTaskReminder([DataSourceRequest] DataSourceRequest request, TaskReminderModel obj, string TaskDateTime)
        {
            string format = "dd/MM/yyyy HH:mm";
            DateTime dateTime1;
            if (DateTime.TryParseExact(TaskDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime1))
            {
                obj.TaskReminderDatetime = dateTime1;
                int result = _clientService.SaveTaskReminder(obj);

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

        public ActionResult DeleteTaskReminder([DataSourceRequest] DataSourceRequest request, TaskReminderModel obj)
        {
            bool result = _clientService.DeleteSaveTaskReminder(obj.TaskId);
            return Json(result
                ? new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }
                : new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadClients()
        {
            return View();
        }
        public ActionResult SaveUploadClient(HttpPostedFileBase UploadFormFile, ClientMasterModel model)
        {
            try
            {
                string directoryPath = System.Web.HttpContext.Current.Server.MapPath("~/TempFiles");
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }
                string filePath = directoryPath + @"\" + Guid.NewGuid().ToString() + ".xlsx";
                if (UploadFormFile != null)
                {
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
                sql = "SELECT * FROM [" + dtSheets.Rows[0]["Table_name"] + "]";
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

                if(model.Source == "ScioPay")
                {
                    DataTable dtResult = new DataTable();

                    dtResult.Columns.Add("AccountNo");
                    dtResult.Columns.Add("AccountStatus");
                    dtResult.Columns.Add("ClientType");
                    dtResult.Columns.Add("FullName");
                    dtResult.Columns.Add("CompanyName");
                    dtResult.Columns.Add("MobileNo");
                    dtResult.Columns.Add("EmailAddress");
                    dtResult.Columns.Add("AltMobileNo");
                    dtResult.Columns.Add("ScioPayClientId");
                    dtResult.Columns.Add("ScioPayAccountId");
                    dtResult.Columns.Add("SalesPersonId");
                    dtResult.Columns.Add("TraderId");

                    if (dtXLS != null)
                    {
                        if (dtXLS.Rows.Count > 0)
                        {
                            dtXLS = dtXLS.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull)).CopyToDataTable();
                        }
                        for (int i = 0; i < dtXLS.Rows.Count; i++)
                        {
                            DataRow dtrow = dtResult.NewRow();
                            dtrow["AccountNo"] = Convert.ToString(dtXLS.Rows[i]["AccountNo"]);
                            dtrow["AccountStatus"] = Convert.ToString(dtXLS.Rows[i]["AccountStatus"]);
                            dtrow["ClientType"] = Convert.ToString(dtXLS.Rows[i]["ClientType"]);
                            dtrow["FullName"] = Convert.ToString(dtXLS.Rows[i]["ContactFirstName"]) + " " + Convert.ToString(dtXLS.Rows[i]["ContactLastName"]);
                            dtrow["CompanyName"] = Convert.ToString(dtXLS.Rows[i]["CompanyName"]);
                            dtrow["MobileNo"] = Convert.ToString(dtXLS.Rows[i]["Telephone"]);
                            dtrow["EmailAddress"] = Convert.ToString(dtXLS.Rows[i]["Email"]);
                            dtrow["AltMobileNo"] = Convert.ToString(dtXLS.Rows[i]["CompanyTelephone"]);
                            dtrow["ScioPayClientId"] = null;
                            dtrow["ScioPayAccountId"] = Convert.ToString(dtXLS.Rows[i]["AccountNo"]);
                            string SalesPersonName = Convert.ToString(dtXLS.Rows[i]["SalesPersonName"]);

                            if(SalesPersonName != null && SalesPersonName != "")
                            {
                                long SalesPersonId = _clientService.GetSalespersonIdByName(SalesPersonName);
                                if(SalesPersonId > 0)
                                {
                                    dtrow["SalesPersonId"] = SalesPersonId;
                                }
                                else
                                {
                                    ErrorNotification("Please Enter Valid Sales Person Name at row: " + (i+1));
                                    return RedirectToAction("UploadClients", "Client");
                                }
                            }
                            else
                            {
                                dtrow["SalesPersonId"] = null;
                            }

                            string TraderName = Convert.ToString(dtXLS.Rows[i]["TraderName"]);
                            if (TraderName != null && TraderName != "")
                            {
                                long TraderId = _clientService.GetTraderIdByName(TraderName);
                                if (TraderId > 0)
                                {
                                    dtrow["TraderId"] = TraderId;
                                }
                                else
                                {
                                    ErrorNotification("Please Enter Valid Trader Name at row: " + (i + 1));
                                    return RedirectToAction("UploadClients", "Client");
                                }
                            }
                            else
                            {
                                dtrow["TraderId"] = null;
                            }

                            string accNo = String.Empty;
                            if (dtResult.Rows.Count > 0)
                            {
                                accNo = (from DataRow dr in dtResult.Rows
                                         where (string)dr["AccountNo"] == Convert.ToString(dtXLS.Rows[i]["AccountNo"])
                                         select (string)dr["AccountNo"]).FirstOrDefault();

                                if (accNo == null)
                                {
                                    dtResult.Rows.Add(dtrow);
                                }
                            }
                            else
                            {
                                dtResult.Rows.Add(dtrow);
                            }

                        }

                    }

                    string result = _clientService.SaveUploadScioPayClient(dtResult, model);
                    if (result == "")
                    {
                        SuccessNotification("Record Saved Successfully");
                    }
                    else
                    {
                        ErrorNotification("Error Occur record not saved successfully");
                    }
                }
                else if(model.Source == "GCPartner")
                {
                    DataTable dtResult = new DataTable();
                    dtResult.Columns.Add("ClientId");
                    dtResult.Columns.Add("FullName");
                    dtResult.Columns.Add("CompanyName");
                    dtResult.Columns.Add("AddressLine1");
                    dtResult.Columns.Add("AddressLine2");
                    dtResult.Columns.Add("TownCity");
                    dtResult.Columns.Add("CountryId");
                    dtResult.Columns.Add("EmailAddress");
                    dtResult.Columns.Add("Mobile");
                    dtResult.Columns.Add("Telephone");
                    dtResult.Columns.Add("DateAdded");
                    dtResult.Columns.Add("IsActive");
                    dtResult.Columns.Add("SalesPersonId");
                    dtResult.Columns.Add("TraderId");

                    if (dtXLS != null)
                    {
                        if (dtXLS.Rows.Count > 0)
                        {
                            dtXLS = dtXLS.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull)).CopyToDataTable();
                        }
                        for (int i = 0; i < dtXLS.Rows.Count; i++)
                        {
                            DataRow dtrow = dtResult.NewRow();
                            dtrow["ClientId"] = Convert.ToString(dtXLS.Rows[i]["ClientId"]);
                            dtrow["FullName"] = Convert.ToString(dtXLS.Rows[i]["FirstName"]) + " " + Convert.ToString(dtXLS.Rows[i]["LastName"]);
                            dtrow["CompanyName"] = Convert.ToString(dtXLS.Rows[i]["CompanyName"]);
                            dtrow["AddressLine1"] = Convert.ToString(dtXLS.Rows[i]["AddressLine1"]);
                            dtrow["AddressLine2"] = Convert.ToString(dtXLS.Rows[i]["AddressLine2"]);
                            dtrow["TownCity"] = Convert.ToString(dtXLS.Rows[i]["TownCity"]);
                            dtrow["CountryId"] = Convert.ToString(dtXLS.Rows[i]["CountryId"]);
                            dtrow["EmailAddress"] = Convert.ToString(dtXLS.Rows[i]["EmailAddress"]);
                            dtrow["Mobile"] = Convert.ToString(dtXLS.Rows[i]["Mobile"]);
                            dtrow["Telephone"] = Convert.ToString(dtXLS.Rows[i]["Telephone"]);
                            dtrow["DateAdded"] =string.IsNullOrEmpty(dtXLS.Rows[i]["DateAdded"].ToString()) ? null : DateTime.Parse(dtXLS.Rows[i]["DateAdded"].ToString()).ToString("MM/dd/yyyy");
                            dtrow["IsActive"] = Convert.ToBoolean(string.IsNullOrEmpty(dtXLS.Rows[i]["EmploymentStatus"].ToString())? false : true );

                            string SalesPersonName = Convert.ToString(dtXLS.Rows[i]["SalesPersonName"]);

                            if (SalesPersonName != null && SalesPersonName != "")
                            {
                                long SalesPersonId = _clientService.GetSalespersonIdByName(SalesPersonName);
                                if (SalesPersonId > 0)
                                {
                                    dtrow["SalesPersonId"] = SalesPersonId;
                                }
                                else
                                {
                                    ErrorNotification("Please Enter Valid Sales Person Name at row: " + (i + 1));
                                    return RedirectToAction("UploadClients", "Client");
                                }
                            }
                            else
                            {
                                dtrow["SalesPersonId"] = null;
                            }

                            string TraderName = Convert.ToString(dtXLS.Rows[i]["TraderName"]);
                            if (TraderName != null && TraderName != "")
                            {
                                long TraderId = _clientService.GetTraderIdByName(TraderName);
                                if (TraderId > 0)
                                {
                                    dtrow["TraderId"] = TraderId;
                                }
                                else
                                {
                                    ErrorNotification("Please Enter Valid Trader Name at row: " + (i + 1));
                                    return RedirectToAction("UploadClients", "Client");
                                }
                            }
                            else
                            {
                                dtrow["TraderId"] = null;
                            }


                            string clientId = String.Empty;
                            if (dtResult.Rows.Count > 0)
                            {

                                clientId = (from DataRow dr in dtResult.Rows
                                         where (string)dr["ClientId"] == Convert.ToString(dtXLS.Rows[i]["ClientId"])
                                         select (string)dr["ClientId"]).FirstOrDefault();

                                if (clientId == null)
                                {
                                    dtResult.Rows.Add(dtrow);
                                }
                            }
                            else
                            {
                                dtResult.Rows.Add(dtrow);
                            }
                        }

                    }

                    string result = _clientService.SaveUploadGCPartnerClient(dtResult, model);
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
                _logger.Error("SaveUploadClient:" + " " + ex.Message);
                ErrorNotification("Something went wrong");
                return RedirectToAction("UploadClients", "Client");
            }
            return RedirectToAction("Index", "Client");
        }

        #region Market Order Setting

        public ActionResult GetMarketOrderNotificationFilter()
        {
            List<NotificationSettingModel> filterList = _clientService.GetMarketOrderNotificationFilter().ToList();
            return Json(filterList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetConditionalOperatorList()
        {
            List<ConditionalOperatorModel> opertorList = _clientService.GetConditionalOperatorList().ToList();
            return Json(opertorList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveMarketOrderSetting([DataSourceRequest] DataSourceRequest request, MarketOrderSettingModel obj)
        {
            if (obj.FilterId != 3)
            {
                obj.EndDate = null;
            }
            long result = _clientService.SaveMarketOrderSetting(obj);

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

        public ActionResult GetMarketOrderSettingList([DataSourceRequest] DataSourceRequest request, string FullName, string EmailAddress, string CompanyName, string MarketOrderStatus, int? FromCurrency, int? ToCurrency, long ClientId = 0, long ProspectId = 0)
        {
            //if (string.IsNullOrEmpty(MarketOrderStatus))
            //{
            //    MarketOrderStatus = "Triggered,Created";
            //}
            long SalesPersonId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode() ? ProjectSession.LoginUserDetails.UserId : 0;
            //var TraderId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
            int TraderId = 0;
            List<MarketOrderSettingModel> clientMOList = _clientService.GetMarketOrderSettingList(FullName, EmailAddress, CompanyName, MarketOrderStatus, FromCurrency, ToCurrency, ClientId, ProspectId, TraderId, SalesPersonId);
            foreach (MarketOrderSettingModel item in clientMOList)
            {
                if (item.MarketRate > item.ClientRate)
                {
                    item.Operator = ">";
                }
                else if (item.MarketRate < item.ClientRate)
                {
                    item.Operator = "<";
                }
                else if (item.MarketRate >= item.ClientRate)
                {
                    item.Operator = ">=";
                }
                else
                {
                    item.Operator = "<=";
                }
            }
            return Json(clientMOList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MarketOrderSettingUpdateStatus(int ID, string Status)
        {
            bool result = _clientService.UpdateMarketOrderSettingStatus(ID, Status);
            return Json(result
                    ? new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }
                    : new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteMarketOrderSetting([DataSourceRequest] DataSourceRequest request, MarketOrderSettingModel obj)
        {
            bool result = _clientService.DeleteMarketOrderSetting(obj.MarketOrderId);
            return Json(result
                    ? new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }
                    : new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                , JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<bool> MarketOrderTriggerWithGCPartner()
        {

            bool result = false;
            try
            {
                List<MarketOrderSettingModel> clientMOList = _clientService.GetMarketOrderSettingList(null, null, null, null, null, null, 0);
                if (clientMOList != null && clientMOList.Count > 0)
                {
                    foreach (MarketOrderSettingModel item in clientMOList)
                    {
                        try
                        {
                            GCPartnerMarketOrderRateModel model = new GCPartnerMarketOrderRateModel();

                            string url = "GetInterbankRates";

                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            dic.Add("Currency", item.FromCurrencyName);

                            GCPartnerMarketOrderRateModel objMarketRate = await WebApiHelper.HttpClientPostGCPartnerToken(model, url, dic);
                            if (objMarketRate != null && objMarketRate.ReturnObject != null)
                            {
                                Type myType = objMarketRate.ReturnObject.GetType();
                                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                                decimal currentMarketRate = 0;
                                foreach (PropertyInfo prop in props)
                                {
                                    if (prop.Name.ToUpper() == item.ToCurrencyName.ToUpper())
                                    {
                                        currentMarketRate = Convert.ToDecimal(prop.GetValue(objMarketRate.ReturnObject, null));
                                        break;
                                    }
                                }

                                if (currentMarketRate > 0)
                                {
                                    switch (item.Operator)
                                    {
                                        case "<":
                                            if (item.Filter.ToUpper() == "BEFORE" &&
                                                item.StartDate > DateTime.Now &&
                                                (currentMarketRate < item.MarketRate))
                                            {

                                                SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                                            }
                                            else if (item.Filter.ToUpper() == "AFTER" &&
                                                     item.StartDate < DateTime.Now &&
                                                     (currentMarketRate < item.MarketRate))
                                            {
                                                SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                                            }
                                            else if (item.Filter.ToUpper() == "RANGE" &&
                                                    (item.StartDate >= DateTime.Now &&
                                                      item.EndDate <= DateTime.Now) &&
                                                    currentMarketRate < item.MarketRate)
                                            {
                                                SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                                            }

                                            break;
                                        case ">":
                                            if (item.Filter.ToUpper() == "BEFORE" &&
                                                item.StartDate > DateTime.Now &&
                                                (currentMarketRate > item.MarketRate))
                                            {
                                                SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                                            }
                                            else if (item.Filter.ToUpper() == "AFTER" &&
                                                     item.StartDate < DateTime.Now &&
                                                     (currentMarketRate > item.MarketRate))
                                            {
                                                SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                                            }
                                            else if (item.Filter.ToUpper() == "RANGE" &&
                                                     (item.StartDate >= DateTime.Now &&
                                                      item.EndDate <= DateTime.Now) &&
                                                     currentMarketRate > item.MarketRate)
                                            {
                                                SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                                            }
                                            break;
                                        case "<=":
                                            if (item.Filter.ToUpper() == "BEFORE" &&
                                                item.StartDate > DateTime.Now &&
                                                (currentMarketRate <= item.MarketRate))
                                            {
                                                SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                                            }
                                            else if (item.Filter.ToUpper() == "AFTER" &&
                                                     item.StartDate < DateTime.Now &&
                                                     (currentMarketRate <= item.MarketRate))
                                            {
                                                SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                                            }
                                            else if (item.Filter.ToUpper() == "RANGE" &&
                                                     (item.StartDate >= DateTime.Now &&
                                                      item.EndDate <= DateTime.Now) &&
                                                     currentMarketRate <= item.MarketRate)
                                            {
                                                SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                                            }
                                            break;
                                        case ">=":
                                            if (item.Filter.ToUpper() == "BEFORE" &&
                                                item.StartDate > DateTime.Now &&
                                                (currentMarketRate >= item.MarketRate))
                                            {
                                                SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                                            }
                                            else if (item.Filter.ToUpper() == "AFTER" &&
                                                     item.StartDate < DateTime.Now &&
                                                     (currentMarketRate >= item.MarketRate))
                                            {
                                                SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                                            }
                                            else if (item.Filter.ToUpper() == "RANGE" &&
                                                     (item.StartDate >= DateTime.Now &&
                                                      item.EndDate <= DateTime.Now) &&
                                                     currentMarketRate >= item.MarketRate)
                                            {
                                                SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            result = true;
                        }
                        catch (Exception e)
                        {
                            _logger.Error("ClientContoller -> MarketOrderTriggerWithGCPartner -> Inner Catch:"+" "+e.Message);
                            result = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("ClientContoller -> MarketOrderTriggerWithGCPartner -> Outer Catch:" + " " + ex.Message);
                result = false;
            }
            return result;
        }


        public void SaveMarketOrderClientNotification(long clientId, long marketOrderId)
        {
            try
            {
                _clientService.SaveMarketOrderNotification(clientId, marketOrderId);
            }
            catch (Exception e)
            {
                _logger.Error("ClientContoller -> SaveMarketOrderClientNotification :" + " " + e.Message);
            }
        }

        #endregion

        #region Ring Central

        [HttpGet]
        public ActionResult RingCentralClient()
        {
            return View();
        }

        public ActionResult GetClientDetailByMobile(string MobileNo)
        {
            RingCentralClientModel data = _clientService.GetClientDetailByMobile(MobileNo);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidateClientByRingCentralMobileAndSalesPerson(string MobileNo)
        {
            long SalesPersonId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode() ? ProjectSession.LoginUserDetails.UserId : 0;
            int data = 1;
            if (SalesPersonId > 0)
            {
                data = _clientService.ValidateClientByRingCentralMobileAndSalesPerson(MobileNo, SalesPersonId);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult UpdatePastCommDetail(long ClientId, string PastCommDetail)
        {
            bool result = _clientService.UpdatePastCommDetail(ClientId, PastCommDetail);
            return Json(result
                ? new { Message = FXAdminTransferConnexResource.StatusSuccess, IsError = Enums.NotifyType.Success }
                : new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                , JsonRequestBehavior.AllowGet);
        }
    }
}