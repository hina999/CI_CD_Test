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

namespace FXAdminTransferConnex.Business.Trader
{
    public class TraderService : ITraderService
    {
        #region Fields

        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<UserMaster> _usersRepository;

        #endregion

        #region ctor

        public TraderService(IRepository<UserMaster> providerRepository)
        {
            _usersRepository = providerRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// By Mayank
        /// 20 March 2018
        /// Get Trader details list
        /// </summary>
        /// <returns></returns>
        public List<TraderModel> GetTraderlist(int pageNo, int pageSize, string sortColumn, string sortDir, string search)
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

            SqlParameter paraSearch = new SqlParameter
            {
                ParameterName = "Search",
                DbType = DbType.String,
                Value = (object)search ?? DBNull.Value
            };

            List<TraderModel> traderlist = _usersRepository.ExecuteStoredProcedureList<TraderModel>("usp_TraderMaster_List", paraPageNo, paraPageSize, paraSortColumn, paraSortOrder, paraSearch).ToList();
            return traderlist;
        }

        /// <summary>
        /// By Mayank
        /// 20 March 2018
        /// Get Trader details list by TraderId
        /// </summary>
        /// <returns></returns>
        public TraderModel GetTraderDetailsByTraderId(long traderId)
        {
            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = traderId
            };

            TraderModel traderlist = _usersRepository.ExecuteStoredProcedureList<TraderModel>("usp_TraderMaster_GetDetailByTraderId", paraTraderId).FirstOrDefault();
            return traderlist;
        }


        /// <summary>
        /// By Mayank
        /// 20 March 2018
        /// Add/Edit trader details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long SaveTrader(TraderModel model)
        {
            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)model.TraderId ?? DBNull.Value
            };

            SqlParameter paraFirstName = new SqlParameter
            {
                ParameterName = "FirstName",
                DbType = DbType.String,
                Value = (object)model.FirstName ?? DBNull.Value
            };

            SqlParameter paraLastName = new SqlParameter
            {
                ParameterName = "LastName",
                DbType = DbType.String,
                Value = (object)model.LastName ?? DBNull.Value
            };

            SqlParameter paraEmailAddress = new SqlParameter
            {
                ParameterName = "EmailAddress",
                DbType = DbType.String,
                Value = (object)model.EmailAddress ?? DBNull.Value
            };

            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = (object)model.CompanyName ?? DBNull.Value
            };

            SqlParameter paraMobileNo = new SqlParameter
            {
                ParameterName = "MobileNo",
                DbType = DbType.String,
                Value = (object)model.MobileNo ?? DBNull.Value
            };

            SqlParameter paraJoiningDate = new SqlParameter
            {
                ParameterName = "JoiningDate",
                DbType = DbType.DateTime,
                Value = (object)model.JoiningDate ?? DBNull.Value
            };

            SqlParameter paraIsClose = new SqlParameter
            {
                ParameterName = "IsClose",
                DbType = DbType.Int16,
                Value = (object)model.IsClose ?? DBNull.Value
            };

            SqlParameter paraPassword = new SqlParameter
            {
                ParameterName = "Password",
                DbType = DbType.String,
                Value = Security.Encrypt(model.Password)
            };

            SqlParameter paraImageName = new SqlParameter
            {
                ParameterName = "ImageName",
                DbType = DbType.String,
                Value = (object)model.ImageName ?? DBNull.Value
            };

            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };

            long result;
            if (model.TraderId > 0)
            {
                result = _usersRepository.ExecuteStoredProcedure<long>("usp_TraderMaster_Update", paraTraderId, paraFirstName, paraLastName, paraEmailAddress, paraCompanyName, paraMobileNo, paraJoiningDate, paraIsClose, paraUserId, paraPassword, paraImageName).FirstOrDefault();
            }
            else
            {
                result = _usersRepository.ExecuteStoredProcedure<long>("usp_TraderMaster_Insert", paraFirstName, paraLastName, paraEmailAddress, paraCompanyName, paraMobileNo, paraJoiningDate, paraIsClose, paraUserId, paraPassword, paraImageName).FirstOrDefault();
            }
            return result;
        }


        /// <summary>
        /// By Mayank
        /// 20 March 2018
        /// Delete trader details
        /// </summary>
        /// <param name="TraderId"></param>
        /// <returns></returns>
        public int DeleteTrader(long TraderId)
        {
            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)TraderId ?? DBNull.Value
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_TraderMaster_Delete", paraTraderId).FirstOrDefault();
            return result;
        }

        #region Trader Commission

        /// <summary>
        /// By Mayank
        /// 22 March 2018
        /// Get Trader commission details list
        /// </summary>
        /// <returns></returns>
        public List<TraderCommissionModel> GetTraderCommissionList(long TraderId)
        {
            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)TraderId ?? DBNull.Value
            };

            List<TraderCommissionModel> Tradercommlist = _usersRepository.ExecuteStoredProcedureList<TraderCommissionModel>("usp_TraderCommission_List", paraTraderId).ToList();
            return Tradercommlist;
        }


        /// <summary>
        /// By Mayank
        /// 20 March 2018
        /// Add/Edit trader details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SaveTraderCommission(TraderCommissionModel model)
        {
            SqlParameter paraTraderCommissionId = new SqlParameter
            {
                ParameterName = "TraderCommissionId",
                DbType = DbType.Int64,
                Value = (object)model.TraderCommissionId ?? DBNull.Value
            };

            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)model.TraderId ?? DBNull.Value
            };

            SqlParameter paraDealFromAmt = new SqlParameter
            {
                ParameterName = "DealFromAmt",
                DbType = DbType.Int32,
                Value = (object)model.DealFromAmt ?? DBNull.Value
            };

            SqlParameter paraDealToAmt = new SqlParameter
            {
                ParameterName = "DealToAmt",
                DbType = DbType.Int32,
                Value = (object)model.DealToAmt ?? DBNull.Value
            };

            SqlParameter paraCommission = new SqlParameter
            {
                ParameterName = "Commission",
                DbType = DbType.Decimal,
                Value = (object)model.Commission ?? DBNull.Value
            };

            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };

            int result;
            if (model.TraderCommissionId > 0)
            {
                result = _usersRepository.ExecuteStoredProcedure<int>("usp_TraderCommission_Update", paraTraderCommissionId, paraTraderId, paraDealFromAmt, paraDealToAmt, paraCommission, paraUserId).FirstOrDefault();
            }
            else
            {
                result = _usersRepository.ExecuteStoredProcedure<int>("usp_TraderCommission_Insert", paraTraderId, paraDealFromAmt, paraDealToAmt, paraCommission, paraUserId).FirstOrDefault();
            }
            return result;
        }


        /// <summary>
        /// By Mayank
        /// 22 March 2018
        /// Delete trader commission details
        /// </summary>
        /// <param name="TraderCommissionId"></param>
        /// <returns></returns>
        public bool DeleteTraderCommission(long TraderCommissionId)
        {
            var paraTraderCommissionId = new SqlParameter
            {
                ParameterName = "TraderCommissionId",
                DbType = DbType.Int64,
                Value = (object)TraderCommissionId ?? DBNull.Value
            };

            var result = _usersRepository.ExecuteStoredProcedure<int>("usp_TraderCommission_Delete", paraTraderCommissionId).FirstOrDefault();
            return result > 0;
        }
        #endregion

        #endregion
    }
}
