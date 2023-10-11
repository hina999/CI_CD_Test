using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.Deal
{
    public interface IDealService
    {
        /// <summary>
        /// Prototype for get deal list
        /// </summary>
        /// <returns></returns>
        List<DealModel> GetDeallist(int pageNo, int pageSize, string sortColumn, string sortDir, string DealNo, string CompanyName, DateTime? FromDate = null, DateTime? ToDate = null, long SalesPersonId = 0, long JuniorSalesPersonId = 0, long TraderId = 0, string TStatus = null,string Source=null);

        /// <summary>
        /// Prototype for get staging deal details by dealId
        /// </summary>
        /// <returns></returns>
        DealModel GetDealDetailsByDealId(long dealId);

        /// <summary>
        /// Prototype for add deal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int SaveDeal(DealModel model);

        //int ImportClientData(ContactModel contactModel, AccountModel accountModel);

        int ImportDealData(ConversionModel conversionModel, ContactModel contactModel, string TStatus, string DealSource);

        long ImportClient(ContactModel contactModel, AccountModel accountModel, string ClientSource);

        int ImportDeal(ConversionModel conversionModel, string TStatus, string DealSource);

        List<ClientMasterModel> GetCC_Clientlist();

        bool InsertImportLog(DateTime FromDate, DateTime ToDate, int NoofDeals, string DealShortReference, bool ImportStatus, string ImportBy, string DealSource);

        List<ImportLogModel> GetDealImportData();

        long ImportGCPartnerClient(ClientMasterModel objClient);
        long ImportGCPartnerDealData(DealModel objDeal);
        long ImportGCPartnerDealJSON(DealJSONData objDealJson);
        long SaveReconciliation(DataTable dttable, ReconciliationModel model);
        List<ReconciliationMissingOrMismatchModel> GetMissingReconciliationDeal();
        List<ReconciliationMissingOrMismatchModel> GetMismatchReconciliationDeal(int pageNo, int pageSize,string dealRefNo);
        string DatabaseBackup();
        int MergerReconciliationData(string DealRef, string MismatchColumn);

        long ImportCurrencyRate(string FromCurrency, string ToCurrency, decimal Rate);
        string SaveUploadGCPartner(DataTable table, DealModel model);
        long GetClientIdbyclientname(string clientname);
        string SaveUploadEburry(DataTable table, DealModel model);
        ClientMasterModel GetClientIdbyAccNo(string AccNo);
        int DeleteMissingRecords(long reconciliationId);
        int DeleteAllMissingRecords();
        List<ReconciliationMissingOrMismatchModel> GetUnmatchReconciliationDeal(string Source, int pageNo, int pageSize, string sortColumn, string sortDir);
        string SaveUploadScioPay(DataTable table, DealModel model);
        List<DealModel> GetTempDeallist(int pageNo, int pageSize, string sortColumn, string sortDir, string DealNo, string CompanyName,string AccountId,string Source = null,string IsMatch = null);
        ClientMasterModel GetClientIdbyCompanyName(string companyname,string source, string AccountId);
        DealModel GetTempDealById(long ImportDealId);
        int ImportTempDeal(DealModel model);
        int ImportAllDeal(bool allCompletlyMatchDeal, bool allPartiallyMatchDeal);
        int ImportSelectedDeal(string deal);
        List<DealModel> GetAllTempDealList();
    }
}
