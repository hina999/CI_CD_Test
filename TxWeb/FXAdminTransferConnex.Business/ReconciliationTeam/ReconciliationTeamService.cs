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

namespace FXAdminTransferConnex.Business.ReconciliationTeam
{
    public class ReconciliationTeamService : IReconciliationTeamService
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
        private readonly IRepository<ReconciliationTeamModel> _reconciliationTeamRepository;


        #endregion

        #region ctor
        public ReconciliationTeamService(IRepository<ReconciliationTeamModel> providerRepository)
        {
            _reconciliationTeamRepository = providerRepository;
        }
        #endregion

        #region methods

        public bool AddUpdateReconciliationTeamData(long CreatedBy, DateTime RecordDate, DataTable TeamMember)
        {
            try
            {
                SqlParameter paraCreatedBy = new SqlParameter
                {
                    ParameterName = "CreatedBy",
                    DbType = DbType.Int64,
                    Value = (object)CreatedBy ?? DBNull.Value
                };

                SqlParameter paraRecordDate = new SqlParameter
                {
                    ParameterName = "RecordDate",
                    DbType = DbType.Date,
                    Value = (object)RecordDate ?? DBNull.Value
                };

                SqlParameter paramTeamMember = new SqlParameter
                {
                    ParameterName = "TeamMember",
                    SqlDbType = SqlDbType.Structured,
                    Value = TeamMember,
                    TypeName = "dbo.tt_ReconciliationTeam"
                };

                string result = _reconciliationTeamRepository.ExecuteStoredProcedure<string>("usp_ReconciliationTeam_AddEdit", paraCreatedBy, paraRecordDate, paramTeamMember).FirstOrDefault();

                if (string.IsNullOrEmpty(result))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }

        public List<ReconciliationTeamMemberModel> GetReconciliationTeamData(long? TraderId, DateTime RecordDate)
        {
            try
            {
                SqlParameter paraTraderId = new SqlParameter
                {
                    ParameterName = "TraderId",
                    DbType = DbType.Int64,
                    Value = (object)TraderId ?? DBNull.Value
                };

                SqlParameter paraRecordDate = new SqlParameter
                {
                    ParameterName = "RecordDate",
                    DbType = DbType.Date,
                    Value = (object)RecordDate ?? DBNull.Value
                };

                List<ReconciliationTeamMemberModel> result = _reconciliationTeamRepository.ExecuteStoredProcedure<ReconciliationTeamMemberModel>("usp_ReconciliationTeam_GetByDate", paraTraderId, paraRecordDate).ToList();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<ReconciliationTeamMemberModel>();
            }
        }

        public List<ReconciliationTeamMemberModel> GetReconciliationTeamReport(DateTime? FromDate, DateTime? ToDate)
        {
            try
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

                List<ReconciliationTeamMemberModel> result = _reconciliationTeamRepository.ExecuteStoredProcedure<ReconciliationTeamMemberModel>("usp_ReconciliationTeam_GetReport", paraFromDate, paraToDate).ToList();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<ReconciliationTeamMemberModel>();
            }
        }

        public List<UserModel> GetSalesPersoListByTraderId(long? TraderId)
        {
            try
            {
                SqlParameter paraTraderId = new SqlParameter
                {
                    ParameterName = "TraderId",
                    DbType = DbType.Int64,
                    Value = (object)TraderId ?? DBNull.Value
                };

                List<UserModel> result = _reconciliationTeamRepository.ExecuteStoredProcedure<UserModel>("GetSalesPersoListByTraderId", paraTraderId).ToList();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<UserModel>();
            }
        }

        public List<ReconciliationTeamMemberModel> GetReconciliationTopReport()
        {
            try
            {
                List<ReconciliationTeamMemberModel> result = _reconciliationTeamRepository.ExecuteStoredProcedure<ReconciliationTeamMemberModel>("usp_ReconciliationTop_GetCurruntReport").ToList();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<ReconciliationTeamMemberModel>();
            }
        }


        public List<ReconciliationTeamMemberModel> GetReconciliationTeamCurrentYearReport()
        {
            try
            {
                List<ReconciliationTeamMemberModel> result = _reconciliationTeamRepository.ExecuteStoredProcedure<ReconciliationTeamMemberModel>("usp_ReconciliationTeam_GetCurrentYearReport").ToList();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<ReconciliationTeamMemberModel>();
            }
        }
        public List<ReconciliationTeamMemberModel> GetReconciliationTeamCurrentMonthReport()
        {
            try
            {
                List<ReconciliationTeamMemberModel> result = _reconciliationTeamRepository.ExecuteStoredProcedure<ReconciliationTeamMemberModel>("usp_ReconciliationTeam_GetCurrentMonthReport").ToList();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<ReconciliationTeamMemberModel>();
            }
        }

        #endregion


    }
}
