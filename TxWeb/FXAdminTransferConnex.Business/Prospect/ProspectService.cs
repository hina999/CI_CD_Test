using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using FXAdminTransferConnex.Data.Repository;
using FXAdminTransferConnex.Entities;
using log4net;

namespace FXAdminTransferConnex.Business.Prospect
{
    public class ProspectService : IProspectService
    {

        #region Fields

        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<ProspectModel> _usersRepository;

        #endregion

        #region ctor

        public ProspectService(IRepository<ProspectModel> providerRepository)
        {
            _usersRepository = providerRepository;
        }

        #endregion

        public int DeleteProspect(long ProspectId)
        {
            SqlParameter paraProspectId = new SqlParameter
            {
                ParameterName = "ProspectId",
                DbType = DbType.Int64,
                Value = (object)ProspectId ?? DBNull.Value
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_ProspectMaster_DeleteById", paraProspectId).FirstOrDefault();
            return result;
        }

        public bool DeleteTaskReminder(long TaskId)
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

        public ProspectModel GetProspectById(long ProspectId)
        {
            SqlParameter paraProspectId = new SqlParameter
            {
                ParameterName = "ProspectId",
                DbType = DbType.Int64,
                Value = ProspectId
            };

            ProspectModel userList = _usersRepository.ExecuteStoredProcedureList<ProspectModel>("usp_ProspectMaster_GetById", paraProspectId).FirstOrDefault();
            return userList;
        }

        public List<ClientMasterModel> GetClientByCompany(string CompanyName)
        {
            SqlParameter paraCompanyName = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = (object)CompanyName ?? DBNull.Value
            };
            List<ClientMasterModel> client = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("usp_ClientDetails_ByCompanyName", paraCompanyName).ToList();
            return client;
        }

        public List<ProspectModel> GetProspectlist(int pageNo, int pageSize, string fullName, string companyName, string mobileNo, string email, long? SalesPersonId, long? JuniorSalesPersonId, long? TraderId, long? loggedinUserId, int UserTypeId, long? LoggedinTraderId, string boughtCurrency, string soldCurrency, string leadCategory, string CommunicationDetail, long? sectorCategoryId, long? businessCategoryId)
        {
            SqlParameter parafullName = new SqlParameter
            {
                ParameterName = "FullName",
                DbType = DbType.String,
                Value = (object)fullName ?? DBNull.Value
            };

            SqlParameter paraCompany = new SqlParameter
            {
                ParameterName = "CompanyName",
                DbType = DbType.String,
                Value = (object)companyName ?? DBNull.Value
            };

            SqlParameter paramobileNo = new SqlParameter
            {
                ParameterName = "MobileNo",
                DbType = DbType.String,
                Value = (object)mobileNo ?? DBNull.Value
            };

            SqlParameter paraEmail = new SqlParameter
            {
                ParameterName = "Email",
                DbType = DbType.String,
                Value = (object)email ?? DBNull.Value
            };

            SqlParameter paraSales = new SqlParameter
            {
                ParameterName = "SalesPersonId",
                DbType = DbType.Int64,
                Value = (object)SalesPersonId ?? DBNull.Value
            };
            SqlParameter paraJuniorSales = new SqlParameter
            {
                ParameterName = "JuniorSalesPersonId",
                DbType = DbType.Int64,
                Value = (object)JuniorSalesPersonId ?? DBNull.Value
            };

            SqlParameter paraTrader = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int64,
                Value = (object)TraderId ?? DBNull.Value
            };

            SqlParameter paramLoggedinUserId = new SqlParameter
            {
                ParameterName = "LoggedinUserId",
                DbType = DbType.Int64,
                Value = (object)loggedinUserId ?? DBNull.Value
            };
            SqlParameter paramUserTypeId = new SqlParameter
            {
                ParameterName = "UserTypeId",
                DbType = DbType.Int32,
                Value = (object)UserTypeId ?? DBNull.Value
            };
            SqlParameter paramLoggedinTraderId = new SqlParameter
            {
                ParameterName = "LoggedinTraderId",
                DbType = DbType.Int64,
                Value = (object)LoggedinTraderId ?? DBNull.Value
            };
            SqlParameter paracurrencyBought = new SqlParameter
            {
                ParameterName = "CurrencyBought",
                DbType = DbType.String,
                Value = (object)boughtCurrency ?? DBNull.Value
            };
            SqlParameter paracurrencySold = new SqlParameter
            {
                ParameterName = "CurrencySold",
                DbType = DbType.String,
                Value = (object)soldCurrency ?? DBNull.Value
            };
            SqlParameter paracurrencyLeadCategory = new SqlParameter
            {
                ParameterName = "LeadCategory",
                DbType = DbType.String,
                Value = (object)leadCategory ?? DBNull.Value
            };
            SqlParameter paracurrencyCommunicationDetail = new SqlParameter
            {
                ParameterName = "CommunicationDetail",
                DbType = DbType.String,
                Value = (object)CommunicationDetail ?? DBNull.Value
            };

            SqlParameter paraPageNo = new SqlParameter
            {
                ParameterName = "PageNumber",
                DbType = DbType.Int16,
                Value = pageNo
            };

