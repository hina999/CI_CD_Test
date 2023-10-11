using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.Report
{
    public interface IReportService
    {
        List<ClientReproductionReport> GetClientReproductionList(int Month, int Year, string ClientName, string CompanyName, long traderId, long SalesPersonId, long? JuniorSalesPersonId);

        List<ClientRevenueReport> GetClientRevenueList(int Month, int Year, string ClientName, string CompanyName, long traderId, long SalesPersonId, long? JuniorSalesPersonId);

        List<CurrencyCloudReportModel> GetCurrencyCloudClientList(string ClientName, string CompanyName, string AccountNo, DateTime? NoTradeFromDate = null, DateTime? NoTradeToDate = null, long traderId = 0, long SalesPersonId = 0, long? JuniorSalesPersonId = 0);

        List<ClientMarginReportModel> GetClientMarginList(string ClientName, string CompanyName, string AccountNo, long traderId = 0, long SalesPersonId = 0, long? JuniorSalesPersonId = 0);

        List<ProfitLossReportModel> GetProfitLossReportList(int pageNo, int pageSize, string sortColumn, string sortDir, string ClientName, string CompanyName, string AccountNo, string DealNo, DateTime? FromDate = null, DateTime? ToDate = null, long traderId = 0, long SalesPersonId = 0, long? JuniorSalesPersonId = 0);

        List<NewClientReportModel> GetNewClientsReportList(int? Month, int? Year, long TraderId , long SalesPersonId, long? JuniorSalesPersonId);

        List<VolumeReportByMonthodelMonthYear> GetVolumeReportByMonthList(int? StartMonth, int? StartYear, int? EndMonth, int? EndYear);

        List<VolumeReportByMonthodelMonthYear> GetVolumeReportByTraderList(int? Month, int? Year);
        List<VolumeReportByMonthodelMonthYear> GetVolumeReportByCategoryList(int? Month, int? Year);

        List<ClientMasterModel> GetClientListByClientId(List<long> client_arr);
        NewClientReportModel GetNewIndividualClientsReportList(int? Month, int? Year, long clientId);
        UserModel GetSalesPersonDetailsById(long SalesPersonID);
        TraderModel GetTraderDetailsByTraderId(long TraderId);
        List<ClientDDL> GetClientListForDropdown();
        List<ClientRevenueReport> GetNewClientRevenueList(int Month, int Year, string ClientName, string CompanyName, long traderId, long SalesPersonId, long? JuniorSalesPersonId);
        List<ClientRevenueReport> GetNewClientRevenueList_settledfigures(int Month, int Year, string ClientName, string CompanyName, long traderId, long SalesPersonId, long? JuniorSalesPersonId);

    }
}
