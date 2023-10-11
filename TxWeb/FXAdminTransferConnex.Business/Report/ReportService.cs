using FXAdminTransferConnex.Data.Models;
using FXAdminTransferConnex.Data.Repository;
using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.Report
{
    public class ReportService : IReportService
    {
        #region Fields

        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<UserMaster> _usersRepository;

        #endregion

        #region ctor

        public ReportService(IRepository<UserMaster> providerRepository)
        {
            _usersRepository = providerRepository;
        }

        #endregion

        #region methods
        public List<ClientReproductionReport> GetClientReproductionList(int Month, int Year, string ClientName, string CompanyName, long traderId, long SalesPersonId, long? JuniorSalesPersonId)
        {
            SqlParameter paraMonth = new SqlParameter
            {
                ParameterName = "Month",
                DbType = DbType.Int32,
                Value = (object)Month ?? DBNull.Value
            };

            SqlParameter paraYear = new SqlParameter
            {
                ParameterName = "ToYear",
                DbType = DbType.Int32,
                Value = (object)Year ?? DBNull.Value
            };

            SqlParameter paraClientName = new SqlParameter
            {
                ParameterName = "ClientName",
                DbType = DbType.String,
                Value = (object)ClientName ?? DBNull.Value
            };

            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = (object)CompanyName ?? DBNull.Value
            };

            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)traderId ?? DBNull.Value
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

            List<ClientReproductionReport> result = _usersRepository.ExecuteStoredProcedureList<ClientReproductionReport>("MIS_GetClientReproductionReport", paraMonth, paraYear, paraClientName, paraCompanyName, paraTraderId, paraSalesPersonId, paraJuniorSalesPersonId).ToList();
            return result;
        }


        public List<ClientMasterModel> GetClientListByClientId(List<long> client_arr)
        {
            List<ClientMasterModel> userList = new List<ClientMasterModel>();
            foreach (long i in client_arr)
            {
                SqlParameter paraClientId = new SqlParameter
                {
                    ParameterName = "ClientId",
                    DbType = DbType.Int64,
                    Value = i
                };
                //userList = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("usp_ClientMaster_GetClientByClientId", paraClientId).ToList();
                ClientMasterModel user = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("usp_ClientMaster_GetClientByClientId", paraClientId).FirstOrDefault();
                userList.Add(user);
            }
            return userList;
        }

        public UserModel GetSalesPersonDetailsById(long SalesPersonID)
        {
            SqlParameter paraSalesPersonId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = SalesPersonID
            };

            UserModel traderlist = _usersRepository.ExecuteStoredProcedureList<UserModel>("usp_UserMaster_GetDetailBySalesPersonId", paraSalesPersonId).FirstOrDefault();
            return traderlist;
        }

        public TraderModel GetTraderDetailsByTraderId(long TraderId)
        {
            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = TraderId
            };

            TraderModel traderlist = _usersRepository.ExecuteStoredProcedureList<TraderModel>("usp_TraderMaster_GetDetailByTraderId", paraTraderId).FirstOrDefault();
            return traderlist;
        }

        public List<ClientRevenueReport> GetClientRevenueList(int Month, int Year, string ClientName, string CompanyName, long traderId, long SalesPersonId, long? JuniorSalesPersonId)
        {
            SqlParameter paraMonth = new SqlParameter
            {
                ParameterName = "Month",
                DbType = DbType.Int32,
                Value = (object)Month ?? DBNull.Value
            };

            SqlParameter paraYear = new SqlParameter
            {
                ParameterName = "ToYear",
                DbType = DbType.Int32,
                Value = (object)Year ?? DBNull.Value
            };

            SqlParameter paraClientName = new SqlParameter
            {
                ParameterName = "ClientName",
                DbType = DbType.String,
                Value = (object)ClientName ?? DBNull.Value
            };

            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = (object)CompanyName ?? DBNull.Value
            };

            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)traderId ?? DBNull.Value
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

            List<ClientRevenueReport> result = _usersRepository.ExecuteStoredProcedureList<ClientRevenueReport>("MIS_GetClientRenvenueReport", paraMonth, paraYear, paraClientName, paraCompanyName, paraTraderId, paraSalesPersonId, paraJuniorSalesPersonId).ToList();
            return result;
        }

        public List<CurrencyCloudReportModel> GetCurrencyCloudClientList(string ClientName, string CompanyName, string AccountNo, DateTime? NoTradeFromDate = null, DateTime? NoTradeToDate = null, long traderId = 0, long SalesPersonId = 0, long? JuniorSalesPersonId = 0)
        {
            SqlParameter paraClientName = new SqlParameter
            {
                ParameterName = "ClientName",
                DbType = DbType.String,
                Value = (object)ClientName ?? DBNull.Value
            };

            SqlParameter paraAccountNo = new SqlParameter
            {
                ParameterName = "AccountNo",
                DbType = DbType.String,
                Value = (object)AccountNo ?? DBNull.Value
            };

            SqlParameter paraNoTradeFromDate = new SqlParameter
            {
                ParameterName = "NoTradeFromDate",
                DbType = DbType.DateTime,
                Value = (object)NoTradeFromDate ?? DBNull.Value
            };

            SqlParameter paraNoTradeToDate = new SqlParameter
            {
                ParameterName = "NoTradeToDate",
                DbType = DbType.DateTime,
                Value = (object)NoTradeToDate ?? DBNull.Value
            };
            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = (object)CompanyName ?? DBNull.Value
            };

            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)traderId ?? DBNull.Value
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

            List<CurrencyCloudReportModel> result = _usersRepository.ExecuteStoredProcedureList<CurrencyCloudReportModel>("usp_ClientMaster_GetCurrencyCloudClientList", paraClientName, paraAccountNo, paraNoTradeFromDate, paraNoTradeToDate, paraCompanyName, paraTraderId, paraSalesPersonId, paraJuniorSalesPersonId).ToList();
            return result;
        }

        public List<ClientMarginReportModel> GetClientMarginList(string ClientName, string CompanyName, string AccountNo, long traderId = 0, long SalesPersonId = 0, long? JuniorSalesPersonId = 0)
        {
            SqlParameter paraClientName = new SqlParameter
            {
                ParameterName = "ClientName",
                DbType = DbType.String,
                Value = (object)ClientName ?? DBNull.Value
            };

            SqlParameter paraAccountNo = new SqlParameter
            {
                ParameterName = "AccountNo",
                DbType = DbType.String,
                Value = (object)AccountNo ?? DBNull.Value
            };

            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = (object)CompanyName ?? DBNull.Value
            };

            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)traderId ?? DBNull.Value
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

            List<ClientMarginReportModel> result = _usersRepository.ExecuteStoredProcedureList<ClientMarginReportModel>("usp_ClientMaster_GetClientMarginList", paraClientName, paraAccountNo, paraCompanyName, paraTraderId, paraSalesPersonId, paraJuniorSalesPersonId).ToList();
            return result;
        }
        public List<ProfitLossReportModel> GetProfitLossReportList(int pageNo, int pageSize, string sortColumn, string sortDir, string ClientName, string CompanyName, string AccountNo, string DealNo, DateTime? FromDate = null, DateTime? ToDate = null, long traderId = 0, long SalesPersonId = 0, long? JuniorSalesPersonId = 0)
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


            SqlParameter paraClientName = new SqlParameter
            {
                ParameterName = "ClientName",
                DbType = DbType.String,
                Value = (object)ClientName ?? DBNull.Value
            };

            SqlParameter paraAccountNo = new SqlParameter
            {
                ParameterName = "AccountNo",
                DbType = DbType.String,
                Value = (object)AccountNo ?? DBNull.Value
            };
            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = (object)CompanyName ?? DBNull.Value
            };
            SqlParameter paraDealNo = new SqlParameter
            {
                ParameterName = "DealNo",
                DbType = DbType.String,
                Value = (object)DealNo ?? DBNull.Value
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

            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)traderId ?? DBNull.Value
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

            List<ProfitLossReportModel> result = _usersRepository.ExecuteStoredProcedureList<ProfitLossReportModel>
                ("usp_ClientMaster_GetProfitLossList", paraPageNo,
                paraPageSize, paraSortColumn, paraSortOrder
                , paraClientName, paraAccountNo, paraCompanyName, paraDealNo, paraFromDate, paraToDate, paraTraderId, paraSalesPersonId, paraJuniorSalesPersonId).ToList();
            return result;
        }

        public List<NewClientReportModel> GetNewClientsReportList(int? Month, int? Year, long TraderId, long SalesPersonId, long? JuniorSalesPersonId)
        {
            SqlParameter paraMonth = new SqlParameter
            {
                ParameterName = "Month",
                DbType = DbType.Int32,
                Value = (object)Month ?? DBNull.Value
            };

            SqlParameter paraYear = new SqlParameter
            {
                ParameterName = "Year",
                DbType = DbType.Int32,
                Value = (object)Year ?? DBNull.Value
            };

            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)TraderId ?? DBNull.Value
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

            List<NewClientReportModel> result = _usersRepository.ExecuteStoredProcedureList<NewClientReportModel>("usp_NewClientsReport", paraMonth, paraYear, paraTraderId, paraSalesPersonId, paraJuniorSalesPersonId).ToList();
            return result;
        }


        public NewClientReportModel GetNewIndividualClientsReportList(int? Month, int? Year, long clientId)
        {
            SqlParameter paraMonth = new SqlParameter
            {
                ParameterName = "Month",
                DbType = DbType.Int32,
                Value = (object)Month ?? DBNull.Value
            };

            SqlParameter paraYear = new SqlParameter
            {
                ParameterName = "Year",
                DbType = DbType.Int32,
                Value = (object)Year ?? DBNull.Value
            };

            SqlParameter paraclientId = new SqlParameter
            {
                ParameterName = "clientId",
                DbType = DbType.Int64,
                Value = (object)clientId ?? DBNull.Value
            };

            NewClientReportModel result = _usersRepository.ExecuteStoredProcedureList<NewClientReportModel>("usp_NewClientsReport_deal", paraMonth, paraYear, paraclientId).FirstOrDefault();
            return result;
        }

        public List<VolumeReportByMonthodelMonthYear> GetVolumeReportByMonthList(int? StartMonth, int? StartYear, int? EndMonth, int? EndYear)
        {
            SqlParameter paraStartMonth = new SqlParameter
            {
                ParameterName = "StartMonth",
                DbType = DbType.Int32,
                Value = (object)StartMonth ?? DBNull.Value
            };

            SqlParameter paraStartYear = new SqlParameter
            {
                ParameterName = "StartYear",
                DbType = DbType.Int32,
                Value = (object)StartYear ?? DBNull.Value
            };

            SqlParameter paraEndMonth = new SqlParameter
            {
                ParameterName = "EndMonth",
                DbType = DbType.Int32,
                Value = (object)EndMonth ?? DBNull.Value
            };

            SqlParameter paraEndYear = new SqlParameter
            {
                ParameterName = "EndYear",
                DbType = DbType.Int32,
                Value = (object)EndYear ?? DBNull.Value
            };

            List<VolumeReportByMonthodelMonthYear> result = _usersRepository.ExecuteStoredProcedureList<VolumeReportByMonthodelMonthYear>("usp_VolumeReportByMonth", paraStartMonth, paraStartYear, paraEndMonth, paraEndYear).ToList();
            return result;
        }

        public List<VolumeReportByMonthodelMonthYear> GetVolumeReportByTraderList(int? Month, int? Year)
        {
            SqlParameter paraMonth = new SqlParameter
            {
                ParameterName = "Month",
                DbType = DbType.Int32,
                Value = (object)Month ?? DBNull.Value
            };

            SqlParameter paraYear = new SqlParameter
            {
                ParameterName = "Year",
                DbType = DbType.Int32,
                Value = (object)Year ?? DBNull.Value
            };

            List<VolumeReportByMonthodelMonthYear> result = _usersRepository.ExecuteStoredProcedureList<VolumeReportByMonthodelMonthYear>("usp_VolumeReportByTrader", paraMonth, paraYear).ToList();
            return result;
        }

        public List<VolumeReportByMonthodelMonthYear> GetVolumeReportByCategoryList(int? Month, int? Year)
        {
            SqlParameter paraMonth = new SqlParameter
            {
                ParameterName = "Month",
                DbType = DbType.Int32,
                Value = (object)Month ?? DBNull.Value
            };

            SqlParameter paraYear = new SqlParameter
            {
                ParameterName = "Year",
                DbType = DbType.Int32,
                Value = (object)Year ?? DBNull.Value
            };

            List<VolumeReportByMonthodelMonthYear> result = _usersRepository.ExecuteStoredProcedureList<VolumeReportByMonthodelMonthYear>("usp_VolumeReportByCategory", paraMonth, paraYear).ToList();
            return result;
        }

        public List<ClientDDL> GetClientListForDropdown()
        {
            List<ClientDDL> result = _usersRepository.ExecuteStoredProcedureList<ClientDDL>("usp_ClientMaster_GetClientListDDL").ToList();
            return result;
        }
        public List<ClientRevenueReport> GetNewClientRevenueList(int Month, int Year, string ClientName, string CompanyName, long traderId, long SalesPersonId, long? JuniorSalesPersonId)
        {
            SqlParameter paraMonth = new SqlParameter
            {
                ParameterName = "Month",
                DbType = DbType.Int32,
                Value = (object)Month ?? DBNull.Value
            };

            SqlParameter paraYear = new SqlParameter
            {
                ParameterName = "ToYear",
                DbType = DbType.Int32,
                Value = (object)Year ?? DBNull.Value
            };

            SqlParameter paraClientName = new SqlParameter
            {
                ParameterName = "ClientName",
                DbType = DbType.String,
                Value = (object)ClientName ?? DBNull.Value
            };

            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = (object)CompanyName ?? DBNull.Value
            };

            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)traderId ?? DBNull.Value
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

            List<ClientRevenueReport> result = _usersRepository.ExecuteStoredProcedureList<ClientRevenueReport>("MIS_GetNewClientRenvenueReport", paraMonth, paraYear, paraClientName, paraCompanyName, paraTraderId, paraSalesPersonId, paraJuniorSalesPersonId).ToList();
            return result;
        }


        public List<ClientRevenueReport> GetNewClientRevenueList_settledfigures(int Month, int Year, string ClientName, string CompanyName, long traderId, long SalesPersonId, long? JuniorSalesPersonId)
        {
            SqlParameter paraMonth = new SqlParameter
            {
                ParameterName = "Month",
                DbType = DbType.Int32,
                Value = (object)Month ?? DBNull.Value
            };

            SqlParameter paraYear = new SqlParameter
            {
                ParameterName = "ToYear",
                DbType = DbType.Int32,
                Value = (object)Year ?? DBNull.Value
            };

            SqlParameter paraClientName = new SqlParameter
            {
                ParameterName = "ClientName",
                DbType = DbType.String,
                Value = (object)ClientName ?? DBNull.Value
            };

            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = (object)CompanyName ?? DBNull.Value
            };

            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)traderId ?? DBNull.Value
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
            List<ClientRevenueReport> result = _usersRepository.ExecuteStoredProcedureList<ClientRevenueReport>("MIS_GetNewClientRenvenueReport_SettledFigures", paraMonth, paraYear, paraClientName, paraCompanyName, paraTraderId, paraSalesPersonId, paraJuniorSalesPersonId).ToList();
            return result;
        }
        #endregion

    }
}
