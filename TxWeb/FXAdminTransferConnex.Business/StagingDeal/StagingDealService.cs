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

namespace FXAdminTransferConnex.Business.StagingDeal
{
    public class StagingDealService : IStagingDealService
    {
        #region Fields

        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<UserMaster> _usersRepository;

        #endregion

        #region ctor

        public StagingDealService(IRepository<UserMaster> providerRepository)
        {
            _usersRepository = providerRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Get stagingDeal details list
        /// </summary>
        /// <returns></returns>
        public List<StagingDealModel> GetStagingDeallist(int pageNo, int pageSize, string sortColumn, string sortDir, string DealNo, string CompanyName, DateTime? FromDate = null, DateTime? ToDate = null)
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

            List<StagingDealModel> stagingDeallist = _usersRepository.ExecuteStoredProcedureList<StagingDealModel>("usp_stagingdeal_List1", paraPageNo, paraPageSize, paraSortColumn, paraSortOrder, paraEmpId, paraDealNo, paraCompanyName, paraFromDate, paraToDate).ToList();
            return stagingDeallist;
        }

        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Get stagingDeal details list by stagingDealId
        /// </summary>
        /// <returns></returns>
        public StagingDealModel GetStagingDealDetailsByStagingDealId(long stagingDealId)
        {
            SqlParameter paraStagingDealId = new SqlParameter
            {
                ParameterName = "StagingDealId",
                DbType = DbType.Int64,
                Value = stagingDealId
            };

            StagingDealModel stagingDeallist = _usersRepository.ExecuteStoredProcedureList<StagingDealModel>("usp_StagingDeal_GetDetailByStagingDealId", paraStagingDealId).FirstOrDefault();
            return stagingDeallist;
        }

        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Import Staging Deal
        /// </summary>
        /// <param name="model"></param>
        /// <param name="EmpId"></param>
        /// <returns></returns>

        public int ImportStagingDeal(DataTable model)
        {
            SqlParameter empidStaging = new SqlParameter
            {
                ParameterName = "EmpId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };

            SqlParameter stagingdeals = new SqlParameter
            {
                ParameterName = "ImportStagingData",
                SqlDbType = SqlDbType.Structured,
                Value = model,
                TypeName = "dbo.ImportStagingDataCDTable"
            };

            IEnumerable<int> result = _usersRepository.ExecuteStoredProcedure<int>("[usp_StagingDeal_Import]", empidStaging, stagingdeals);
            int resultCount = result.FirstOrDefault();
            return resultCount;
        }



        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Save Staging deal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SaveStagingDeal(StagingDealModel model)
        {
            SqlParameter paraStagingDealId = new SqlParameter
            {
                ParameterName = "StagingDealId",
                DbType = DbType.Int64,
                Value = (object)model.StagingDealId ?? DBNull.Value
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

            int result = 0;
            if (model.StagingDealId > 0)
            {
                result = _usersRepository.ExecuteStoredProcedure<int>("usp_StagingDeal_update", paraStagingDealId, paraDealNo, paraContactName, paraTradeDate, paraValueDate, paraClientSoldAmt, paraClientSoldCCY, paraClientSoldGBP, paraClientBoughtAmt, paraClientBoughtCCY, paraGrossCommGBP, paraWLTotalCommGBP, paraTradeType, paraTStatus, paraUserId).FirstOrDefault();
            }
            return result;
        }


        /// <summary>
        /// By Mayank
        /// 22 March 2018
        /// Reload Staging deal
        /// </summary>
        /// <param name="StagingDealId"></param>
        /// <returns></returns>
        public bool ReloadStagingDeal(long StagingDealId)
        {
            SqlParameter paraStagingDealId = new SqlParameter
            {
                ParameterName = "StagingDealId",
                DbType = DbType.Int64,
                Value = StagingDealId
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_StagingDeal_Reload", paraStagingDealId).FirstOrDefault();
            return result > 0;
        }

        /// <summary>
        /// By Mayank
        /// 22 March 2018
        /// Proceed Staging deal
        /// </summary>
        /// <param name="StagingDealId"></param>
        /// <returns></returns>
        public bool ProceedStagingDeal(long StagingDealId)
        {
            SqlParameter paraStagingDealId = new SqlParameter
            {
                ParameterName = "StagingDealId",
                DbType = DbType.Int64,
                Value = StagingDealId
            };

            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };
            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_StagingDeal_Proceed", paraStagingDealId, paraUserId).FirstOrDefault();
            return result > 0;
        }


        /// <summary>
        /// By Mayank
        /// 23 March 2018
        /// Discard Staging deal
        /// </summary>
        /// <param name="StagingDealId"></param>
        /// <returns></returns>
        public bool DiscardStagingDeal(long StagingDealId)
        {
            SqlParameter paraStagingDealId = new SqlParameter
            {
                ParameterName = "StagingDealId",
                DbType = DbType.Int64,
                Value = StagingDealId
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_StagingDeal_Discard", paraStagingDealId).FirstOrDefault();
            return result > 0;
        }

        #endregion
    }
}
