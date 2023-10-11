using FXAdminTransferConnex.Data.Models;
using FXAdminTransferConnex.Data.Repository;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FXAdminTransferConnex.Entities;
using System.Data;
using System.Data.SqlClient;
using FXAdminTransferConnex.Common;

namespace FXAdminTransferConnex.Business.BulkEmail
{
    public class BulkEmailService : IBulkEmailService
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

        public BulkEmailService(IRepository<UserMaster> providerRepository)
        {
            _usersRepository = providerRepository;

        }

        /// <summary>
        /// By Vaibhav
        /// 10 Aug 2019
        /// Get all client details list
        /// </summary>
        /// <returns></returns>
        public List<ClientMasterModel> GetAllClientlist(int pageNo, int pageSize, string sortColumn, string sortDir, string FullName, string EmailAddress, string CompanyName, string AccountNo, string AwaitingAction = "2", string MarketOrder = "2", string SearchType = null)
        {
            try
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


                SqlParameter paraFullName = new SqlParameter
                {
                    ParameterName = "FullName",
                    DbType = DbType.String,
                    Value = (object)FullName ?? DBNull.Value
                };
                SqlParameter paraCompanyName = new SqlParameter
                {
                    ParameterName = "CompanyName",
                    DbType = DbType.String,
                    Value = (object)CompanyName ?? DBNull.Value
                };
                SqlParameter paraAccountNo = new SqlParameter
                {
                    ParameterName = "AccountNo",
                    DbType = DbType.String,
                    Value = (object)AccountNo ?? DBNull.Value
                };
                SqlParameter paraEmailAddress = new SqlParameter
                {
                    ParameterName = "EmailAddress",
                    DbType = DbType.String,
                    Value = (object)EmailAddress ?? DBNull.Value
                };

                SqlParameter paraAwaitingAction = new SqlParameter
                {
                    ParameterName = "AwaitingAction",
                    DbType = DbType.Int32,
                    Value = Convert.ToInt32(AwaitingAction)
                };

                SqlParameter paraMarketOrder = new SqlParameter
                {
                    ParameterName = "MarketOrder",
                    DbType = DbType.Int32,
                    Value = Convert.ToInt32(MarketOrder)
                };

                SqlParameter paraUserTypeAdmin = new SqlParameter
                {
                    ParameterName = "UserType",
                    DbType = DbType.Int32,
                    Value = Convert.ToInt32(1)
                };

                SqlParameter paraUserTypeTrader = new SqlParameter
                {
                    ParameterName = "UserType",
                    DbType = DbType.Int32,
                    Value = Convert.ToInt32(2)
                };

                SqlParameter paraUserTypeAffiliate = new SqlParameter
                {
                    ParameterName = "UserType",
                    DbType = DbType.Int32,
                    Value = Convert.ToInt32(3)
                };

                SqlParameter paraUserTypeSales_Person = new SqlParameter
                {
                    ParameterName = "UserType",
                    DbType = DbType.Int32,
                    Value = Convert.ToInt32(4)
                };
                //var paraUserTypeJuniorSales_Person = new SqlParameter
                //{
                //    ParameterName = "UserType",
                //    DbType = DbType.Int32,
                //    Value = Convert.ToInt32(4)
                //};

                //var clientlist = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("usp_ClientMaster_GetBulkEmailClientList", paraPageNo, paraPageSize, paraSortColumn, paraSortOrder, paraFullName, paraCompanyName, paraAccountNo, paraEmailAddress, paraTraderId, paraSalesPersonId, paraAwaitingAction, paraMarketOrder).ToList();
                //return clientlist;

                List<ClientMasterModel> clientlist = new List<ClientMasterModel>();
                switch (SearchType)
                {
                    case "Client":
                        clientlist = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("[usp_ClientMaster_GetBulkEmailClientList]", paraPageNo, paraPageSize, paraSortColumn, paraSortOrder, paraFullName, paraEmailAddress,paraCompanyName,paraAccountNo,paraAwaitingAction,paraMarketOrder).ToList();
                        break;
                    case "Admin":
                        clientlist = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("[usp_UserMaster_GetBulkEmailList]", paraPageNo, paraPageSize, paraSortColumn, paraSortOrder, paraFullName, paraEmailAddress, paraUserTypeAdmin).ToList();
                        break;
                    case "Trader":
                        clientlist = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("[usp_UserMaster_GetBulkEmailList]", paraPageNo, paraPageSize, paraSortColumn, paraSortOrder, paraFullName, paraEmailAddress, paraUserTypeTrader).ToList();
                        break;
                    case "Affiliate":
                        clientlist = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("[usp_UserMaster_GetBulkEmailList]", paraPageNo, paraPageSize, paraSortColumn, paraSortOrder, paraFullName, paraEmailAddress, paraUserTypeAffiliate).ToList();
                        break;
                    case "Sales_Person":
                        clientlist = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("[usp_UserMaster_GetBulkEmailList]", paraPageNo, paraPageSize, paraSortColumn, paraSortOrder, paraFullName, paraEmailAddress, paraUserTypeSales_Person).ToList();
                        break;
                    //case "Junior_Sales_Person":
                    //    clientlist = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("[usp_UserMaster_GetBulkEmailList]", paraPageNo, paraPageSize, paraSortColumn, paraSortOrder, paraFullName, paraEmailAddress, paraUserTypeJuniorSales_Person).ToList();
                    //    break;
                }

                return clientlist;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return new List<ClientMasterModel>();
        }

        /// <summary>
        /// By Vaibhav
        /// 12 Aug 2019
        /// Send Mail
        /// </summary>
        /// <param name="Emails"></param>
        /// <returns></returns>
        public bool SendMail(string Emails, string Subject, string Body)
        {
            try
            {
                return EmailNotification.SendAsyncEmail(null, Emails, null, Subject, Body, null);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }
        #endregion
    }
}
