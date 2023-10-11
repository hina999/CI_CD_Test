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

namespace FXAdminTransferConnex.Business.Ringcentral
{
    public class RingcentralService : IRingcentralService
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

        public RingcentralService(IRepository<UserMaster> providerRepository)
        {
            _usersRepository = providerRepository;
        }
        #endregion
        #region methods
        

        public List<RingCentralModel> GetCallAcceptancePercentage()
        {
            return _usersRepository.ExecuteStoredProcedureList<RingCentralModel>("usp_RingCentralCallLog_GetCallAcceptancePercentage").ToList();
        }

        public List<RingCentralModel> GetEmployeeWiseCallCount()
        {
            return _usersRepository.ExecuteStoredProcedureList<RingCentralModel>("usp_RingCentralCallLog_GetEmployeeWiseCallCount").ToList();
        }

        public List<RingCentralModel> GetInboundCount()
        {
            return _usersRepository.ExecuteStoredProcedureList<RingCentralModel>("usp_RingCentralCallLog_GetInboundCount").ToList();
        }

        public List<RingCentralModel> GetOutboundCount()
        {
            return _usersRepository.ExecuteStoredProcedureList<RingCentralModel>("usp_RingCentralCallLog_GetOutboundCount").ToList();
        }

        public List<RingCentralModel> GetRingcentralCallList(DateTime? FromDate, DateTime? ToDate, string MobileNo, string Name, string CallDirection, int PageNumber, int PageSize)
        {
            SqlParameter paraFromDate = new SqlParameter
            {
                ParameterName = "FromDate",
                DbType = DbType.Date,
                Value = (object)FromDate ?? DBNull.Value
            };

            SqlParameter paraToDate = new SqlParameter
            {
                ParameterName = "ToDate",
                DbType = DbType.Date,
                Value = (object)ToDate ?? DBNull.Value
            };

            SqlParameter paraMobileNo = new SqlParameter
            {
                ParameterName = "MobileNo",
                DbType = DbType.String,
                Value = (object)MobileNo ?? DBNull.Value
            };

            SqlParameter paraName = new SqlParameter
            {
                ParameterName = "Name",
                DbType = DbType.String,
                Value = (object)Name ?? DBNull.Value
            };

            SqlParameter paraCallDirectione = new SqlParameter
            {
                ParameterName = "CallDirection",
                DbType = DbType.String,
                Value = (object)CallDirection ?? DBNull.Value
            };

            SqlParameter paraPageNumber = new SqlParameter
            {
                ParameterName = "PageNumber",
                DbType = DbType.Int32,
                Value = (object)PageNumber ?? DBNull.Value
            };

            SqlParameter paraPageSize = new SqlParameter
            {
                ParameterName = "PageSize",
                DbType = DbType.Int32,
                Value = (object)PageSize ?? DBNull.Value
            };

            return _usersRepository.ExecuteStoredProcedureList<RingCentralModel>("usp_RingCentralCallLog_GetList", paraFromDate, paraToDate, paraMobileNo, paraName, paraCallDirectione, paraPageNumber, paraPageSize).ToList();
        }

        public List<RingCentralModel> GetTop3Caller()
        {
            return _usersRepository.ExecuteStoredProcedureList<RingCentralModel>("usp_RingCentralCallLog_GetTop3Caller").ToList();
        }

        public List<RingCentralModel> Last5DayInOutCallLog()
        {
            return _usersRepository.ExecuteStoredProcedureList<RingCentralModel>("usp_RingCentralCallLog_GetLast5DayInOutCallLog").ToList();
        }

        public List<RingCentralModel> GetClientProspectCallLogList(string MobileNo, string AltMobileNo)
        {
            SqlParameter paraMobileNo = new SqlParameter
            {
                ParameterName = "MobileNo",
                DbType = DbType.String,
                Value = (object)MobileNo ?? DBNull.Value
            };

            SqlParameter paraAltMobileNo = new SqlParameter
            {
                ParameterName = "AltMobileNo",
                DbType = DbType.String,
                Value = (object)AltMobileNo ?? DBNull.Value
            };

            return _usersRepository.ExecuteStoredProcedureList<RingCentralModel>("usp_CallLogList_ClientProspect", paraMobileNo, paraAltMobileNo).ToList();
        }

        public List<RingCentralModel> GetTodaysCommission()
        {
            return _usersRepository.ExecuteStoredProcedureList<RingCentralModel>("USP_GetTodaysCompanyCommission").ToList();
        }

        public long SaveCurrencyRate(CurrencyPairModel model)
        {
            int result = 0;
            try
            {
                DataTable dtTable = new DataTable("CurrencyPair");
                dtTable.Columns.Add("CurrencyRate", typeof(string));
                dtTable.Columns.Add("Rate", typeof(decimal));

                DataRow dtRow;
                dtRow = dtTable.NewRow();
                dtRow["CurrencyRate"] = "EURUSD";
                dtRow["Rate"] = model.EURUSD;
                dtTable.Rows.Add(dtRow);

                dtRow = dtTable.NewRow();
                dtRow["CurrencyRate"] = "GBPAUD";
                dtRow["Rate"] = model.GBPAUD;
                dtTable.Rows.Add(dtRow);

                dtRow = dtTable.NewRow();
                dtRow["CurrencyRate"] = "GBPCAD";
                dtRow["Rate"] = model.GBPCAD;
                dtTable.Rows.Add(dtRow);

                dtRow = dtTable.NewRow();
                dtRow["CurrencyRate"] = "GBPCNY";
                dtRow["Rate"] = model.GBPCNY;
                dtTable.Rows.Add(dtRow);

                dtRow = dtTable.NewRow();
                dtRow["CurrencyRate"] = "GBPDKK";
                dtRow["Rate"] = model.GBPDKK;
                dtTable.Rows.Add(dtRow);

                dtRow = dtTable.NewRow();
                dtRow["CurrencyRate"] = "GBPEUR";
                dtRow["Rate"] = model.GBPEUR;
                dtTable.Rows.Add(dtRow);

                dtRow = dtTable.NewRow();
                dtRow["CurrencyRate"] = "GBPJPY";
                dtRow["Rate"] = model.GBPJPY;
                dtTable.Rows.Add(dtRow);

                dtRow = dtTable.NewRow();
                dtRow["CurrencyRate"] = "GBPNOK";
                dtRow["Rate"] = model.GBPNOK;
                dtTable.Rows.Add(dtRow);

                dtRow = dtTable.NewRow();
                dtRow["CurrencyRate"] = "GBPUSD";
                dtRow["Rate"] = model.GBPUSD;
                dtTable.Rows.Add(dtRow);

                SqlParameter currencyRatePair = new SqlParameter
                {
                    ParameterName = "CurrencyPair",
                    SqlDbType = SqlDbType.Structured,
                    Value = dtTable,
                    TypeName = "dbo.tt_CurrencyRatePair"
                };
                result = _usersRepository.ExecuteStoredProcedure<int>("usp_AddCurrencyRate", currencyRatePair).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return result;
        }

        public List<CurrencyPairModel> GetCurrencyRatePair()
        {
            return _usersRepository.ExecuteStoredProcedureList<CurrencyPairModel>("usp_GetCurrencyRate").ToList();
        }

        #endregion
    }
}
