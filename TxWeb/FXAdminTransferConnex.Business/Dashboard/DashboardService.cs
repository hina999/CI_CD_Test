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

namespace FXAdminTransferConnex.Business.Dashboard
{
    public class DashboardService : IDashboardService
    {

        #region Fields

        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<UserMaster> _usersRepository;

        #endregion

        #region ctor

        public DashboardService(IRepository<UserMaster> providerRepository)
        {
            _usersRepository = providerRepository;
        }

        #endregion


        public List<ReconciliationTeamMemberModel> GetReconciliationTeamCurrentMonthReport()
        {
            List<ReconciliationTeamMemberModel> result = _usersRepository.ExecuteStoredProcedure<ReconciliationTeamMemberModel>("usp_SalesFigureTeam_GetCurrentMonthReport_Dashboard").ToList();
            return result;
        }

        public List<Last12MonthsCommissionModel> GetLast12MonthsProfitList(long userId, long traderId)
        {
            SqlParameter paramFromDate = new SqlParameter
            {
                ParameterName = "DateFrom",
                DbType = DbType.Date,
                Value = DateTime.UtcNow.AddMonths(-12)
            };

            SqlParameter paramToDate = new SqlParameter
            {
                ParameterName = "DateTo",
                DbType = DbType.Date,
                Value = DateTime.UtcNow
            };

            SqlParameter paramUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = userId
            };

            SqlParameter paramTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = traderId
            };