            SqlParameter paraPageSize = new SqlParameter
            {
                ParameterName = "PageSize",
                DbType = DbType.Int16,
                Value = pageSize
            };
            SqlParameter paraSectorCategoryId = new SqlParameter
            {
                ParameterName = "SectorCategoryId",
                DbType = DbType.Int64,
                Value = (object)sectorCategoryId ?? DBNull.Value
            };
            SqlParameter paraBusinessCategoryId = new SqlParameter
            {
                ParameterName = "BusinessCategoryId",
                DbType = DbType.Int64,
                Value = (object)businessCategoryId ?? DBNull.Value
            };


            List<ProspectModel> result = _usersRepository.ExecuteStoredProcedure<ProspectModel>("usp_ProspectMaster_GetList", parafullName, paraCompany, paramobileNo, paraEmail, paraSales, paraJuniorSales, paraTrader, paramLoggedinUserId, paramUserTypeId, paramLoggedinTraderId, paracurrencyBought, paracurrencySold, paracurrencyLeadCategory, paracurrencyCommunicationDetail, paraPageNo, paraPageSize, paraSectorCategoryId, paraBusinessCategoryId).ToList();
            return result;
        }

        public List<TaskReminderModel> GetTaskReminderlist(long ProspectId)
        {
            SqlParameter paraProspectId = new SqlParameter
            {
                ParameterName = "ProspectId",
                DbType = DbType.Int64,
                Value = ProspectId
            };

            List<TaskReminderModel> clientdeallist = _usersRepository.ExecuteStoredProcedureList<TaskReminderModel>("usp_Taskreminder_GetProspectTaskreminderList", paraProspectId).ToList();
            return clientdeallist;
        }

        public long SaveProspect(ProspectModel model)
        {
            SqlParameter paraProspectId = new SqlParameter
            {
                ParameterName = "ProspectId",
                DbType = DbType.Int64,
                Value = (object)model.ProspectId ?? DBNull.Value
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

            SqlParameter paraMobileNo = new SqlParameter
            {
                ParameterName = "MobileNo",
                DbType = DbType.String,
                Value = (object)model.MobileNo ?? DBNull.Value
            };

            SqlParameter paraEmail = new SqlParameter
            {
                ParameterName = "Email",
                DbType = DbType.String,
                Value = (object)model.EmailId ?? DBNull.Value
            };

            SqlParameter paraAddress = new SqlParameter
            {
                ParameterName = "Address",
                DbType = DbType.String,
                Value = (object)model.ProspectAddress ?? DBNull.Value
            };

            SqlParameter paraSales = new SqlParameter
            {
                ParameterName = "SalesPersonId",
                DbType = DbType.Int64,
                Value = (object)model.SalesPersonId ?? DBNull.Value
            };
            SqlParameter paraJuniorSales = new SqlParameter
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

            SqlParameter paraComm = new SqlParameter
            {
                ParameterName = "Communication",
                DbType = DbType.String,
                Value = (object)model.CommunicationDetail ?? DBNull.Value
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

            SqlParameter paraLeadCategories = new SqlParameter
            {
                ParameterName = "LeadCategoryId",
                DbType = DbType.Int64,
                Value = (object)model.LeadCategoryId ?? DBNull.Value
            };

            SqlParameter paraCreatedBy = new SqlParameter
            {
                ParameterName = "CreatedBy",
                DbType = DbType.Int64,
                Value = (object)model.CreatedBy ?? DBNull.Value
            };
            SqlParameter paraLandline = new SqlParameter
            {
                ParameterName = "LandlineNo",
                DbType = DbType.String,
                Value = (object)model.LandlineNo ?? DBNull.Value
            };
            SqlParameter paraBusinessCategoryId = new SqlParameter
            {
                ParameterName = "BusinessCategoryId",
                DbType = DbType.Int64,
                Value = (object)model.BusinessCategoryId ?? DBNull.Value
            };
            SqlParameter paraBusinessCategorySectorId = new SqlParameter
            {
                ParameterName = "BusinessCategorySectorId",
                DbType = DbType.Int64,
                Value = (object)model.BusinessCategorySectorId ?? DBNull.Value
            };
            long result = _usersRepository.ExecuteStoredProcedure<long>("usp_ProspectMaster_AddEdit", paraProspectId, paraFullName,
                paraCompanyName, paraMobileNo, paraEmail, paraAddress, paraSales, paraTraderId, paraComm, paraBoughtCurrencies,
                paraSoldCurrencies, paraLeadCategories, paraCreatedBy, paraLandline, paraBusinessCategoryId, paraBusinessCategorySectorId, paraJuniorSales).FirstOrDefault();
            return result;
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

        public bool UpdateCommunicationDetail(long ProspectId, string CommunicationDetail)
        {
            SqlParameter paraProspectId = new SqlParameter
            {
                ParameterName = "ProspectId",
                DbType = DbType.Int64,
                Value = ProspectId
            };

            SqlParameter paraCommunicationDetail = new SqlParameter
            {
                ParameterName = "CommunicationDetail",
                DbType = DbType.String,
                Value = CommunicationDetail
            };

            string result = _usersRepository.ExecuteStoredProcedure<string>("usp_ProspectMaster_UpdateCommunicationDetail", paraProspectId, paraCommunicationDetail).FirstOrDefault();
            if (string.IsNullOrEmpty(result))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
