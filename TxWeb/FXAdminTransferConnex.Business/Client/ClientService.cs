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

namespace FXAdminTransferConnex.Business.Client
{
    public class ClientService : IClientService
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

        public ClientService(IRepository<UserMaster> providerRepository)
        {
            _usersRepository = providerRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// By Mayank
        /// 03 March 2018
        /// Get client details list
        /// </summary>
        /// <returns></returns>
        public List<ClientMasterModel> GetClientlist(int pageNo, int pageSize, string sortColumn, string sortDir, string FullName, string CompanyName, string TraderName, string AccountNo, string EmailAddress, string CommunicationDetail, long? traderId, long? SalesPersonId, long? SearchJuniorSalesPersonId, string AwaitingAction, string MarketOrder, string BoughtCurrency, string SoldCurrency, long? LeadCategoryId, long? SectorCategoryId, long? BusinessCategoryId, string ClientSource)
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
                SqlParameter paraTraderName = new SqlParameter
                {
                    ParameterName = "TraderName",
                    DbType = DbType.String,
                    Value = (object)TraderName ?? DBNull.Value
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
                    Value = (object)SearchJuniorSalesPersonId ?? DBNull.Value
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

                SqlParameter paraCommunicationDetail = new SqlParameter
                {
                    ParameterName = "CommunicationDetail",
                    DbType = DbType.String,
                    Value = (object)CommunicationDetail ?? DBNull.Value
                };
                SqlParameter paracurrencyBought = new SqlParameter
                {
                    ParameterName = "CurrencyBought",
                    DbType = DbType.String,
                    Value = (object)BoughtCurrency ?? DBNull.Value
                };
                SqlParameter paracurrencySold = new SqlParameter
                {
                    ParameterName = "CurrencySold",
                    DbType = DbType.String,
                    Value = (object)SoldCurrency ?? DBNull.Value
                };
                SqlParameter paraLeadCategoryId = new SqlParameter
                {
                    ParameterName = "LeadCategoryId",
                    DbType = DbType.Int64,
                    Value = (object)LeadCategoryId ?? DBNull.Value
                };
                SqlParameter paraSectorCategoryId = new SqlParameter
                {
                    ParameterName = "SectorCategoryId",
                    DbType = DbType.Int64,
                    Value = (object)SectorCategoryId ?? DBNull.Value
                };
                SqlParameter paraBusinessCategoryId = new SqlParameter
                {
                    ParameterName = "BusinessCategoryId",
                    DbType = DbType.Int64,
                    Value = (object)BusinessCategoryId ?? DBNull.Value
                };
                SqlParameter paraClientSource = new SqlParameter
                {
                    ParameterName = "ClientSource",
                    DbType = DbType.String,
                    Value = (object)ClientSource ?? DBNull.Value
                };

                List<ClientMasterModel> clientlist = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("usp_ClientMaster_GetClientList", paraPageNo, paraPageSize, paraSortColumn, paraSortOrder, paraFullName, paraCompanyName, paraTraderName, paraAccountNo, paraEmailAddress, paraTraderId, paraSalesPersonId, paraAwaitingAction, paraMarketOrder, paraCommunicationDetail, paracurrencyBought, paracurrencySold, paraLeadCategoryId, paraSectorCategoryId, paraBusinessCategoryId, paraClientSource, paraJuniorSalesPersonId).ToList();
                return clientlist;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return new List<ClientMasterModel>();
        }
        //Currency Details
        public CurrencyModel GetCurrencyDetailById(int CCYId)
        {
            SqlParameter paraCCYId = new SqlParameter
            {
                ParameterName = "CCYId",
                DbType = DbType.Int32,
                Value = CCYId
            };

            CurrencyModel result = _usersRepository.ExecuteStoredProcedureList<CurrencyModel>("usp_GetCurrencyDetailById", paraCCYId).FirstOrDefault();
            return result;

        }


        public LeadCategoryModel GetLeadCategoryDetailById(long LeadId)
        {
            SqlParameter paraLeadId = new SqlParameter
            {
                ParameterName = "LeadId",
                DbType = DbType.Int64,
                Value = LeadId
            };
            LeadCategoryModel result = _usersRepository.ExecuteStoredProcedureList<LeadCategoryModel>("usp_GetLeadCategoryDetailById", paraLeadId).FirstOrDefault();
            return result;
        }



        /// <summary>
        /// By Mayank
        /// 12 March 2018
        /// Get client details list by clientId
        /// </summary>
        /// <returns></returns>
        public ClientMasterModel GetClientDetailsByClientId(long clientId)
        {
            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = clientId
            };

            ClientMasterModel userList = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("usp_ClientMaster_GetClientByClientId", paraClientId).FirstOrDefault();
            return userList;
        }

