using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using FXAdminTransferConnex.Data.Repository;
using FXAdminTransferConnex.Entities;
using log4net;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Business.Commission
{
    public class CommissionService : ICommissionService
    {

        #region Constants


        #endregion

        #region Fields

        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<TargetCommissionModel> _targetCommissionRepository;

        #endregion

        #region ctor

        public CommissionService(IRepository<TargetCommissionModel> providerRepository)
        {
            _targetCommissionRepository = providerRepository;
        }

        #endregion

        public long SaveTargetCommison(TargetCommissionModel model, DataTable dtTargetDuration)
        {
            SqlParameter paraTargetCommissionId = new SqlParameter
            {
                ParameterName = "TargetCommissionId",
                DbType = DbType.Int64,
                Value = (object)model.TargetCommissionId ?? DBNull.Value
            };
            SqlParameter paraTargetYear = new SqlParameter
            {
                ParameterName = "TargetYear",
                DbType = DbType.Int32,
                Value = (object)model.TargetYear ?? DBNull.Value
            };

            SqlParameter paraTargetType = new SqlParameter
            {
                ParameterName = "TargetType",
                DbType = DbType.String,
                Value = (object)model.TargetType ?? DBNull.Value
            };

            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)model.TraderId ?? DBNull.Value
            };

            SqlParameter paraSalesPersonId = new SqlParameter
            {
                ParameterName = "SalesPersonId",
                DbType = DbType.Int64,
                Value = (object)model.SalesPersonId ?? DBNull.Value
            };
            SqlParameter paraLoggedinUserId = new SqlParameter
            {
                ParameterName = "LoggedinUserId",
                DbType = DbType.Int64,
                Value = (object)model.LoggedinUserId ?? DBNull.Value
            };
            SqlParameter paramdtTargetDuration = new SqlParameter
            {
                ParameterName = "dtTargetDuration",
                SqlDbType = SqlDbType.Structured,
                Value = dtTargetDuration,
                TypeName = "dbo.tt_TargetDurationList"
            };

            long result = _targetCommissionRepository.ExecuteStoredProcedure<long>("usp_TargetCommission_AddEdit_New",
                    paraTargetCommissionId, paraTargetYear, paraTargetType, paraTraderId, paraSalesPersonId, paraLoggedinUserId, paramdtTargetDuration).FirstOrDefault();

            return result;
        }

        public long SaveTargetCommison(TargetCommissionModel model)
        {
            SqlParameter paraTargetCommissionId = new SqlParameter
            {
                ParameterName = "TargetCommissionId",
                DbType = DbType.Int64,
                Value = (object)model.TargetCommissionId ?? DBNull.Value
            };

            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = (object)model.ClientId ?? DBNull.Value
            };

            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)model.TraderId ?? DBNull.Value
            };

            SqlParameter paraSalesPersonId = new SqlParameter
            {
                ParameterName = "SalesPersonId",
                DbType = DbType.Int64,
                Value = (object)model.SalesPersonId ?? DBNull.Value
            };

            SqlParameter paraTargetType = new SqlParameter
            {
                ParameterName = "TargetType",
                DbType = DbType.String,
                Value = (object)model.TargetType ?? DBNull.Value
            };
            SqlParameter paraTargetCommission = new SqlParameter
            {
                ParameterName = "TargetCommission",
                DbType = DbType.Decimal,
                Value = (object)model.TargetCommission ?? DBNull.Value
            };
            SqlParameter paraIsActive = new SqlParameter
            {
                ParameterName = "IsActive",
                DbType = DbType.Boolean,
                Value = (object)model.IsActive ?? DBNull.Value
            };

            SqlParameter paraLoggedinUserId = new SqlParameter
            {
                ParameterName = "LoggedinUserId",
                DbType = DbType.Int64,
                Value = (object)model.LoggedinUserId ?? DBNull.Value
            };

            long result = _targetCommissionRepository.ExecuteStoredProcedure<long>("usp_TargetCommission_Save", paraTargetCommissionId, paraClientId,
                paraTraderId, paraSalesPersonId, paraTargetType, paraTargetCommission, paraIsActive, paraLoggedinUserId).FirstOrDefault();
            return result;
        }

        public List<TargetCommissionModel> GetTargetCommissionlist()
        {
            return _targetCommissionRepository.ExecuteStoredProcedure<TargetCommissionModel>("usp_TargetCommission_GetList").ToList();
        }
        public TargetCommissionModel GetTargetCommissionById(long TargetCommissionId)
        {
            SqlParameter paraTargetCommissionId = new SqlParameter
            {
                ParameterName = "TargetCommissionId",
                DbType = DbType.Int64,
                Value = TargetCommissionId
            };
            return _targetCommissionRepository.ExecuteStoredProcedureList<TargetCommissionModel>("usp_TargetCommission_GetById", paraTargetCommissionId).FirstOrDefault();
        }
        public List<TargetDurationModel> GetDailyTargetCommissionById(long TargetCommissionId, int TargetYear)
        {
            SqlParameter paraTargetCommissionId = new SqlParameter
            {
                ParameterName = "TargetCommissionId",
                DbType = DbType.Int64,
                Value = TargetCommissionId
            };
            SqlParameter paraTargetYear = new SqlParameter
            {
                ParameterName = "TargetYear",
                DbType = DbType.Int32,
                Value = TargetYear
            };
            return _targetCommissionRepository.ExecuteStoredProcedureList<TargetDurationModel>("usp_DailyTargetCommission_GetById", paraTargetCommissionId, paraTargetYear).ToList();
        }
        public List<TargetDurationModel> GetWeeklyTargetCommissionById(long TargetCommissionId, int TargetYear)
        {
            SqlParameter paraTargetCommissionId = new SqlParameter
            {
                ParameterName = "TargetCommissionId",
                DbType = DbType.Int64,
                Value = TargetCommissionId
            };
            SqlParameter paraTargetYear = new SqlParameter
            {
                ParameterName = "TargetYear",
                DbType = DbType.Int32,
                Value = TargetYear
            };
            return _targetCommissionRepository.ExecuteStoredProcedureList<TargetDurationModel>("usp_WeeklyTargetCommission_GetById", paraTargetCommissionId, paraTargetYear).ToList();
        }
        public List<TargetDurationModel> GetMonthlyTargetCommissionById(long TargetCommissionId, int TargetYear)
        {
            SqlParameter paraTargetCommissionId = new SqlParameter
            {
                ParameterName = "TargetCommissionId",
                DbType = DbType.Int64,
                Value = TargetCommissionId
            };
            SqlParameter paraTargetYear = new SqlParameter
            {
                ParameterName = "TargetYear",
                DbType = DbType.Int32,
                Value = TargetYear
            };
            return _targetCommissionRepository.ExecuteStoredProcedureList<TargetDurationModel>("usp_MonthlyTargetCommission_GetById", paraTargetCommissionId, paraTargetYear).ToList();
        }
        public long DeleteTargetCommission(long TargetCommissionId)
        {
            SqlParameter paraTargetCommisionId = new SqlParameter
            {
                ParameterName = "TargetCommissionId",
                DbType = DbType.Int64,
                Value = (object)TargetCommissionId ?? DBNull.Value
            };

            long result = _targetCommissionRepository.ExecuteStoredProcedure<long>("usp_TargetCommission_DeleteById", paraTargetCommisionId).FirstOrDefault();
            return result;
        }


        public long ActivateDeactivateTargetCommission(long TargetCommissionId)
        {
            SqlParameter paraTargetCommisionId = new SqlParameter
            {
                ParameterName = "TargetCommissionId",
                DbType = DbType.Int64,
                Value = (object)TargetCommissionId ?? DBNull.Value
            };

            long result = _targetCommissionRepository.ExecuteStoredProcedure<long>("usp_TargetCommision_Active_DeActive", paraTargetCommisionId).FirstOrDefault();
            return result;
        }


        public long ImportTargetCommisions(long LoggedinUserId)
        {
            SqlParameter LoggedinUserIdParam = new SqlParameter
            {
                ParameterName = "LoggedinUserId",
                DbType = DbType.Int64,
                Value = (object)LoggedinUserId ?? DBNull.Value
            };

            int result = _targetCommissionRepository.ExecuteStoredProcedure<int>("usp_TargetCommisions_Import", LoggedinUserIdParam).FirstOrDefault();
            return result;
        }

    }
}
