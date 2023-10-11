using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Data.Models;
using FXAdminTransferConnex.Data.Repository;
using FXAdminTransferConnex.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FXAdminTransferConnex.Business.Deal
{
    public class DealService : IDealService
    {
        #region Constants

        /// <summary>
        /// Declare The logger object for perform operation on Logger
        /// </summary>
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Fields

        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<UserMaster> _usersRepository;

        #endregion

        #region ctor

        public DealService(IRepository<UserMaster> providerRepository)
        {
            _usersRepository = providerRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Get Deal details list
        /// </summary>
        /// <returns></returns>
        public List<DealModel> GetDeallist(int pageNo, int pageSize, string sortColumn, string sortDir, string DealNo, string CompanyName, DateTime? FromDate = null, DateTime? ToDate = null, long SalesPersonId = 0, long JuniorSalesPersonId = 0, long TraderId = 0, string TStatus = null, string Source = null)
        {
            SqlParameter paraPageNo = new SqlParameter
            {
                ParameterName = "PageNo",
                DbType = DbType.Int16,
                Value = pageNo
            };

            SqlParameter paraPageSize = new SqlParameter
            {
                ParameterName = "PageSize",
                DbType = DbType.Int16,
                Value = pageSize
            };

            SqlParameter paraSortColumn = new SqlParameter
            {
                ParameterName = "SortColumn",
                DbType = DbType.String,
                Value = sortColumn
            };

            SqlParameter paraSortOrder = new SqlParameter
            {
                ParameterName = "SortOrder",
                DbType = DbType.String,
                Value = sortDir
            };

            SqlParameter paraEmpId = new SqlParameter
            {
                ParameterName = "EmpId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };

            SqlParameter paraDealNo = new SqlParameter
            {
                ParameterName = "DealNo",
                DbType = DbType.String,
                Value = (object)DealNo ?? DBNull.Value
            };
            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = (object)CompanyName ?? DBNull.Value
            };
            SqlParameter paraFromDate = new SqlParameter
            {
                ParameterName = "FromDate",
                DbType = DbType.DateTime,
                Value = (object)FromDate ?? DBNull.Value
            };
            SqlParameter paraToDate = new SqlParameter
            {
                ParameterName = "ToDate",
                DbType = DbType.DateTime,
                Value = (object)ToDate ?? DBNull.Value
            };


            SqlParameter paraSalesPersonId = new SqlParameter
            {
                ParameterName = "SalesPersonId",
                DbType = DbType.Int64,
                Value = (object)SalesPersonId ?? DBNull.Value
            };
            SqlParameter paraJuniorSalesPersonId = new SqlParameter
            {
                ParameterName = "JuniorSalesPersonId",
                DbType = DbType.Int64,
                Value = (object)JuniorSalesPersonId ?? DBNull.Value
            };


            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)TraderId ?? DBNull.Value
            };

            SqlParameter paraTStatus = new SqlParameter
            {
                ParameterName = "TStatus",
                DbType = DbType.String,
                Value = (object)TStatus ?? DBNull.Value
            };
            SqlParameter paraSource = new SqlParameter
            {
                ParameterName = "Source",
                DbType = DbType.String,
                Value = (object)Source ?? DBNull.Value
            };


            List<DealModel> Deallist = _usersRepository.ExecuteStoredProcedureList<DealModel>("usp_Deal_List", paraPageNo, paraPageSize, paraSortColumn, paraSortOrder, paraEmpId, paraDealNo, paraCompanyName, paraFromDate, paraToDate, paraSalesPersonId, paraTraderId, paraTStatus, paraSource, paraJuniorSalesPersonId).ToList();
            return Deallist;
        }

        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Get Deal details list by DealId
        /// </summary>
        /// <returns></returns>
        public DealModel GetDealDetailsByDealId(long dealId)
        {
            SqlParameter paraDealId = new SqlParameter
            {
                ParameterName = "DealId",
                DbType = DbType.Int64,
                Value = dealId
            };

            DealModel List = _usersRepository.ExecuteStoredProcedureList<DealModel>("usp_Deal_GetDetailByDealId", paraDealId).FirstOrDefault();
            return List;
        }


        /// <summary>
        /// By Mayank
        /// 15 MArch 2018
        /// Save deal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SaveDeal(DealModel model)
        {
            SqlParameter paraDealId = new SqlParameter
            {
                ParameterName = "DealId",
                DbType = DbType.Int64,
                Value = (object)model.DealId ?? DBNull.Value
            };

            SqlParameter paraDealNo = new SqlParameter
            {
                ParameterName = "DealNo",
                DbType = DbType.String,
                Value = (object)model.DealNo ?? DBNull.Value
            };

            SqlParameter paraContactName = new SqlParameter
            {
                ParameterName = "ContactName",
                DbType = DbType.String,
                Value = (object)model.ContactName ?? DBNull.Value
            };

            SqlParameter paraTradeDate = new SqlParameter
            {
                ParameterName = "TradeDate",
                DbType = DbType.DateTime,
                Value = (object)model.TradeDate ?? DBNull.Value
            };

            SqlParameter paraValueDate = new SqlParameter
            {
                ParameterName = "ValueDate",
                DbType = DbType.DateTime,
                Value = (object)model.ValueDate ?? DBNull.Value
            };

            SqlParameter paraClientSoldAmt = new SqlParameter
            {
                ParameterName = "ClientSoldAmt",
                DbType = DbType.Decimal,
                Value = (object)model.ClientSoldAmt ?? DBNull.Value
            };

            SqlParameter paraClientSoldCCY = new SqlParameter
            {
                ParameterName = "ClientSoldCCY",
                DbType = DbType.String,
                Value = (object)model.ClientSoldCCY ?? DBNull.Value
            };

            SqlParameter paraClientSoldGBP = new SqlParameter
            {
                ParameterName = "ClientSoldGBP",
                DbType = DbType.Decimal,
                Value = (object)model.ClientSoldGBP ?? DBNull.Value
            };

            SqlParameter paraClientBoughtAmt = new SqlParameter
            {
                ParameterName = "ClientBoughtAmt",
                DbType = DbType.Decimal,
                Value = (object)model.ClientBoughtAmt ?? DBNull.Value
            };

            SqlParameter paraClientBoughtCCY = new SqlParameter
            {
                ParameterName = "ClientBoughtCCY",
                DbType = DbType.String,
                Value = (object)model.ClientBoughtCCY ?? DBNull.Value
            };

            SqlParameter paraGrossCommGBP = new SqlParameter
            {
                ParameterName = "GrossCommGBP",
                DbType = DbType.Decimal,
                Value = (object)model.GrossCommGBP ?? DBNull.Value
            };

            SqlParameter paraWLTotalCommGBP = new SqlParameter
            {
                ParameterName = "WLTotalCommGBP",
                DbType = DbType.Decimal,
                Value = (object)model.WLTotalCommGBP ?? DBNull.Value
            };

            SqlParameter paraTradeType = new SqlParameter
            {
                ParameterName = "TradeType",
                DbType = DbType.String,
                Value = (object)model.TradeType ?? DBNull.Value
            };

            SqlParameter paraTStatus = new SqlParameter
            {
                ParameterName = "TStatus",
                DbType = DbType.String,
                Value = (object)model.TStatus ?? DBNull.Value
            };

            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };

            SqlParameter paraGrossCommGBPFinal = new SqlParameter
            {
                ParameterName = "GrossCommGBPFinal",
                DbType = DbType.Decimal,
                Value = (object)model.GrossCommGBPFinal ?? DBNull.Value
            };
            SqlParameter paraPLAmount = new SqlParameter
            {
                ParameterName = "AdditionalPLAmount",
                DbType = DbType.Decimal,
                Value = (object)model.AdditionalPLAmount ?? DBNull.Value
            };
            SqlParameter paraPLnotes = new SqlParameter
            {
                ParameterName = "AdditionalPLNotes",
                DbType = DbType.String,
                Value = (object)model.AdditionalPLNotes ?? DBNull.Value
            };


            int result = 0;
            if (model.DealId > 0)
            {
                result = _usersRepository.ExecuteStoredProcedure<int>("usp_Deal_update", paraDealId, paraDealNo, paraContactName, paraTradeDate, paraValueDate, paraClientSoldAmt, paraClientSoldCCY, paraClientSoldGBP, paraClientBoughtAmt, paraClientBoughtCCY, paraGrossCommGBP, paraGrossCommGBPFinal, paraWLTotalCommGBP, paraTradeType, paraTStatus, paraUserId, paraPLAmount, paraPLnotes).FirstOrDefault();
            }

            return result;
        }


        public bool InsertImportLog(DateTime FromDate, DateTime ToDate, int NoofDeals, string DealShortReference, bool ImportStatus, string ImportBy, string DealSource)
        {
            SqlParameter paraImportDate = new SqlParameter
            {
                ParameterName = "ImportDate",
                DbType = DbType.DateTime,
                Value = DateTime.UtcNow
            };

            SqlParameter paraImportFrom = new SqlParameter
            {
                ParameterName = "ImportFrom",
                DbType = DbType.DateTime,
                Value = (object)FromDate ?? DBNull.Value
            };

            SqlParameter paraImportTo = new SqlParameter
            {
                ParameterName = "ImportTo",
                DbType = DbType.DateTime,
                Value = (object)ToDate ?? DBNull.Value
            };

            SqlParameter paraNoOfDeals = new SqlParameter
            {
                ParameterName = "NoofDeals",
                DbType = DbType.Int32,
                Value = (object)NoofDeals ?? DBNull.Value
            };

            SqlParameter paraDealShortReference = new SqlParameter
            {
                ParameterName = "DealShortReference",
                DbType = DbType.String,
                Value = (object)DealShortReference ?? DBNull.Value
            };

            SqlParameter paraImportStatus = new SqlParameter
            {
                ParameterName = "ImportStatus",
                DbType = DbType.Boolean,
                Value = (object)ImportStatus ?? DBNull.Value
            };

            SqlParameter paraImportBy = new SqlParameter
            {
                ParameterName = "ImportBy",
                DbType = DbType.String,
                Value = (object)ImportBy ?? DBNull.Value
            };

            SqlParameter paraDealSource = new SqlParameter
            {
                ParameterName = "DealSource",
                DbType = DbType.String,
                Value = (object)DealSource ?? DBNull.Value
            };
            bool result = true;

            result = _usersRepository.ExecuteStoredProcedure<bool>("INSERT_IMPORTLOGS", paraImportDate, paraImportFrom, paraImportTo, paraImportStatus, paraNoOfDeals, paraDealShortReference, paraImportBy, paraDealSource).FirstOrDefault();
            return result;
        }



        /// <summary>
        /// By Mayank
        /// 29 Nov 2018
        /// Import Client
        /// </summary>
        /// <returns></returns>
        public long ImportClient(ContactModel contactModel, AccountModel accountModel, string ClientSource)
        {
            SqlParameter paraFullName = new SqlParameter { ParameterName = "FullName", DbType = DbType.String, Value = (object)(contactModel.first_name + ' ' + contactModel.last_name) ?? DBNull.Value };
            SqlParameter paraCompanyName = new SqlParameter { ParameterName = "CompanyName", DbType = DbType.String, Value = (object)(contactModel.account_name) ?? DBNull.Value };
            SqlParameter paraAddressLine1 = new SqlParameter { ParameterName = "AddressLine1", DbType = DbType.String, Value = (object)(accountModel.street) ?? DBNull.Value };
            SqlParameter paraAddressLine2 = new SqlParameter { ParameterName = "AddressLine2", DbType = DbType.String, Value = (object)(accountModel.postal_code) ?? DBNull.Value };
            SqlParameter paraCityTown = new SqlParameter { ParameterName = "City_Town", DbType = DbType.String, Value = (object)(accountModel.city) ?? DBNull.Value };
            SqlParameter paraCountry = new SqlParameter { ParameterName = "Country", DbType = DbType.String, Value = (object)(accountModel.country) ?? DBNull.Value };
            SqlParameter paraMobileNo = new SqlParameter { ParameterName = "MobileNo", DbType = DbType.String, Value = (object)(contactModel.mobile_phone_number) ?? DBNull.Value };
            SqlParameter paraAltMobileNo = new SqlParameter { ParameterName = "AltMobileNo", DbType = DbType.String, Value = (object)(contactModel.phone_number) ?? DBNull.Value };
            SqlParameter paraEmailAddress = new SqlParameter { ParameterName = "EmailAddress", DbType = DbType.String, Value = (object)(contactModel.email_address) ?? DBNull.Value };
            SqlParameter paraAccountNo = new SqlParameter { ParameterName = "AccountNo", DbType = DbType.String, Value = (object)(accountModel.short_reference) ?? DBNull.Value };
            SqlParameter paraCCAccountId = new SqlParameter { ParameterName = "CCAccount_id", DbType = DbType.String, Value = (object)(contactModel.account_id) ?? DBNull.Value };
            SqlParameter paraRegiterDate = new SqlParameter { ParameterName = "RegiterDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(contactModel.created_at)) ?? DBNull.Value };
            int IsActive = 0;
            if (contactModel.status == "enabled")
                IsActive = 1;
            SqlParameter paraIsActive = new SqlParameter { ParameterName = "IsActive", DbType = DbType.Int32, Value = (object)(IsActive) ?? DBNull.Value };
            SqlParameter paraCreatedDate = new SqlParameter { ParameterName = "CreatedDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(contactModel.created_at)) ?? DBNull.Value };
            SqlParameter paraUpdatedDate = new SqlParameter { ParameterName = "UpdatedDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(contactModel.updated_at)) ?? DBNull.Value };
            SqlParameter paraUserId = new SqlParameter { ParameterName = "UserId", DbType = DbType.Int64, Value = (object)ProjectSession.LoginUserDetails.UserId ?? DBNull.Value };
            SqlParameter paraClientSource = new SqlParameter { ParameterName = "ClientSource", DbType = DbType.String, Value = (object)(ClientSource) ?? DBNull.Value };
            long Importresult = _usersRepository.ExecuteStoredProcedure<long>("usp_ClientMaster_ImportClient", paraFullName, paraCompanyName, paraAddressLine1, paraAddressLine2, paraCityTown, paraCountry, paraMobileNo, paraAltMobileNo, paraEmailAddress, paraAccountNo, paraCCAccountId, paraRegiterDate, paraIsActive, paraCreatedDate, paraUpdatedDate, paraUserId, paraClientSource).FirstOrDefault();
            return Importresult;
        }


        /// <summary>
        /// By Mayank
        /// 29 Nov 2018
        /// Import Deal
        /// </summary>
        /// <returns></returns>
        public int ImportDealData(ConversionModel conversionModel, ContactModel contactModel, string TStatus, string DealSource)
        {
            SqlParameter paraDealNo = new SqlParameter { ParameterName = "DealNo", DbType = DbType.String, Value = (object)(conversionModel.short_reference) ?? DBNull.Value };
            //var paraContactName = new SqlParameter { ParameterName = "ContactName", DbType = DbType.String, Value = (object)(contact.first_name + ' ' + contact.last_name) ?? DBNull.Value };
            SqlParameter paraTradeDate = new SqlParameter { ParameterName = "TradeDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(conversionModel.conversion_date)) ?? DBNull.Value };
            SqlParameter paraValueDate = new SqlParameter { ParameterName = "ValueDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(conversionModel.settlement_date)) ?? DBNull.Value };
            SqlParameter paraClientSoldAmt = new SqlParameter { ParameterName = "ClientSoldAmt", DbType = DbType.Decimal, Value = (object)(conversionModel.client_sell_amount) ?? DBNull.Value };
            SqlParameter paraClientSoldCCY = new SqlParameter { ParameterName = "ClientSoldCCY", DbType = DbType.String, Value = (object)(conversionModel.sell_currency) ?? DBNull.Value };
            SqlParameter paraClientSoldGBP = new SqlParameter { ParameterName = "ClientSoldGBP", DbType = DbType.Decimal, Value = (object)(conversionModel.client_sell_amount_In_GBP) ?? DBNull.Value };
            SqlParameter paraClientBoughtAmt = new SqlParameter { ParameterName = "ClientBoughtAmt", DbType = DbType.Decimal, Value = (object)(conversionModel.client_buy_amount) ?? DBNull.Value };
            SqlParameter paraClientBoughtCCY = new SqlParameter { ParameterName = "ClientBoughtCCY", DbType = DbType.String, Value = (object)(conversionModel.buy_currency) ?? DBNull.Value };
            SqlParameter paraTradeType = new SqlParameter { ParameterName = "TradeType", DbType = DbType.String, Value = (object)("Spot") ?? DBNull.Value };
            SqlParameter paraTStatus = new SqlParameter { ParameterName = "TStatus", DbType = DbType.String, Value = (object)TStatus ?? DBNull.Value };
            //var paraCompanyName = new SqlParameter { ParameterName = "CompanyName", DbType = DbType.String, Value = (object)(contact.account_name) ?? DBNull.Value };
            SqlParameter paraCCAccountId = new SqlParameter { ParameterName = "CCAccount_id", DbType = DbType.String, Value = (object)(contactModel.account_id) ?? DBNull.Value };
            SqlParameter paraGrossCommGBP = new SqlParameter { ParameterName = "GrossCommGBP", DbType = DbType.Decimal, Value = (object)(conversionModel.GrossCommGBP) ?? DBNull.Value };
            SqlParameter paraCreatedDate = new SqlParameter { ParameterName = "CreatedDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(conversionModel.created_at)) ?? DBNull.Value };
            SqlParameter paraUpdatedDate = new SqlParameter { ParameterName = "UpdatedDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(conversionModel.updated_at)) ?? DBNull.Value };
            SqlParameter paraUserId = new SqlParameter { ParameterName = "UserId", DbType = DbType.Int64, Value = (object)ProjectSession.LoginUserDetails.UserId ?? DBNull.Value };
            SqlParameter paraDealSource = new SqlParameter { ParameterName = "DealSource", DbType = DbType.String, Value = (object)DealSource ?? DBNull.Value };
            SqlParameter paraMarketConversionRate = new SqlParameter { ParameterName = "MarketConversionRate", DbType = DbType.Decimal, Value = (object)conversionModel.mid_market_rate ?? DBNull.Value };
            SqlParameter paraGBPConversationRate = new SqlParameter { ParameterName = "GBPConversationRate", DbType = DbType.Decimal, Value = (object)conversionModel.GBPConversationRate ?? DBNull.Value };
            int Importresult = _usersRepository.ExecuteStoredProcedure<int>("usp_Deal_ImportDeal", paraDealNo, paraTradeDate, paraValueDate, paraClientSoldAmt, paraClientSoldCCY, paraClientSoldGBP, paraClientBoughtAmt, paraClientBoughtCCY, paraTradeType, paraTStatus, paraCCAccountId, paraGrossCommGBP, paraCreatedDate, paraUpdatedDate, paraUserId, paraDealSource, paraMarketConversionRate, paraGBPConversationRate).FirstOrDefault();

            return Importresult;
        }


        /// <summary>
        /// By Mayank
        /// 29 Nov 2018
        /// Import Deal
        /// </summary>
        /// <returns></returns>
        public int ImportDeal(ConversionModel conversionModel, string TStatus, string DealSource)
        {
            SqlParameter paraDealNo = new SqlParameter { ParameterName = "DealNo", DbType = DbType.String, Value = (object)(conversionModel.short_reference) ?? DBNull.Value };
            //var paraContactName = new SqlParameter { ParameterName = "ContactName", DbType = DbType.String, Value = (object)(contact.first_name + ' ' + contact.last_name) ?? DBNull.Value };
            SqlParameter paraTradeDate = new SqlParameter { ParameterName = "TradeDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(conversionModel.created_at)) ?? DBNull.Value };
            SqlParameter paraValueDate = new SqlParameter { ParameterName = "ValueDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(conversionModel.settlement_date)) ?? DBNull.Value };
            SqlParameter paraClientSoldAmt = new SqlParameter { ParameterName = "ClientSoldAmt", DbType = DbType.Decimal, Value = (object)(conversionModel.client_sell_amount) ?? DBNull.Value };
            SqlParameter paraClientSoldCCY = new SqlParameter { ParameterName = "ClientSoldCCY", DbType = DbType.String, Value = (object)(conversionModel.sell_currency) ?? DBNull.Value };
            SqlParameter paraClientSoldGBP = new SqlParameter { ParameterName = "ClientSoldGBP", DbType = DbType.Decimal, Value = (object)(conversionModel.client_sell_amount_In_GBP) ?? DBNull.Value };
            SqlParameter paraClientBoughtAmt = new SqlParameter { ParameterName = "ClientBoughtAmt", DbType = DbType.Decimal, Value = (object)(conversionModel.client_buy_amount) ?? DBNull.Value };
            SqlParameter paraClientBoughtCCY = new SqlParameter { ParameterName = "ClientBoughtCCY", DbType = DbType.String, Value = (object)(conversionModel.buy_currency) ?? DBNull.Value };
            SqlParameter paraTradeType = new SqlParameter { ParameterName = "TradeType", DbType = DbType.String, Value = (object)("Spot") ?? DBNull.Value };
            SqlParameter paraTStatus = new SqlParameter { ParameterName = "TStatus", DbType = DbType.String, Value = (object)TStatus ?? DBNull.Value };
            //var paraCompanyName = new SqlParameter { ParameterName = "CompanyName", DbType = DbType.String, Value = (object)(contact.account_name) ?? DBNull.Value };
            SqlParameter paraCCAccountId = new SqlParameter { ParameterName = "CCAccount_id", DbType = DbType.String, Value = (object)(conversionModel.account_id) ?? DBNull.Value };
            SqlParameter paraGrossCommCurrency = new SqlParameter { ParameterName = "GrossCommCurrency", DbType = DbType.String, Value = (object)conversionModel.GrossCommCurrency ?? DBNull.Value };
            SqlParameter paraGrossCommGBP = new SqlParameter { ParameterName = "GrossCommGBP", DbType = DbType.Decimal, Value = (object)(conversionModel.GrossCommGBP) ?? DBNull.Value };
            SqlParameter paraGrossComm = new SqlParameter { ParameterName = "GrossComm", DbType = DbType.Decimal, Value = (object)(conversionModel.GrossComm) ?? DBNull.Value };
            SqlParameter paraCreatedDate = new SqlParameter { ParameterName = "CreatedDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(conversionModel.created_at)) ?? DBNull.Value };
            SqlParameter paraUpdatedDate = new SqlParameter { ParameterName = "UpdatedDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(conversionModel.updated_at)) ?? DBNull.Value };
            SqlParameter paraUserId = new SqlParameter { ParameterName = "UserId", DbType = DbType.Int64, Value = (object)ProjectSession.LoginUserDetails.UserId ?? DBNull.Value };
            SqlParameter paraStatus = new SqlParameter { ParameterName = "Status", DbType = DbType.String, Value = (object)conversionModel.status ?? DBNull.Value };
            SqlParameter paraDealSource = new SqlParameter { ParameterName = "DealSource", DbType = DbType.String, Value = (object)DealSource ?? DBNull.Value };
            SqlParameter paraMarketConversionRate = new SqlParameter { ParameterName = "MarketConversionRate", DbType = DbType.Decimal, Value = (object)conversionModel.mid_market_rate ?? DBNull.Value };
            SqlParameter paraGBPConversationRate = new SqlParameter { ParameterName = "GBPConversationRate", DbType = DbType.Decimal, Value = (object)conversionModel.GBPConversationRate ?? DBNull.Value };

            SqlParameter paraProfitOrLoss = new SqlParameter { ParameterName = "ProfitOrLoss", DbType = DbType.Decimal, Value = (object)conversionModel.amount_profit_loss ?? DBNull.Value };
            SqlParameter paraEventDate = new SqlParameter { ParameterName = "EventDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(conversionModel.event_date)) ?? DBNull.Value };

            int Importresult = _usersRepository.ExecuteStoredProcedure<int>("usp_Deal_ImportDeal", paraDealNo, paraTradeDate, paraValueDate, paraClientSoldAmt, paraClientSoldCCY, paraClientSoldGBP, paraClientBoughtAmt, paraClientBoughtCCY, paraTradeType, paraTStatus, paraCCAccountId, paraGrossCommCurrency, paraGrossCommGBP, paraGrossComm, paraCreatedDate, paraUpdatedDate, paraUserId, paraStatus, paraDealSource, paraMarketConversionRate, paraGBPConversationRate, paraProfitOrLoss, paraEventDate).FirstOrDefault();

            return Importresult;
        }

        public long ImportCurrencyRate(string FromCurrency, string ToCurrency, decimal Rate)
        {
            SqlParameter paraFromCurrency = new SqlParameter
            {
                ParameterName = "FromCurrency",
                DbType = DbType.String,
                Value = FromCurrency
            };
            SqlParameter paraToCurrency = new SqlParameter
            {
                ParameterName = "ToCurrency",
                DbType = DbType.String,
                Value = ToCurrency
            };
            SqlParameter paraRate = new SqlParameter
            {
                ParameterName = "Rate",
                DbType = DbType.Decimal,
                Value = Rate
            };
            long Result = _usersRepository.ExecuteStoredProcedure<long>("usp_Currency_ImportCurrencyRate", paraFromCurrency, paraToCurrency, paraRate).FirstOrDefault();
            return Result;
        }

        public List<ClientMasterModel> GetCC_Clientlist()
        {
            List<ClientMasterModel> clientlist = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("usp_ClientMaster_GetCC_ClientList").ToList();
            return clientlist;
        }

        public List<ImportLogModel> GetDealImportData()
        {
            List<ImportLogModel> clientlist1 = _usersRepository.ExecuteStoredProcedureList<ImportLogModel>("usp_GetImportDealData").ToList();
            return clientlist1;

        }
        #endregion

        #region GcPartner Import API
        /// <summary>
        /// By Krupesh
        /// 03 April 2020
        /// Import GCPartner  Client
        /// </summary>
        /// <returns></returns>
        public long ImportGCPartnerClient(ClientMasterModel objClient)
        {
            SqlParameter paraFullName = new SqlParameter { ParameterName = "FullName", DbType = DbType.String, Value = (object)(objClient.FullName) ?? DBNull.Value };
            SqlParameter paraCompanyName = new SqlParameter { ParameterName = "CompanyName", DbType = DbType.String, Value = (object)(objClient.CompanyName) ?? DBNull.Value };
            SqlParameter paraAddressLine1 = new SqlParameter { ParameterName = "AddressLine1", DbType = DbType.String, Value = (object)(objClient.AddressLine1) ?? DBNull.Value };
            SqlParameter paraAddressLine2 = new SqlParameter { ParameterName = "AddressLine2", DbType = DbType.String, Value = (object)(objClient.AddressLine2) ?? DBNull.Value };
            SqlParameter paraCityTown = new SqlParameter { ParameterName = "City_Town", DbType = DbType.String, Value = (object)(objClient.City_Town) ?? DBNull.Value };
            SqlParameter paraCountry = new SqlParameter { ParameterName = "Country", DbType = DbType.String, Value = (object)(objClient.Country) ?? DBNull.Value };
            SqlParameter paraMobileNo = new SqlParameter { ParameterName = "MobileNo", DbType = DbType.String, Value = (object)(objClient.MobileNo) ?? DBNull.Value };
            SqlParameter paraAltMobileNo = new SqlParameter { ParameterName = "AltMobileNo", DbType = DbType.String, Value = (object)(objClient.AltMobileNo) ?? DBNull.Value };
            SqlParameter paraEmailAddress = new SqlParameter { ParameterName = "EmailAddress", DbType = DbType.String, Value = (object)(objClient.EmailAddress) ?? DBNull.Value };
            SqlParameter paraAccountNo = new SqlParameter { ParameterName = "AccountNo", DbType = DbType.String, Value = (object)(objClient.AccountNo) ?? DBNull.Value };
            SqlParameter paraCCAccountId = new SqlParameter { ParameterName = "CCAccount_id", DbType = DbType.String, Value = (object)(objClient.CCAccount_id) ?? DBNull.Value };
            SqlParameter paraRegiterDate = new SqlParameter { ParameterName = "RegiterDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(objClient.RegiterDate)) ?? DBNull.Value };
            SqlParameter paraIsActive = new SqlParameter { ParameterName = "IsActive", DbType = DbType.Int32, Value = (object)(objClient.IsActive) ?? DBNull.Value };
            SqlParameter paraSource = new SqlParameter { ParameterName = "Source", DbType = DbType.String, Value = (object)(objClient.Source) ?? DBNull.Value };
            SqlParameter paraGcPartnerClientId = new SqlParameter { ParameterName = "GcPartnerClientId", DbType = DbType.String, Value = (object)(objClient.GcPartnerClientId) ?? DBNull.Value };
            SqlParameter paraCreatedBy = new SqlParameter { ParameterName = "CreatedBy", DbType = DbType.Int64, Value = (object)(objClient.UserId) ?? DBNull.Value };
            long Importresult = _usersRepository.ExecuteStoredProcedure<long>("usp_ClientMaster_ImportGCPartnerClient", paraFullName, paraCompanyName, paraAddressLine1, paraAddressLine2, paraCityTown, paraCountry, paraMobileNo, paraAltMobileNo, paraEmailAddress, paraAccountNo, paraCCAccountId, paraRegiterDate, paraIsActive, paraSource, paraGcPartnerClientId, paraCreatedBy).FirstOrDefault();

            return Importresult;
        }
        /// <summary>
        /// By Krupesh
        /// 03 April 2020
        /// Import GCPartner Deal
        /// </summary>
        /// <returns></returns>
        public long ImportGCPartnerDealData(DealModel objDeal)
        {

            SqlParameter paraClientId = new SqlParameter { ParameterName = "GcPartnerClientId", DbType = DbType.String, Value = (object)(objDeal.ClientId) ?? DBNull.Value };
            SqlParameter paraDealNo = new SqlParameter { ParameterName = "DealNo", DbType = DbType.String, Value = (object)(objDeal.DealNo) ?? DBNull.Value };
            SqlParameter paraTradeDate = new SqlParameter { ParameterName = "TradeDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(objDeal.TradeDate)) ?? DBNull.Value };
            SqlParameter paraValueDate = new SqlParameter { ParameterName = "ValueDate", DbType = DbType.DateTime, Value = (object)(Convert.ToDateTime(objDeal.ValueDate)) ?? DBNull.Value };
            SqlParameter paraClientSoldAmt = new SqlParameter { ParameterName = "ClientSoldAmt", DbType = DbType.Decimal, Value = (object)(objDeal.ClientSoldAmt) ?? DBNull.Value };
            SqlParameter paraClientSoldCCY = new SqlParameter { ParameterName = "ClientSoldCCY", DbType = DbType.String, Value = (object)(objDeal.ClientSoldCCY) ?? DBNull.Value };
            SqlParameter paraClientBoughtAmt = new SqlParameter { ParameterName = "ClientBoughtAmt", DbType = DbType.Decimal, Value = (object)(objDeal.ClientBoughtAmt) ?? DBNull.Value };
            SqlParameter paraClientBoughtCCY = new SqlParameter { ParameterName = "ClientBoughtCCY", DbType = DbType.String, Value = (object)(objDeal.ClientBoughtCCY) ?? DBNull.Value };
            SqlParameter paraTradeType = new SqlParameter { ParameterName = "TradeType", DbType = DbType.String, Value = (object)(objDeal.TradeType) ?? DBNull.Value };
            SqlParameter paraTStatus = new SqlParameter { ParameterName = "TStatus", DbType = DbType.String, Value = (object)(objDeal.TStatus) ?? DBNull.Value };
            SqlParameter paraDealSource = new SqlParameter { ParameterName = "DealSource", DbType = DbType.String, Value = (object)(objDeal.DealSource) ?? DBNull.Value };
            SqlParameter paraCreatedBy = new SqlParameter { ParameterName = "CreatedBy", DbType = DbType.Int64, Value = (object)(objDeal.UserId) ?? DBNull.Value };

            long result = _usersRepository.ExecuteStoredProcedure<long>("usp_GCPartnerDeal_ImportDeal",
                paraClientId, paraDealNo, paraTradeDate, paraValueDate,
                paraClientSoldAmt, paraClientSoldCCY,
                paraClientBoughtAmt, paraClientBoughtCCY, paraTradeType, paraTStatus, paraDealSource, paraCreatedBy).FirstOrDefault();
            return result;

        }

        /// <summary>
        /// By Krupesh
        /// 03 April 2020
        /// Import GCPartner Original Deal Json
        /// </summary>
        /// <returns></returns>
        public long ImportGCPartnerDealJSON(DealJSONData objDealJson)
        {
            SqlParameter paraDealId = new SqlParameter { ParameterName = "DealId", DbType = DbType.Int64, Value = (object)(objDealJson.DealId) ?? DBNull.Value };
            SqlParameter paraJsonStr = new SqlParameter { ParameterName = "JsonStr", DbType = DbType.String, Value = (object)(objDealJson.DealJsonStr) ?? DBNull.Value };
            SqlParameter paraCreatedby = new SqlParameter { ParameterName = "CreatedBy", DbType = DbType.Int64, Value = (object)(objDealJson.UserId) ?? DBNull.Value };

            return _usersRepository.ExecuteStoredProcedure<long>("usp_ImportDealOriginalJson",
                paraDealId, paraJsonStr, paraCreatedby).FirstOrDefault();
        }

        public string DatabaseBackup()
        {
            return _usersRepository.ExecuteStoredProcedure<string>("usp_DatabseBackup").FirstOrDefault();
        }

        #endregion

        #region Re-conciliation
        public long SaveReconciliation(DataTable dttable, ReconciliationModel model)
        {
            try
            {
                SqlParameter paramSource = new SqlParameter
                {
                    ParameterName = "Source",
                    DbType = DbType.String,
                    Value = model.Source
                };
                SqlParameter paramMonth = new SqlParameter
                {
                    ParameterName = "Month",
                    DbType = DbType.String,
                    Value = model.ReportMonth
                };
                SqlParameter paramYear = new SqlParameter
                {
                    ParameterName = "Year",
                    DbType = DbType.String,
                    Value = model.ReportYear
                };
                SqlParameter paramUserId = new SqlParameter
                {
                    ParameterName = "UserId",
                    DbType = DbType.Int32,
                    Value = ProjectSession.LoginUserDetails.UserId
                };
                SqlParameter paramImportReconciliationTable = new SqlParameter
                {
                    ParameterName = "ImportReconciliationTable",
                    SqlDbType = SqlDbType.Structured,
                    Value = dttable,
                    TypeName = "dbo.ImportReconciliationTable"
                };

                long result = _usersRepository.ExecuteStoredProcedure<long>("usp_SaveReconciliationDeal", paramSource,

                     paramMonth,
                     paramYear,
                     paramUserId,
                     paramImportReconciliationTable).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return 0;
            }
        }

        public List<ReconciliationMissingOrMismatchModel> GetMissingReconciliationDeal()
        {
            try
            {
                List<ReconciliationMissingOrMismatchModel> clientdeallist = _usersRepository.ExecuteStoredProcedureList<ReconciliationMissingOrMismatchModel>("usp_MissingReconciliationDeal").ToList();
                return clientdeallist;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return new List<ReconciliationMissingOrMismatchModel>();
        }

        public int DeleteMissingRecords(long reconciliationId)
        {
            try
            {
                SqlParameter paraReconciliationId = new SqlParameter
                {
                    ParameterName = "ReconciliationId",
                    DbType = DbType.Int64,
                    Value = (object)reconciliationId ?? DBNull.Value
                };

                int result = _usersRepository.ExecuteStoredProcedure<int>("usp_Reconciliation_MissingRecords_Delete", paraReconciliationId).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return 0;
            }
        }

        public int DeleteAllMissingRecords()
        {
            try
            {
                int result = _usersRepository.ExecuteStoredProcedure<int>("usp_Reconciliation_MissingRecords_DeleteAll").FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return 0;
            }
        }
        public List<ReconciliationMissingOrMismatchModel> GetMismatchReconciliationDeal(int pageNo, int pageSize, string dealRefNo)
        {
            try
            {
                SqlParameter paraDealRefNo = new SqlParameter
                {
                    ParameterName = "DealRefNo",
                    DbType = DbType.String,
                    Value = dealRefNo
                };
                SqlParameter paraPageNo = new SqlParameter
                {
                    ParameterName = "PageNo",
                    DbType = DbType.Int16,
                    Value = pageNo
                };

                SqlParameter paraPageSize = new SqlParameter
                {
                    ParameterName = "PageSize",
                    DbType = DbType.Int16,
                    Value = pageSize
                };

                List<ReconciliationMissingOrMismatchModel> clientdeallist = _usersRepository.ExecuteStoredProcedureList<ReconciliationMissingOrMismatchModel>("usp_MismatchReconciliationDeal", paraDealRefNo, paraPageNo, paraPageSize).ToList();
                return clientdeallist;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return new List<ReconciliationMissingOrMismatchModel>();
        }

        public int MergerReconciliationData(string DealRef, string MismatchColumn)
        {
            try
            {
                SqlParameter MismatchColumnName = new SqlParameter
                {
                    ParameterName = "MismatchColumnName",
                    DbType = DbType.String,
                    Value = (object)MismatchColumn ?? DBNull.Value
                };

                SqlParameter DealNo = new SqlParameter
                {
                    ParameterName = "DealRef",
                    DbType = DbType.String,
                    Value = (object)DealRef ?? DBNull.Value
                };

                int result = _usersRepository.ExecuteStoredProcedure<int>("usp_Reconciliation_MergeData", MismatchColumnName, DealNo).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return 0;
            }
        }
        #endregion
        #region Upload GC Partner Deals
        public string SaveUploadGCPartner(DataTable table, DealModel model)
        {

            try
            {
                SqlParameter paramSource = new SqlParameter
                {
                    ParameterName = "Source",
                    DbType = DbType.String,
                    Value = model.DealSource
                };

                SqlParameter paramImportGCTable = new SqlParameter
                {
                    ParameterName = "ImportGCTable",
                    SqlDbType = SqlDbType.Structured,
                    Value = table,
                    TypeName = "dbo.ImportGCPartnerTable"
                };

                string result = _usersRepository.ExecuteStoredProcedure<string>("usp_SaveUploadDeal_GCPartner", paramSource,
                     paramImportGCTable).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ex.Message;
            }
        }
        public string SaveUploadEburry(DataTable table, DealModel model)
        {

            try
            {
                SqlParameter paramSource = new SqlParameter
                {
                    ParameterName = "Source",
                    DbType = DbType.String,
                    Value = model.DealSource
                };

                SqlParameter paramImportEburryTable = new SqlParameter
                {
                    ParameterName = "ImportEburrytable",
                    SqlDbType = SqlDbType.Structured,
                    Value = table,
                    TypeName = "dbo.ImportEburryTable"
                };

                string result = _usersRepository.ExecuteStoredProcedure<string>("usp_SaveUploadDeal_Eburry", paramSource,
                     paramImportEburryTable).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ex.Message;
            }
        }
        public string SaveUploadScioPay(DataTable table, DealModel model)
        {
            try
            {
                SqlParameter paramSource = new SqlParameter
                {
                    ParameterName = "Source",
                    DbType = DbType.String,
                    Value = model.DealSource
                };

                SqlParameter paramImportEburryTable = new SqlParameter
                {
                    ParameterName = "ImportScioPaytable",
                    SqlDbType = SqlDbType.Structured,
                    Value = table,
                    TypeName = "dbo.ImportScioPaytable"
                };

                string result = _usersRepository.ExecuteStoredProcedure<string>("usp_SaveUploadDeal_ScioPay", paramSource,
                     paramImportEburryTable).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                string fileName = "ImportScioPayDeal.txt";
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "ImportScioPayDeal");
                string path = Path.Combine(folderPath, Path.GetFileName(fileName));
                if (!Directory.Exists(folderPath))
                {
                   Directory.CreateDirectory(folderPath);
                }
                if (!File.Exists(path))
                {
                    FileStream myFile = File.Create(path);
                    myFile.Close();
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("***********************************" + DateTime.Now + "***********************************");

                    sw.WriteLine("Save Upload ScioPay");
                    sw.WriteLine(ex.Message);


                    sw.WriteLine("**********************************************************************************************");
                }
                _logger.Error(ex);
                return ex.Message;
            }
        }
        #endregion
        public long GetClientIdbyclientname(string clientname)
        {
            SqlParameter paraClientName = new SqlParameter
            {
                ParameterName = "ClientName",
                DbType = DbType.String,
                Value = clientname
            };

            long result = _usersRepository.ExecuteStoredProcedure<long>("GetClientIdbyclientname", paraClientName).FirstOrDefault();
            return result;
        }

        public ClientMasterModel GetClientIdbyAccNo(string AccNo)
        {
            SqlParameter paraAccNo = new SqlParameter
            {
                ParameterName = "AccountNo",
                DbType = DbType.String,
                Value = AccNo
            };

            ClientMasterModel result = _usersRepository.ExecuteStoredProcedure<ClientMasterModel>("GetClientIdbyAccNo", paraAccNo).FirstOrDefault();
            return result;
        }
        public List<ReconciliationMissingOrMismatchModel> GetUnmatchReconciliationDeal(string Source, int pageNo, int pageSize, string sortColumn, string sortDir)
        {
            SqlParameter paraSource = new SqlParameter
            {
                ParameterName = "Source",
                DbType = DbType.String,
                Value = Source
            };
            SqlParameter paraPageNo = new SqlParameter
            {
                ParameterName = "PageNo",
                DbType = DbType.Int16,
                Value = pageNo
            };

            SqlParameter paraPageSize = new SqlParameter
            {
                ParameterName = "PageSize",
                DbType = DbType.Int16,
                Value = pageSize
            };

            SqlParameter paraSortColumn = new SqlParameter
            {
                ParameterName = "SortColumn",
                DbType = DbType.String,
                Value = sortColumn
            };

            SqlParameter paraSortOrder = new SqlParameter
            {
                ParameterName = "SortOrder",
                DbType = DbType.String,
                Value = sortDir
            };
            SqlParameter paraEmpId = new SqlParameter
            {
                ParameterName = "EmpId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };
            List<ReconciliationMissingOrMismatchModel> unmatchDeallist = _usersRepository.ExecuteStoredProcedureList<ReconciliationMissingOrMismatchModel>("usp_UnmatchReconciliationDeal", paraSource, paraPageNo, paraPageSize, paraSortColumn, paraSortOrder, paraEmpId).ToList();
            return unmatchDeallist;
        }

        public List<DealModel> GetTempDeallist(int pageNo, int pageSize, string sortColumn, string sortDir, string DealNo, string CompanyName,string AccountId,string Source = null,string IsMatch = null)
        {
            SqlParameter paraPageNo = new SqlParameter
            {
                ParameterName = "PageNo",
                DbType = DbType.Int16,
                Value = pageNo
            };

            SqlParameter paraPageSize = new SqlParameter
            {
                ParameterName = "PageSize",
                DbType = DbType.Int16,
                Value = pageSize
            };

            SqlParameter paraSortColumn = new SqlParameter
            {
                ParameterName = "SortColumn",
                DbType = DbType.String,
                Value = sortColumn
            };

            SqlParameter paraSortOrder = new SqlParameter
            {
                ParameterName = "SortOrder",
                DbType = DbType.String,
                Value = sortDir
            };
            SqlParameter paraEmpId = new SqlParameter
            {
                ParameterName = "EmpId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };
            SqlParameter paraDealNo = new SqlParameter
            {
                ParameterName = "DealNo",
                DbType = DbType.String,
                Value = DealNo
            };
            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = CompanyName
            };
            SqlParameter paraAccountId = new SqlParameter { 
                ParameterName = "AccountNo",
                DbType = DbType.String,
                Value = AccountId
            };
            SqlParameter paraSource = new SqlParameter
            {
                ParameterName = "DealSource",
                DbType = DbType.String,
                Value = Source
            };
            SqlParameter paraIsMatch = new SqlParameter
            {
                ParameterName = "IsMatch",
                DbType = DbType.String,
                Value = IsMatch
            };

            List<DealModel> Deallist = _usersRepository.ExecuteStoredProcedureList<DealModel>("usp_ImportDealTemp_List", paraPageNo, paraPageSize, paraSortColumn, paraSortOrder, paraEmpId, paraDealNo,paraCompanyName,paraAccountId,paraSource,paraIsMatch).ToList();
            return Deallist;
        }


        public List<DealModel> GetAllTempDealList()
        {
            List<DealModel> Deallist = _usersRepository.ExecuteStoredProcedureList<DealModel>("usp_AllImportDealTemp_List").ToList();
            return Deallist;
        }

        public ClientMasterModel GetClientIdbyCompanyName(string companyname, string source, string AccountId)
        {
            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = companyname
            };
            SqlParameter paraSource = new SqlParameter
            {
                ParameterName = "Source",
                DbType = DbType.String,
                Value = source
            };
            SqlParameter paraAccountId = new SqlParameter
            {
                ParameterName = "AccountId",
                DbType = DbType.String,
                Value = AccountId
            };

            ClientMasterModel result = _usersRepository.ExecuteStoredProcedure<ClientMasterModel>("GetClientIdbycompanyname", paraCompanyName, paraSource, paraAccountId).FirstOrDefault();
            return result;
        }


        public DealModel GetTempDealById(long ImportDealId)
        {
            SqlParameter paraDealId = new SqlParameter
            {
                ParameterName = "DealId",
                DbType = DbType.Int64,
                Value = ImportDealId
            };
            DealModel result = _usersRepository.ExecuteStoredProcedure<DealModel>("GetImportTempDealById", paraDealId).FirstOrDefault();
            return result;
        }
        public int ImportTempDeal(DealModel model)
        {
            SqlParameter paraDealId = new SqlParameter
            {
                ParameterName = "DealId",
                DbType = DbType.Int64,
                Value = (object)model.DealId ?? DBNull.Value
            };

            SqlParameter paraDealNo = new SqlParameter
            {
                ParameterName = "DealNo",
                DbType = DbType.String,
                Value = (object)model.DealNo ?? DBNull.Value
            };

            SqlParameter paraContactName = new SqlParameter
            {
                ParameterName = "ContactName",
                DbType = DbType.String,
                Value = (object)model.ContactName ?? DBNull.Value
            };

            SqlParameter paraTradeDate = new SqlParameter
            {
                ParameterName = "TradeDate",
                DbType = DbType.DateTime,
                Value = (object)model.TradeDate ?? DBNull.Value
            };

            SqlParameter paraValueDate = new SqlParameter
            {
                ParameterName = "ValueDate",
                DbType = DbType.DateTime,
                Value = (object)model.ValueDate ?? DBNull.Value
            };

            SqlParameter paraClientSoldAmt = new SqlParameter
            {
                ParameterName = "ClientSoldAmt",
                DbType = DbType.Decimal,
                Value = (object)model.ClientSoldAmt ?? DBNull.Value
            };

            SqlParameter paraClientSoldCCY = new SqlParameter
            {
                ParameterName = "ClientSoldCCY",
                DbType = DbType.String,
                Value = (object)model.ClientSoldCCY ?? DBNull.Value
            };

            SqlParameter paraClientSoldGBP = new SqlParameter
            {
                ParameterName = "ClientSoldGBP",
                DbType = DbType.Decimal,
                Value = (object)model.ClientSoldGBP ?? DBNull.Value
            };

            SqlParameter paraClientBoughtAmt = new SqlParameter
            {
                ParameterName = "ClientBoughtAmt",
                DbType = DbType.Decimal,
                Value = (object)model.ClientBoughtAmt ?? DBNull.Value
            };

            SqlParameter paraClientBoughtCCY = new SqlParameter
            {
                ParameterName = "ClientBoughtCCY",
                DbType = DbType.String,
                Value = (object)model.ClientBoughtCCY ?? DBNull.Value
            };

            SqlParameter paraGrossCommGBP = new SqlParameter
            {
                ParameterName = "GrossCommGBP",
                DbType = DbType.Decimal,
                Value = (object)model.GrossCommGBP ?? DBNull.Value
            };
            SqlParameter paraGrossCommGBPFinal = new SqlParameter
            {
                ParameterName = "GrossCommGBPFinal",
                DbType = DbType.Decimal,
                Value = (object)model.GrossCommGBPFinal ?? DBNull.Value
            };

            SqlParameter paraWLTotalCommGBP = new SqlParameter
            {
                ParameterName = "WLTotalCommGBP",
                DbType = DbType.Decimal,
                Value = (object)model.WLTotalCommGBP ?? DBNull.Value
            };

            SqlParameter paraTradeType = new SqlParameter
            {
                ParameterName = "TradeType",
                DbType = DbType.String,
                Value = (object)model.TradeType ?? DBNull.Value
            };

            SqlParameter paraTStatus = new SqlParameter
            {
                ParameterName = "TStatus",
                DbType = DbType.String,
                Value = (object)model.TStatus ?? DBNull.Value
            };

            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };

            SqlParameter paraPLAmount = new SqlParameter
            {
                ParameterName = "AdditionalPLAmount",
                DbType = DbType.Decimal,
                Value = (object)model.AdditionalPLAmount ?? DBNull.Value
            };
            SqlParameter paraPLnotes = new SqlParameter
            {
                ParameterName = "AdditionalPLNotes",
                DbType = DbType.String,
                Value = (object)model.AdditionalPLNotes ?? DBNull.Value
            };
            SqlParameter paraGrossCommCurrency = new SqlParameter
            {
                ParameterName = "GrossCommCurrency",
                DbType = DbType.String,
                Value = (object)model.GrossCommCurrency ?? DBNull.Value
            };
            SqlParameter paraGrossComm = new SqlParameter
            {
                ParameterName = "GrossComm",
                DbType = DbType.Decimal,
                Value = (object)model.GrossComm ?? DBNull.Value
            };
            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = (object)model.CompanyName ?? DBNull.Value
            };
            SqlParameter paraDealSource = new SqlParameter
            {
                ParameterName = "DealSource",
                DbType = DbType.String,
                Value = (object)model.DealSource ?? DBNull.Value
            };
            SqlParameter paraMarketConversionRate = new SqlParameter
            {
                ParameterName = "MarketConversionRate",
                DbType = DbType.Decimal,
                Value = (object)model.MarketConversionRate ?? DBNull.Value
            };
            SqlParameter paraGBPConversationRate = new SqlParameter
            {
                ParameterName = "GBPConversationRate",
                DbType = DbType.Decimal,
                Value = (object)model.GBPConversationRate ?? DBNull.Value
            };
            SqlParameter paraProfitOrLoss = new SqlParameter
            {
                ParameterName = "ProfitOrLoss",
                DbType = DbType.Decimal,
                Value = (object)model.ProfitOrLoss ?? DBNull.Value
            };
            SqlParameter paraEventDate = new SqlParameter
            {
                ParameterName = "EventDate",
                DbType = DbType.DateTime,
                Value = (object)model.EventDate ?? DBNull.Value
            };
            SqlParameter paraScioPayAccountId = new SqlParameter
            {
                ParameterName = "ScioPayAccountId",
                DbType = DbType.String,
                Value = (object)model.ScioPayAccountId ?? DBNull.Value
            };
            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = (object)model.ClientId ?? DBNull.Value
            };
            SqlParameter paraProfitGBPEst = new SqlParameter
            {
                ParameterName = "ProfitGBPEst",
                DbType = DbType.Decimal,
                Value = (object)model.ProfitGBPEst ?? DBNull.Value
            };
            SqlParameter paraProfitCCY = new SqlParameter
            {
                ParameterName = "ProfitCCY",
                DbType = DbType.String,
                Value = (object)model.ProfitCCY ?? DBNull.Value
            };
            SqlParameter paraSpread = new SqlParameter
            {
                ParameterName = "Spread",
                DbType = DbType.String,
                Value = (object)model.Spread ?? DBNull.Value
            };
            SqlParameter paraVolumeGBPEst = new SqlParameter
            {
                ParameterName = "VolumeGBPEst",
                DbType = DbType.Decimal,
                Value = (object)model.VolumeGBPEst ?? DBNull.Value
            };
            SqlParameter paraWLCommsCCY = new SqlParameter
            {
                ParameterName = "WLCommsCCY",
                DbType = DbType.String,
                Value = (object)model.WLCommsCCY ?? DBNull.Value
            };
            SqlParameter paraWLCommsInCCY = new SqlParameter
            {
                ParameterName = "WLCommsInCCY",
                DbType = DbType.Decimal,
                Value = (object)model.WLCommsInCCY ?? DBNull.Value
            };
            SqlParameter paraWLRevShare = new SqlParameter
            {
                ParameterName = "WLRevShare",
                DbType = DbType.String,
                Value = (object)model.WLRevShare ?? DBNull.Value
            };
            SqlParameter paraBrand = new SqlParameter
            {
                ParameterName = "Brand",
                DbType = DbType.String,
                Value = (object)model.Brand ?? DBNull.Value
            };
            SqlParameter paraAccountCountry = new SqlParameter
            {
                ParameterName = "AccountCountry",
                DbType = DbType.String,
                Value = (object)model.AccountCountry ?? DBNull.Value
            };
            SqlParameter paraOwner = new SqlParameter
            {
                ParameterName = "Owner",
                DbType = DbType.String,
                Value = (object)model.Owner ?? DBNull.Value
            };
            SqlParameter paraContact = new SqlParameter
            {
                ParameterName = "Contact",
                DbType = DbType.String,
                Value = (object)model.Contact ?? DBNull.Value
            };
            SqlParameter paraCreator = new SqlParameter
            {
                ParameterName = "Creator",
                DbType = DbType.String,
                Value = (object)model.Creator ?? DBNull.Value
            };
            SqlParameter paraAccountId = new SqlParameter
            {
                ParameterName = "AccountId",
                DbType = DbType.String,
                Value = (object)model.AccountId ?? DBNull.Value
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_Import_Deal", paraDealId, paraDealNo, paraContactName, paraTradeDate, paraValueDate, paraClientSoldAmt, paraClientSoldCCY, paraClientSoldGBP, paraClientBoughtAmt, paraClientBoughtCCY, paraGrossCommGBP, paraGrossCommGBPFinal, paraWLTotalCommGBP, paraTradeType, paraTStatus, paraUserId, paraPLAmount, paraPLnotes, paraGrossCommCurrency, paraGrossComm, paraCompanyName, paraDealSource, paraMarketConversionRate, paraGBPConversationRate, paraProfitOrLoss, paraEventDate, paraScioPayAccountId, paraClientId, paraProfitGBPEst, paraProfitCCY, paraSpread, paraVolumeGBPEst,paraWLCommsCCY,paraWLCommsInCCY,paraWLRevShare,paraBrand,paraAccountCountry,paraOwner, paraAccountId).FirstOrDefault();
            return result;
        }
        public int ImportAllDeal(bool allCompletlyMatchDeal, bool allPartiallyMatchDeal)
        {
            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };
            SqlParameter paraAllCompletlyMatchDeal = new SqlParameter
            {
                ParameterName = "IsCompleteMatch",
                DbType = DbType.Boolean,
                Value = allCompletlyMatchDeal
            };
            SqlParameter paraAllPartiallyMatchDeal = new SqlParameter
            {
                ParameterName = "IsPartialMatch",
                DbType = DbType.Boolean,
                Value = allPartiallyMatchDeal
            };
            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_ImportAll_Deal", paraUserId, paraAllCompletlyMatchDeal, paraAllPartiallyMatchDeal).FirstOrDefault();
            return result;
        }

        public int ImportSelectedDeal(string deal)
        {
            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };
            SqlParameter paraDeal = new SqlParameter
            {
                ParameterName = "Deal",
                DbType = DbType.String,
                Value = deal
            };
            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_ImportSelected_Deal", paraUserId, paraDeal).FirstOrDefault();
            return result;
        }
    }
}