            return _usersRepository.ExecuteStoredProcedureList<Last12MonthsCommissionModel>("usp_DashboardLast12MonthsCommission", paramFromDate, paramToDate, paramUserId, paramTraderId).ToList();
        }

        public List<TraderCommissions> GetTraderCommissionsList(long userId)
        {
            SqlParameter paramUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = userId
            };
            return _usersRepository.ExecuteStoredProcedureList<TraderCommissions>("usp_DashboardTraderCommissions", paramUserId).ToList();
        }



        public List<SalesPersonCommissions> GetSalesPersonCommissionsList(long userId)
        {
            SqlParameter paramUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = userId
            };
            return _usersRepository.ExecuteStoredProcedureList<SalesPersonCommissions>("usp_DashboardSalesPersonCommissions", paramUserId).ToList();
        }


        public List<Top10ClientCommissions> GetTop10ClientCommissionsList(long traderId, long salespersonId)
        {
            SqlParameter paramFromDate = new SqlParameter
            {
                ParameterName = "DateFrom",
                DbType = DbType.Date,
                Value = DateTime.UtcNow.AddMonths(-12)
            };

            SqlParameter paramToDate = new SqlParameter
            {
                ParameterName = "DateTo",
                DbType = DbType.Date,
                Value = DateTime.UtcNow
            };

            SqlParameter paramTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = traderId
            };

            SqlParameter paramSalesPersonID = new SqlParameter
            {
                ParameterName = "SalesPersonId",
                DbType = DbType.Int64,
                Value = salespersonId
            };

            return _usersRepository.ExecuteStoredProcedureList<Top10ClientCommissions>("usp_DashboardTop10ClientCommissions", paramFromDate, paramToDate, paramTraderId, paramSalesPersonID).ToList();
        }

        public List<ClientMasterModel> GetAwaitingActionCount()
        {
            SqlParameter paramTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.TraderId
            };
            return _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("usp_Dashboard_GetAwaitingActionClient", paramTraderId).ToList();
        }


        public List<MarketOrderSettingModel> GetMarketOrderCount(long TraderId = 0, long SalesPersonId = 0)
        {
            //added
            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = DBNull.Value
            };
            SqlParameter paraProspectId = new SqlParameter
            {
                ParameterName = "ProspectId",
                DbType = DbType.Int64,
                Value = DBNull.Value
            };
            SqlParameter paraFullName = new SqlParameter
            {
                ParameterName = "FullName",
                DbType = DbType.String,
                Value = DBNull.Value
            };
            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = DBNull.Value
            };
            SqlParameter paraEmailAddress = new SqlParameter
            {
                ParameterName = "EmailAddress",
                DbType = DbType.String,
                Value = DBNull.Value
            };
            SqlParameter paraStatus = new SqlParameter
            {
                ParameterName = "Status",
                DbType = DbType.String,
                Value = DBNull.Value
            };
            SqlParameter paraFromCurrency = new SqlParameter
            {
                ParameterName = "FromCurrency",
                DbType = DbType.Int32,
                Value = DBNull.Value
            };
            SqlParameter paraToCurrency = new SqlParameter
            {
                ParameterName = "ToCurrency",
                DbType = DbType.Int32,
                Value = DBNull.Value
            };


            SqlParameter paramTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = TraderId
            };
            SqlParameter paramSalesPersonId = new SqlParameter
            {
                ParameterName = "SalesPersonId",
                DbType = DbType.Int64,
                Value = SalesPersonId
            };
            List<MarketOrderSettingModel> marketOrderSettingList = _usersRepository.ExecuteStoredProcedureList<MarketOrderSettingModel>("usp_MarketOrderSettingList",paraClientId, paraProspectId, paraFullName, paraCompanyName, paraEmailAddress, paraStatus, paraFromCurrency, paraToCurrency, paramTraderId, paramSalesPersonId).ToList();
            return marketOrderSettingList;
            
            //return _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("usp_Dashboard_GetMarketOrderClient", paramTraderId, paramSalesPersonId).ToList();
        }


        public List<Last30DaysCommissionModel> GetLast30DaysProfitList(long userId, long traderId)
        {

            SqlParameter paramUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = userId
            };

            SqlParameter paramTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = traderId
            };

            return _usersRepository.ExecuteStoredProcedureList<Last30DaysCommissionModel>("usp_DashboardLast30DaysCommission", paramUserId, paramTraderId).ToList();
        }

        public List<Last30DaysCommissionModel> GetLast5DaysProfitList(long userId, long traderId)
        {

            SqlParameter paramUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = userId
            };

            SqlParameter paramTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = traderId
            };

            return _usersRepository.ExecuteStoredProcedureList<Last30DaysCommissionModel>("usp_DashboardLast5DaysCommission", paramUserId, paramTraderId).ToList();
        }

        public List<Last6WeeksCommissionModel> GetLast6WeeksCommissionList(long userId, long traderId)
        {

            SqlParameter paramUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = userId
            };

            SqlParameter paramTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = traderId
            };

            return _usersRepository.ExecuteStoredProcedureList<Last6WeeksCommissionModel>("usp_DashboardLast6WeeksCommission", paramUserId, paramTraderId).ToList();
        }

        public List<TaskReminderModel> GetTodayPopupTaskRemider(DateTime SysDate)
        {
            SqlParameter paramUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };

            SqlParameter parameterDateTime = new SqlParameter
            {
                ParameterName = "SystemDateTime",
                DbType = DbType.DateTime,
                Value = SysDate
            };

            return _usersRepository.ExecuteStoredProcedureList<TaskReminderModel>("usp_TaskReminder_GetTodayTaskReminderPopUp", paramUserId, parameterDateTime).ToList();
        }

        public List<TaskReminderModel> GetPopupTaskRemider()
        {
            SqlParameter paramUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };

            return _usersRepository.ExecuteStoredProcedureList<TaskReminderModel>("usp_TaskReminder_GetTaskReminderPopUp", paramUserId).ToList();
        }

        public int TaskSnooze(int TaskId, DateTime SysDate)
        {
            SqlParameter paramTaskId = new SqlParameter
            {
                ParameterName = "TaskId",
                DbType = DbType.Int32,
                Value = TaskId
            };

            SqlParameter paramCurrDate = new SqlParameter
            {
                ParameterName = "CurrDate",
                DbType = DbType.DateTime,
                Value = SysDate
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_TaskReminder_Snooze", paramTaskId, paramCurrDate).FirstOrDefault();
            return result;
        }

        public int TaskSnoozeAll(long UserId, DateTime SysDate)
        {
            SqlParameter paramUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int32,
                Value = UserId
            };

            SqlParameter paramCurrDate = new SqlParameter
            {
                ParameterName = "SystemDateTime",
                DbType = DbType.DateTime,
                Value = SysDate
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_TaskReminder_SnoozeAll", paramUserId, paramCurrDate).FirstOrDefault();
            return result;
        }

        public int TaskDismiss(int TaskId)
        {
            SqlParameter paramTaskId = new SqlParameter
            {
                ParameterName = "TaskId",
                DbType = DbType.Int32,
                Value = TaskId
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_TaskReminder_Dismiss", paramTaskId).FirstOrDefault();
            return result;
        }


        public int RescheduleTaskReminder(DashboardTaskReminderModel model)
        {
            SqlParameter paraTaskId = new SqlParameter
            {
                ParameterName = "TaskId",
                DbType = DbType.Int64,
                Value = (object)model.TaskId ?? DBNull.Value
            };

            SqlParameter paraTaskReminderDatetime = new SqlParameter
            {
                ParameterName = "TaskReminderDatetime",
                DbType = DbType.DateTime,
                Value = (object)model.TaskReminderDatetime ?? DBNull.Value
            };

            SqlParameter paraNotes = new SqlParameter
            {
                ParameterName = "Notes",
                DbType = DbType.String,
                Value = (object)model.Notes ?? DBNull.Value
            };

            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = (object)ProjectSession.LoginUserDetails.UserId ?? DBNull.Value
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_TaskReminder_Reschedule", paraTaskId, paraTaskReminderDatetime, paraNotes, paraUserId).FirstOrDefault();
            return result;
        }

        #region Market Order Notification
        public List<MarketValueNotificationModel> GetMarketOrderNotificationList(long TraderId, long SalesPersonId)
        {
            SqlParameter paramTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = TraderId
            };
            SqlParameter paramSalesPersonId = new SqlParameter
            {
                ParameterName = "SalesPersonId",
                DbType = DbType.Int64,
                Value = SalesPersonId
            };
            return _usersRepository.ExecuteStoredProcedureList<MarketValueNotificationModel>("usp_GetMarketValueNotificationList", paramTraderId, paramSalesPersonId).ToList();
        }

        public bool MarketOrderNotificationUpdate(long NotificationId)
        {
            SqlParameter paramNotificationId = new SqlParameter
            {
                ParameterName = "NotificationId",
                DbType = DbType.Int64,
                Value = NotificationId
            };


            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_MarketValueNotification_Update", paramNotificationId).FirstOrDefault();
            if (result == 1)
                return true;
            else
            {
                return false;
            }
        }
        #endregion

        #region RingCentral
        public List<RingCentralModel> GetLast5DayInCallLog()
        {
            return _usersRepository.ExecuteStoredProcedureList<RingCentralModel>("usp_RingCentralCallLog_GetLast5DayInCallLog").ToList();
        }

        public List<RingCentralModel> GetLast5DayOutCallLog()
        {
            return _usersRepository.ExecuteStoredProcedureList<RingCentralModel>("usp_RingCentralCallLog_GetLast5DayOutCallLog").ToList();
        }

        public List<RingCentralModel> GetYesterdaysOutCallLog()
        {
            return _usersRepository.ExecuteStoredProcedureList<RingCentralModel>("usp_RingCentralCallLog_YesterdaysOutCall").ToList();
        }

        public List<RingCentralModel> GetYesterdaysInCallLog()
        {
            return _usersRepository.ExecuteStoredProcedureList<RingCentralModel>("usp_RingCentralCallLog_YesterdaysInCall").ToList();
        }

        public List<GlobalSearchModel> GetGlobalSearchData(string searchString)
        {
            SqlParameter paramSearchString = new SqlParameter
            {
                ParameterName = "searchString",
                DbType = DbType.String,
                Value = (object)searchString ?? DBNull.Value
            };

            return _usersRepository.ExecuteStoredProcedureList<GlobalSearchModel>("USP_GlobalSearch", paramSearchString).ToList();
        }
        #endregion

        #region Company Commission
        public List<Last12MonthsCommissionModel> GetLast12MonthsCompanyCommission()
        {
            SqlParameter paramFromDate = new SqlParameter
            {
                ParameterName = "DateFrom",
                DbType = DbType.Date,
                Value = DateTime.UtcNow.AddMonths(-12)
            };

            SqlParameter paramToDate = new SqlParameter
            {
                ParameterName = "DateTo",
                DbType = DbType.Date,
                Value = DateTime.UtcNow
            };

            return _usersRepository.ExecuteStoredProcedureList<Last12MonthsCommissionModel>("usp_DashboardCompanyLast12MonthsCommission", paramFromDate, paramToDate).ToList();
        }

        public List<Last30DaysCommissionModel> GetLast30DaysCompanyCommission()
        {
            return _usersRepository.ExecuteStoredProcedureList<Last30DaysCommissionModel>("usp_DashboardLast30DaysCompanyCommission").ToList();
        }

        public List<Last30DaysCommissionModel> GetLast5DaysCompanyCommission()
        {
            return _usersRepository.ExecuteStoredProcedureList<Last30DaysCommissionModel>("usp_DashboardLast5DaysComapnyCommission").ToList();
        }
        public List<Last6WeeksCommissionModel> GetLast6WeeksCompanyCommission()
        {
            return _usersRepository.ExecuteStoredProcedureList<Last6WeeksCommissionModel>("usp_DashboardLast6WeeksCompanyCommission").ToList();
        }
        #endregion
    }
}