        public CategoryModel GetBusinessCategoryByCategoryId(long? categoryId)
        {
            SqlParameter paraCategoryId = new SqlParameter
            {
                ParameterName = "CategoryId",
                DbType = DbType.Int64,
                Value = categoryId
            };

            CategoryModel userList = _usersRepository.ExecuteStoredProcedureList<CategoryModel>("usp_GetCategoryByCategoryId", paraCategoryId).FirstOrDefault();
            return userList;
        }

        /// <summary>
        /// By Mayank
        /// 15 FEB 2017
        /// Add/Edit user details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long AddClient(ClientMasterModel model)
        {
            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = (object)model.ClientId ?? DBNull.Value
            };

            SqlParameter paraFullName = new SqlParameter
            {
                ParameterName = "FullName",
                DbType = DbType.String,
                Value = (object)model.FullName ?? DBNull.Value
            };

            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = (object)model.CompanyName ?? DBNull.Value
            };

            SqlParameter paraAddressLine1 = new SqlParameter
            {
                ParameterName = "AddressLine1",
                DbType = DbType.String,
                Value = (object)model.AddressLine1 ?? DBNull.Value
            };

            SqlParameter paraAddressLine2 = new SqlParameter
            {
                ParameterName = "AddressLine2",
                DbType = DbType.String,
                Value = (object)model.AddressLine2 ?? DBNull.Value
            };

            SqlParameter paraCity_Town = new SqlParameter
            {
                ParameterName = "City_Town",
                DbType = DbType.String,
                Value = (object)model.City_Town ?? DBNull.Value
            };

            SqlParameter paraState_County = new SqlParameter
            {
                ParameterName = "State_County",
                DbType = DbType.String,
                Value = (object)model.State_County ?? DBNull.Value
            };

            SqlParameter paraCountry = new SqlParameter
            {
                ParameterName = "Country",
                DbType = DbType.String,
                Value = (object)model.Country ?? DBNull.Value
            };

            SqlParameter paraMobileNo = new SqlParameter
            {
                ParameterName = "MobileNo",
                DbType = DbType.String,
                Value = (object)model.MobileNo ?? DBNull.Value
            };

            SqlParameter paraAltMobileNo = new SqlParameter
            {
                ParameterName = "AltMobileNo",
                DbType = DbType.String,
                Value = (object)model.AltMobileNo ?? DBNull.Value
            };

            SqlParameter paraEmailAddress = new SqlParameter
            {
                ParameterName = "EmailAddress",
                DbType = DbType.String,
                Value = (object)model.EmailAddress ?? DBNull.Value
            };

            SqlParameter paraAccountNo = new SqlParameter
            {
                ParameterName = "AccountNo",
                DbType = DbType.String,
                Value = (object)model.AccountNo ?? DBNull.Value
            };

            SqlParameter paraRegiterDate = new SqlParameter
            {
                ParameterName = "RegiterDate",
                DbType = DbType.DateTime,
                Value = (object)model.RegiterDate ?? DBNull.Value
            };

            SqlParameter paraDefaultMargin = new SqlParameter
            {
                ParameterName = "DefaultMargin",
                DbType = DbType.String,
                Value = (object)model.DefaultMargin ?? DBNull.Value
            };

            SqlParameter paraCurrencies = new SqlParameter
            {
                ParameterName = "Currencies",
                DbType = DbType.String,
                Value = (object)model.Currencies ?? DBNull.Value
            };

            SqlParameter paraSalesPersonId = new SqlParameter
            {
                ParameterName = "SalesPersonId",
                DbType = DbType.Int64,
                Value = (object)model.SalesPersonId ?? DBNull.Value
            };
            SqlParameter paraJuniorSalesPersonId = new SqlParameter
            {
                ParameterName = "JuniorSalesPersonId",
                DbType = DbType.Int64,
                Value = (object)model.JuniorSalesPersonId ?? DBNull.Value
            };

            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)model.TraderId ?? DBNull.Value
            };

            SqlParameter paraAffiliateId = new SqlParameter
            {
                ParameterName = "AffiliateId",
                DbType = DbType.Int64,
                Value = (object)model.AffiliateId ?? DBNull.Value
            };


            SqlParameter paraPastCommDetail = new SqlParameter
            {
                ParameterName = "PastCommDetail",
                DbType = DbType.String,
                Value = (object)model.PastCommDetail ?? DBNull.Value
            };

            SqlParameter paraIsActive = new SqlParameter
            {
                ParameterName = "IsActive",
                DbType = DbType.Int16,
                Value = (object)model.IsActive ?? DBNull.Value
            };

            SqlParameter paraAwaitingAction = new SqlParameter
            {
                ParameterName = "AwaitingAction",
                DbType = DbType.String,
                Value = (object)model.AwaitingAction ?? DBNull.Value
            };

            SqlParameter paraIsAwaitingAction = new SqlParameter
            {
                ParameterName = "IsAwaitingAction",
                DbType = DbType.Int16,
                Value = (object)model.IsAwaitingAction ?? DBNull.Value
            };

            SqlParameter paraMarketOrder = new SqlParameter
            {
                ParameterName = "MarketOrder",
                DbType = DbType.String,
                Value = (object)model.MarketOrder ?? DBNull.Value
            };

            SqlParameter paraIsMarketOrder = new SqlParameter
            {
                ParameterName = "IsMarketOrder",
                DbType = DbType.Int16,
                Value = (object)model.IsMarketOrder ?? DBNull.Value
            };

            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };
            //var paraSource = new SqlParameter
            //{
            //    ParameterName = "Source",
            //    DbType = DbType.String,
            //    Value = (object)model.Source ?? DBNull.Value
            //};
            SqlParameter paraMarginPosted = new SqlParameter
            {
                ParameterName = "MarginPosted",
                DbType = DbType.Decimal,
                Value = (object)model.MarginPosted ?? DBNull.Value
            };

            SqlParameter paraBoughtCurrencies = new SqlParameter
            {
                ParameterName = "BoughtCurrencies",
                DbType = DbType.String,
                Value = (object)model.BoughtCurrencies ?? DBNull.Value
            };

            SqlParameter paraSoldCurrencies = new SqlParameter
            {
                ParameterName = "SoldCurrencies",
                DbType = DbType.String,
                Value = (object)model.SoldCurrencies ?? DBNull.Value
            };

            SqlParameter paraLeadCategoryId = new SqlParameter
            {
                ParameterName = "LeadCategoryId",
                DbType = DbType.Int64,
                Value = (object)model.LeadCategoryId ?? DBNull.Value
            };

            SqlParameter paraCategoryId = new SqlParameter
            {
                ParameterName = "CategoryId",
                DbType = DbType.Int64,
                Value = (object)model.CategoryId ?? DBNull.Value
            };
            SqlParameter paraSectorId = new SqlParameter
            {
                ParameterName = "SectorId",
                DbType = DbType.Int64,
                Value = (object)model.SectorId ?? DBNull.Value
            };
            SqlParameter paraClientSource = new SqlParameter
            {
                ParameterName = "ClientSource",
                DbType = DbType.String,
                Value = (object)model.ClientSource ?? DBNull.Value
            };
            SqlParameter paraGcPartnerClientId = new SqlParameter
            {
                ParameterName = "GcPartnerClientId",
                DbType = DbType.String,
                Value = (object)model.GcPartnerClientId ?? DBNull.Value
            };
            SqlParameter paraScioPayAccountId = new SqlParameter
            {
                ParameterName = "ScioPayAccountId",
                DbType = DbType.String,
                Value = (object)model.ScioPayAccountId ?? DBNull.Value
            };
            long result;
            if (model.ClientId > 0)
            {
                result = _usersRepository.ExecuteStoredProcedure<long>("usp_ClientMaster_UpdateClient", paraClientId, paraFullName, paraCompanyName, paraAddressLine1, paraAddressLine2, paraCity_Town, paraState_County, paraCountry, paraMobileNo, paraAltMobileNo, paraEmailAddress, paraAccountNo, paraRegiterDate, paraDefaultMargin, paraCurrencies, paraSalesPersonId, paraTraderId, paraAffiliateId, paraPastCommDetail, paraIsActive, paraAwaitingAction, paraIsAwaitingAction, paraMarketOrder, paraIsMarketOrder, paraUserId, paraMarginPosted, paraBoughtCurrencies, paraSoldCurrencies, paraLeadCategoryId, paraCategoryId, paraSectorId, paraClientSource, paraGcPartnerClientId, paraScioPayAccountId, paraJuniorSalesPersonId).FirstOrDefault();
            }
            else
            {
                result = _usersRepository.ExecuteStoredProcedure<long>("usp_ClientMaster_InsertClient", paraFullName, paraCompanyName, paraAddressLine1, paraAddressLine2, paraCity_Town, paraState_County, paraCountry, paraMobileNo, paraAltMobileNo, paraEmailAddress, paraAccountNo, paraRegiterDate, paraDefaultMargin, paraCurrencies, paraSalesPersonId, paraTraderId, paraAffiliateId, paraPastCommDetail, paraIsActive, paraUserId, paraMarginPosted, paraBoughtCurrencies, paraSoldCurrencies, paraLeadCategoryId, paraCategoryId, paraSectorId, paraClientSource, paraGcPartnerClientId, paraScioPayAccountId, paraJuniorSalesPersonId).FirstOrDefault();
            }

            return result;
        }

        /// <summary>
        /// By Mayank
        /// 15 FEB 2017
        /// Save Awaiting action details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SaveAwaitingAction(ClientMasterModel model)
        {
            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = (object)model.ClientId ?? DBNull.Value
            };

            SqlParameter paraPastCommDetail = new SqlParameter
            {
                ParameterName = "PastCommDetail",
                DbType = DbType.String,
                Value = (object)model.PastCommDetail ?? DBNull.Value
            };

            SqlParameter paraAwaitingAction = new SqlParameter
            {
                ParameterName = "AwaitingAction",
                DbType = DbType.String,
                Value = (object)model.AwaitingAction ?? DBNull.Value
            };

            SqlParameter paraIsAwaitingAction = new SqlParameter
            {
                ParameterName = "IsAwaitingAction",
                DbType = DbType.Int16,
                Value = (object)model.IsAwaitingAction ?? DBNull.Value
            };

            SqlParameter paraMarketOrder = new SqlParameter
            {
                ParameterName = "MarketOrder",
                DbType = DbType.String,
                Value = (object)model.MarketOrder ?? DBNull.Value
            };

            SqlParameter paraIsMarketOrder = new SqlParameter
            {
                ParameterName = "IsMarketOrder",
                DbType = DbType.Int16,
                Value = (object)model.IsMarketOrder ?? DBNull.Value
            };

            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };

            SqlParameter paraDefaultMargin = new SqlParameter
            {
                ParameterName = "DefaultMargin",
                DbType = DbType.String,
                Value = (object)model.DefaultMargin ?? DBNull.Value
            };

            SqlParameter paraMobileNo = new SqlParameter
            {
                ParameterName = "MobileNo",
                DbType = DbType.String,
                Value = (object)model.MobileNo ?? DBNull.Value
            };

            SqlParameter paraAltMobileNo = new SqlParameter
            {
                ParameterName = "AltMobileNo",
                DbType = DbType.String,
                Value = (object)model.AltMobileNo ?? DBNull.Value
            };

            SqlParameter paraEmailAddress = new SqlParameter
            {
                ParameterName = "EmailAddress",
                DbType = DbType.String,
                Value = (object)model.EmailAddress ?? DBNull.Value
            };

            SqlParameter paraCurrencies = new SqlParameter
            {
                ParameterName = "Currencies",
                DbType = DbType.String,
                Value = (object)model.Currencies ?? DBNull.Value
            };

            // --
            SqlParameter paraBoughtCurrencies = new SqlParameter
            {
                ParameterName = "BoughtCurrencies",
                DbType = DbType.String,
                Value = (object)model.BoughtCurrencies ?? DBNull.Value
            };

            SqlParameter paraSoldCurrencies = new SqlParameter
            {
                ParameterName = "SoldCurrencies",
                DbType = DbType.String,
                Value = (object)model.SoldCurrencies ?? DBNull.Value
            };
           
            int result = 0;
            if (model.ClientId > 0)
            {
                result = _usersRepository.ExecuteStoredProcedure<int>("usp_ClientMaster_UpdateAwaitingAction",
                    paraClientId, paraPastCommDetail, paraAwaitingAction, paraIsAwaitingAction, paraMarketOrder,
                    paraIsMarketOrder, paraUserId, paraDefaultMargin, paraMobileNo, paraAltMobileNo, paraEmailAddress, paraCurrencies, paraBoughtCurrencies, paraSoldCurrencies).FirstOrDefault(); //-- 
            }

            return result;
        }


        /// <summary>
        /// By Mayank
        /// 16 March 2018
        /// Delete details
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public int DeleteClient(long clientId)
        {
            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = (object)clientId ?? DBNull.Value
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_ClientMaster_DeleteClient", paraClientId).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// By Mayank
        /// 13 Aug 2019
        /// Change Marketing Order active status
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool ChangeMarketOrderStatus(long clientId, bool status)
        {
            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = (object)clientId ?? DBNull.Value
            };

            SqlParameter action = new SqlParameter
            {
                ParameterName = "Status",
                DbType = DbType.Boolean,
                Value = status
            };

            IEnumerable<int> result = _usersRepository.ExecuteStoredProcedure<int>("usp_ClientMaster_MarketOrderStatus", paraClientId, action);
            int rowCount = result.FirstOrDefault();

            return (rowCount == 1);
        }

        /// <summary>
        /// By Mayank
        /// 27 March 2019
        /// Get client deal details list
        /// </summary>
        /// <returns></returns>
        public List<DealModel> GetClientDeallist(long clientId)
        {
            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = clientId
            };

            List<DealModel> clientdeallist = _usersRepository.ExecuteStoredProcedureList<DealModel>("usp_Deal_ClientDeal_List", paraClientId).ToList();
            return clientdeallist;
        }

        /// <summary>
        /// By Mayank
        /// 16 July 2019
        /// Get client deal details list
        /// </summary>
        /// <returns></returns>
        public List<TaskReminderModel> GetTaskReminderlist(long clientId)
        {
            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = clientId
            };
            List<TaskReminderModel> clientdeallist = _usersRepository.ExecuteStoredProcedureList<TaskReminderModel>("usp_TaskReminder_GetTaskReminderList", paraClientId).ToList();
            return clientdeallist;
        }

        public int SaveTaskReminder(TaskReminderModel model)
        {
            SqlParameter paraTaskId = new SqlParameter
            {
                ParameterName = "TaskId",
                DbType = DbType.Int64,
                Value = (object)model.TaskId ?? DBNull.Value
            };

            SqlParameter paraSubject = new SqlParameter
            {
                ParameterName = "Subject",
                DbType = DbType.String,
                Value = (object)model.Subject ?? DBNull.Value
            };

            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = (object)model.ClientId ?? DBNull.Value
            };

            SqlParameter paraProspectId = new SqlParameter
            {
                ParameterName = "ProspectId",
                DbType = DbType.Int64,
                Value = (object)model.ProspectId ?? DBNull.Value
            };

            SqlParameter paraAssignToId = new SqlParameter
            {
                ParameterName = "AssignToId",
                DbType = DbType.Int64,
                Value = (object)model.AssignToId ?? DBNull.Value
            };

            SqlParameter paraTaskTypeId = new SqlParameter
            {
                ParameterName = "TaskTypeId",
                DbType = DbType.Int32,
                Value = (object)model.TaskTypeId ?? DBNull.Value
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

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_TaskReminder_Save", paraTaskId, paraSubject, paraClientId, paraProspectId, paraAssignToId, paraTaskTypeId, paraTaskReminderDatetime, paraNotes, paraUserId).FirstOrDefault();
            return result;
        }

        public bool DeleteSaveTaskReminder(long TaskId)
        {
            SqlParameter paraTaskId = new SqlParameter
            {
                ParameterName = "TaskId",
                DbType = DbType.Int64,
                Value = (object)TaskId ?? DBNull.Value
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_TaskReminder_Delete", paraTaskId).FirstOrDefault();
            return result > 0;
        }
        #endregion

        public MobileStatus GetMobileStatus(string MobileNo)
        {
            SqlParameter paraTaskId = new SqlParameter
            {
                ParameterName = "NUMBER",
                DbType = DbType.String,
                Value = (object)MobileNo ?? DBNull.Value
            };

            MobileStatus result = _usersRepository.ExecuteStoredProcedure<MobileStatus>("USP_GetDNDnTPSStstus", paraTaskId).FirstOrDefault();
            return result;
        }

        public void UpdateMobileStatus(string MobileNo, bool tpsResult)
        {
            SqlParameter paraNumber = new SqlParameter
            {
                ParameterName = "NUMBER",
                DbType = DbType.String,
                Value = (object)MobileNo ?? DBNull.Value
            };

            SqlParameter paraStatus = new SqlParameter
            {
                ParameterName = "Status",
                DbType = DbType.String,
                Value = (object)tpsResult ?? DBNull.Value
            };

            List<MobileStatus> result = _usersRepository.ExecuteStoredProcedure<MobileStatus>("USP_UpdateTPSStstus", paraNumber, paraStatus).ToList();
            
        }

        public DNDNumbers GetDNDNumberById(long id)
        {
            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "DNDNumberID",
                DbType = DbType.Int64,
                Value = id
            };

            DNDNumbers userList = _usersRepository.ExecuteStoredProcedureList<DNDNumbers>("usp_DNDNumbers_GetById", paraClientId).FirstOrDefault();
            return userList;
        }

        public List<DNDNumbers> GetDNDNumberlist(string fullName, string mobileNo)
        {
            SqlParameter parafullName = new SqlParameter
            {
                ParameterName = "FullName",
                DbType = DbType.String,
                Value = (object)fullName ?? DBNull.Value
            };

            SqlParameter paramobileNo = new SqlParameter
            {
                ParameterName = "MobileNo",
                DbType = DbType.String,
                Value = (object)mobileNo ?? DBNull.Value
            };

            List<DNDNumbers> result = _usersRepository.ExecuteStoredProcedure<DNDNumbers>("usp_DNDNumbers_GetList", parafullName, paramobileNo).ToList();
            return result;
        }

        public int DeleteDNDNumber(long dNDNumberID)
        {
            SqlParameter paraDNDNumberID = new SqlParameter
            {
                ParameterName = "DNDNumberID",
                DbType = DbType.Int64,
                Value = (object)dNDNumberID ?? DBNull.Value
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_DNDNumbers_DeleteById", paraDNDNumberID).FirstOrDefault();
            return result;
        }

        public int SaveDNDNumber(DNDNumbers model)
        {
            SqlParameter paraDNDNumberID = new SqlParameter
            {
                ParameterName = "DNDNumberID",
                DbType = DbType.Int64,
                Value = (object)model.DNDNumberID ?? DBNull.Value
            };

            SqlParameter paraFullName = new SqlParameter
            {
                ParameterName = "FullName",
                DbType = DbType.String,
                Value = (object)model.FullName ?? DBNull.Value
            };

            SqlParameter paraMobileNo = new SqlParameter
            {
                ParameterName = "MobileNo",
                DbType = DbType.String,
                Value = (object)model.MobileNo ?? DBNull.Value
            };

            SqlParameter paraComments = new SqlParameter
            {
                ParameterName = "Comments",
                DbType = DbType.String,
                Value = (object)model.Comments ?? DBNull.Value
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_DNDNumbers_AddUpdate", paraDNDNumberID, paraFullName, paraMobileNo, paraComments).FirstOrDefault();
            return result;
        }

        #region Market Order Setting
        //MarketOrderNotificationFilter
        /// <summary>
        /// By Krupesh
        /// 14 April 2020
        /// GetMarketOrderNotificationFilter
        /// </summary>
        /// <returns></returns>
        public List<NotificationSettingModel> GetMarketOrderNotificationFilter()
        {
            List<NotificationSettingModel> result = _usersRepository.ExecuteStoredProcedureList<NotificationSettingModel>("usp_NotificationFilter_GetList").ToList();
            return result;
        }

        public List<ConditionalOperatorModel> GetConditionalOperatorList()
        {
            List<ConditionalOperatorModel> result = _usersRepository.ExecuteStoredProcedureList<ConditionalOperatorModel>("usp_ConditionalOperator_GetList").ToList();
            return result;
        }

        public long SaveMarketOrderSetting(MarketOrderSettingModel model)
        {
            SqlParameter paraMarketOrderId = new SqlParameter
            {
                ParameterName = "MarketOrderId",
                DbType = DbType.Int64,
                Value = (object)model.MarketOrderId ?? DBNull.Value
            };

            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = (object)model.ClientId ?? DBNull.Value
            };

            SqlParameter paraProspectId = new SqlParameter
            {
                ParameterName = "ProspectId",
                DbType = DbType.Int64,
                Value = (object)model.ProspectId ?? DBNull.Value
            };

            SqlParameter paraFromCurrency = new SqlParameter
            {
                ParameterName = "FromCurrency",
                DbType = DbType.Int32,
                Value = (object)model.FromCurrency ?? DBNull.Value
            };

            SqlParameter paraToCurrency = new SqlParameter
            {
                ParameterName = "ToCurrency",
                DbType = DbType.Int32,
                Value = (object)model.ToCurrency ?? DBNull.Value
            };

            SqlParameter paraFilterId = new SqlParameter
            {
                ParameterName = "FilterId",
                DbType = DbType.Int32,
                Value = (object)model.FilterId ?? DBNull.Value
            };

            SqlParameter paraStartDate = new SqlParameter
            {
                ParameterName = "StartDate",
                DbType = DbType.DateTime,
                Value = (object)model.StartDate ?? DBNull.Value
            };

            SqlParameter paraEndDate = new SqlParameter
            {
                ParameterName = "EndDate",
                DbType = DbType.DateTime,
                Value = (object)model.EndDate ?? DBNull.Value
            };

            SqlParameter paraConditionId = new SqlParameter
            {
                ParameterName = "ConditionId",
                DbType = DbType.Int32,
                Value = (object)model.ConditionId ?? DBNull.Value
            };
            SqlParameter paraMarketRate = new SqlParameter
            {
                ParameterName = "MarketRate",
                DbType = DbType.Decimal,
                Value = (object)model.MarketRate ?? DBNull.Value
            };

            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = (object)ProjectSession.LoginUserDetails.UserId ?? DBNull.Value
            };

            SqlParameter paraClientRate = new SqlParameter
            {
                ParameterName = "ClientRate",
                DbType = DbType.Decimal,
                Value = (object)model.ClientRate ?? DBNull.Value
            };

            SqlParameter paraAmount = new SqlParameter
            {
                ParameterName = "Amount",
                DbType = DbType.Decimal,
                Value = (object)model.Amount ?? DBNull.Value
            };
            SqlParameter paraComments = new SqlParameter
            {
                ParameterName = "Comments",
                DbType = DbType.String,
                Value = (object)model.Comments ?? DBNull.Value
            };

            long result = _usersRepository.ExecuteStoredProcedure<long>("usp_MarketOrderSetting_Save",
                paraMarketOrderId, paraClientId, paraProspectId, paraFromCurrency, paraToCurrency, paraFilterId, paraStartDate,
                paraEndDate, paraConditionId, paraMarketRate,
                paraUserId, paraClientRate, paraAmount, paraComments).FirstOrDefault();
            return result;
        }

        public List<MarketOrderSettingModel> GetMarketOrderSettingList(string FullName, string EmailAddress, string CompanyName, string MarketOrderStatus, int? FromCurrency = 0, int? ToCurrency = 0, long ClientId = 0, long ProspectId = 0, long TraderId = 0, long SalesPersonId = 0)
        {
            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = ClientId
            };
            SqlParameter paraProspectId = new SqlParameter
            {
                ParameterName = "ProspectId",
                DbType = DbType.Int64,
                Value = ProspectId
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
            SqlParameter paraEmailAddress = new SqlParameter
            {
                ParameterName = "EmailAddress",
                DbType = DbType.String,
                Value = (object)EmailAddress ?? DBNull.Value
            };
            SqlParameter paraStatus = new SqlParameter
            {
                ParameterName = "Status",
                DbType = DbType.String,
                Value = (object)MarketOrderStatus ?? DBNull.Value
            };
            SqlParameter paraFromCurrency = new SqlParameter
            {
                ParameterName = "FromCurrency",
                DbType = DbType.Int32,
                Value = (object)FromCurrency ?? DBNull.Value
            };
            SqlParameter paraToCurrency = new SqlParameter
            {
                ParameterName = "ToCurrency",
                DbType = DbType.Int32,
                Value = (object)ToCurrency ?? DBNull.Value
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
            List<MarketOrderSettingModel> marketOrderSettingList = _usersRepository.ExecuteStoredProcedureList<MarketOrderSettingModel>("usp_MarketOrderSettingList",
                paraClientId, paraProspectId, paraFullName, paraCompanyName, paraEmailAddress, paraStatus, paraFromCurrency, paraToCurrency, paraTraderId, paraSalesPersonId).ToList();
            return marketOrderSettingList;
        }

        public bool DeleteMarketOrderSetting(long MarketOrderId)
        {
            SqlParameter paraMarketOrderId = new SqlParameter
            {
                ParameterName = "MarketOrderId",
                DbType = DbType.Int64,
                Value = (object)MarketOrderId ?? DBNull.Value
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_MarketOrderSetting_Delete", paraMarketOrderId).FirstOrDefault();
            return result > 0;
        }

        public bool UpdateMarketOrderSettingStatus(int ID, string Status)
        {
            SqlParameter paraMarketOrderId = new SqlParameter
            {
                ParameterName = "MarketOrderId",
                DbType = DbType.Int64,
                Value = (object)ID ?? DBNull.Value
            };

            SqlParameter paraStatus = new SqlParameter
            {
                ParameterName = "Status",
                DbType = DbType.String,
                Value = (object)Status ?? DBNull.Value
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_MarketValueNotification_UpdateStatus", paraMarketOrderId, paraStatus).FirstOrDefault();
            return result > 0;
        }

        public long SaveMarketOrderNotification(long clientId, long marketOrderId)
        {
            SqlParameter paraClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = (object)clientId ?? DBNull.Value
            };

            SqlParameter paraMarketOrderId = new SqlParameter
            {
                ParameterName = "MarketOrderId",
                DbType = DbType.Int64,
                Value = (object)marketOrderId ?? DBNull.Value
            };

            long result = _usersRepository.ExecuteStoredProcedure<long>("usp_MarketValueNotification_Insert",
                paraClientId, paraMarketOrderId).FirstOrDefault();
            return result;
        }

        #endregion
        #region Ring Dial

        public RingCentralClientModel GetClientDetailByMobile(string MobileNo)
        {
            SqlParameter paraMobileNo = new SqlParameter
            {
                ParameterName = "MobileNo",
                DbType = DbType.String,
                Value = MobileNo
            };

            return _usersRepository.ExecuteStoredProcedureList<RingCentralClientModel>("usp_GetClientByRingCentralMobile", paraMobileNo).FirstOrDefault();
        }

        public int ValidateClientByRingCentralMobileAndSalesPerson(string MobileNo, long SalesPersonId)
        {
            SqlParameter paraMobileNo = new SqlParameter
            {
                ParameterName = "MobileNo",
                DbType = DbType.String,
                Value = MobileNo
            };

            SqlParameter paraSalesPersonId = new SqlParameter
            {
                ParameterName = "SalesPersonId",
                DbType = DbType.Int64,
                Value = SalesPersonId
            };

            return _usersRepository.ExecuteStoredProcedure<int>("SP_ValidateClientByRingCentralMobileAndSalesPerson", paraMobileNo, paraSalesPersonId).FirstOrDefault();
        }

        public bool UpdatePastCommDetail(long clientId, string PastCommDetail)
        {
            SqlParameter paraclientId = new SqlParameter
            {
                ParameterName = "clientId",
                DbType = DbType.Int64,
                Value = clientId
            };

            SqlParameter paraPastCommDetail = new SqlParameter
            {
                ParameterName = "PastCommDetail",
                DbType = DbType.String,
                Value = PastCommDetail
            };

            string result = _usersRepository.ExecuteStoredProcedure<string>("usp_ClientMaster_UpdatePastCommDetail", paraclientId, paraPastCommDetail).FirstOrDefault();
            if (string.IsNullOrEmpty(result))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        public SectorModel GetBusinessCategorySectorbySectorID(long? sectorid)
        {
            SqlParameter parasectorid = new SqlParameter
            {
                ParameterName = "SectorId",
                DbType = DbType.Int64,
                Value = sectorid
            };

            SectorModel userList = _usersRepository.ExecuteStoredProcedureList<SectorModel>("usp_GetBusinessCategorySectorbySectorID", parasectorid).FirstOrDefault();
            return userList;
        }
        public string SaveUploadScioPayClient(DataTable table, ClientMasterModel model)
        {
            SqlParameter paramSource = new SqlParameter
            {
                ParameterName = "Source",
                DbType = DbType.String,
                Value = model.Source
            };
            SqlParameter paramCreatedBy = new SqlParameter
            {
                ParameterName = "CreatedBy",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };
            SqlParameter paramImportScioPayTable = new SqlParameter
            {
                ParameterName = "ImportScioPayClientTbl",
                SqlDbType = SqlDbType.Structured,
                Value = table,
                TypeName = "dbo.ImportScioPayClientTableTemp"
            };

            string result = _usersRepository.ExecuteStoredProcedure<string>("usp_SaveUploadClient_ScioPay_temp",
                 paramSource, paramCreatedBy, paramImportScioPayTable).FirstOrDefault();

            return result;
        }

        public string SaveUploadGCPartnerClient(DataTable table, ClientMasterModel model)
        {
            SqlParameter paramSource = new SqlParameter
            {
                ParameterName = "Source",
                DbType = DbType.String,
                Value = model.Source
            };
            SqlParameter paramCreatedBy = new SqlParameter
            {
                ParameterName = "CreatedBy",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };
            SqlParameter paramImportGCPartnerTable = new SqlParameter
            {
                ParameterName = "ImportGCPartnerClientTbl",
                SqlDbType = SqlDbType.Structured,
                Value = table,
                TypeName = "dbo.ImportGCPartnerClientTable"
            };

            string result = _usersRepository.ExecuteStoredProcedure<string>("usp_SaveUploadClient_GCPartner",
                 paramSource, paramCreatedBy, paramImportGCPartnerTable).FirstOrDefault();

            return result;
        }

        public long GetSalespersonIdByName(string SalesPersonName)
        {
            SqlParameter ParaSalesPersonName = new SqlParameter
            {
                ParameterName = "SalesPersonName",
                DbType = DbType.String,
                Value = SalesPersonName
            };
            long result = _usersRepository.ExecuteStoredProcedure<long>("usp_SalesPersonByIdByName", ParaSalesPersonName).FirstOrDefault();
            return result;
        }
        public long GetTraderIdByName(string TraderName)
        {
            SqlParameter ParaTraderName = new SqlParameter
            {
                ParameterName = "TraderName",
                DbType = DbType.String,
                Value = TraderName
            };
            long result = _usersRepository.ExecuteStoredProcedure<long>("usp_TraderIdByName", ParaTraderName).FirstOrDefault();
            return result;
        }
    }
}
