using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.Client
{
    public interface IClientService
    {
        /// <summary>
        /// Prototype for get client list
        /// </summary>
        /// <returns></returns>
        List<ClientMasterModel> GetClientlist(int pageNo, int pageSize, string sortColumn, string sortDir, string FullName, string CompanyName, string TraderName, string AccountNo, string EmailAddress, string CommunicationDetail, long? traderId, long? SalesPersonId, long? SearchJuniorSalesPersonId,string AwaitingAction, string MarketOrder, string BoughtCurrency, string SoldCurrency, long? LeadCategoryId, long? SectorCategoryId, long? BusinessCategoryId, string ClientSource);

        /// <summary>
        /// Prototype for get client details by clientid
        /// </summary>
        /// <returns></returns>
        ClientMasterModel GetClientDetailsByClientId(long clientId);
        CategoryModel GetBusinessCategoryByCategoryId(long? categoryId);
        CurrencyModel GetCurrencyDetailById(int CCYId);
        LeadCategoryModel GetLeadCategoryDetailById(long LeadId);

        /// <summary>
        /// Prototype for add client
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        long AddClient(ClientMasterModel model);

        /// <summary>
        /// Prototype for add awaiting action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int SaveAwaitingAction(ClientMasterModel model);

        /// <summary>
        /// Prototype for delete client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        int DeleteClient(long clientId);

        // <summary>
        /// Prototype for get client deal list
        /// </summary>
        /// <returns></returns>
        List<DealModel> GetClientDeallist(long clientId);

        // <summary>
        /// Prototype for get client task reminder list
        /// </summary>
        /// <returns></returns>
        List<TaskReminderModel> GetTaskReminderlist(long clientId);

        int SaveTaskReminder(TaskReminderModel model);

        bool DeleteSaveTaskReminder(long TaskId);

        bool ChangeMarketOrderStatus(long clientId, bool status);

        MobileStatus GetMobileStatus(string MobileNo);

        void UpdateMobileStatus(string MobileNo, bool tpsResult);

        DNDNumbers GetDNDNumberById(long id);

        List<DNDNumbers> GetDNDNumberlist(string fullName, string mobileNo);

        int DeleteDNDNumber(long dNDNumberID);

        int SaveDNDNumber(DNDNumbers model);

        List<NotificationSettingModel> GetMarketOrderNotificationFilter();
        List<ConditionalOperatorModel> GetConditionalOperatorList();
        long SaveMarketOrderSetting(MarketOrderSettingModel model);
        List<MarketOrderSettingModel> GetMarketOrderSettingList(string FullName, string EmailAddress, string CompanyName, string MarketOrderStatus, int? FromCurrency, int? ToCurrency, long ClientId = 0, long ProspectId = 0, long TraderId = 0, long SalesPersonId = 0);
        bool DeleteMarketOrderSetting(long MarketOrderId);
        long SaveMarketOrderNotification(long clientId, long marketOrderId);
        bool UpdateMarketOrderSettingStatus(int ID, string Status);
        RingCentralClientModel GetClientDetailByMobile(string MobileNo);
        int ValidateClientByRingCentralMobileAndSalesPerson(string MobileNo, long SalesPersonId);
        bool UpdatePastCommDetail(long clientId, string PastCommDetail);
        SectorModel GetBusinessCategorySectorbySectorID(long? sectorid);
        string SaveUploadScioPayClient(DataTable table, ClientMasterModel model);
        string SaveUploadGCPartnerClient(DataTable table, ClientMasterModel model);
        long GetSalespersonIdByName(string SalesPersonName);
        long GetTraderIdByName(string TraderName);
    }
}
