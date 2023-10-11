using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Data.Databases;
using FXAdminTransferConnex.Data.Repository;
using log4net;
using FXAdminTransferConnex.Data.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using FXAdminTransferConnex.Entities;
using System.Web.Mvc;

namespace FXAdminTransferConnex.Business.Common
{
    /// <summary>
    /// Class UserService contains all User table related methods and variable.
    /// </summary>
    public class CommonService : ICommonService
    {


        #region Fields

        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<UserMaster> _usersRepository;

        #endregion

        #region ctor

        public CommonService(IRepository<UserMaster> providerRepository)
        {
            _usersRepository = providerRepository;

        }

        #endregion

        #region Static Methods

        /// <summary>
        /// By Karan Trivedi
        /// 13 FEB 2017
        /// Read Text File 
        /// </summary>
        /// <param name="strFilePath"> File name Path </param>
        /// <returns></returns>
        public static string ReadTextFile(string strFilePath)
        {
            string entireFile = string.Empty;
            StreamReader objectRead = null;

            try
            {
                objectRead = File.OpenText(strFilePath);
                entireFile = objectRead.ReadToEnd();
            }
            catch (Exception ex)
            {
                ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                logger.Error("Exception : " + ex);
            }
            finally
            {
                Security.DisposeOf(objectRead);
            }

            return entireFile;
        }

        /// <summary>
        /// By Karan Trivedi
        /// 16 FEB 2017
        /// send a request and retrieve the response
        /// </summary>
        /// <param name="requestString">the request to send</param>
        /// <param name="method"></param>
        /// <param name="postData"></param>
        /// <returns>the response string</returns>
        public static string MakeRequest(string requestString, string method = "GET", string postData = null)
        {
            HttpWebRequest request = WebRequest.CreateHttp(requestString);

            // for sandbox requests comment below two lines
            const string accessToken = "490a9e7619d07fc7c27b5720f63c22c5-420e8db7047a19cc336af0a3cf7df6cb";
            request.Headers.Add("Authorization", "Bearer " + accessToken);


            request.Method = method;
            if (method == "POST" && postData != null)
            {
                //if (postData != null)
                //{
                    byte[] data = Encoding.UTF8.GetBytes(postData);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;

                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                //}
            }

            using (WebResponse response = request.GetResponse())
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseString = reader.ReadToEnd().Trim();

                    return responseString;
                }
            }
        }

        #endregion


        /// <summary>
        /// By sunil
        /// 01 Mar 2017
        /// Get Communication Type
        /// </summary>
        /// <returns></returns>
        public List<CommunicatinTypeModel> GetCommunicationTypelist()
        {
            List<CommunicatinTypeModel> result = _usersRepository.ExecuteStoredProcedureList<CommunicatinTypeModel>("CommunicationType_GetList").ToList();
            return result;
        }

        /// <summary>
        /// By Mayank
        /// 12 Mar 2018
        /// Get User By UserType
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetUserByUserType(int UserTypeId)
        {
            SqlParameter paraUserTypeId = new SqlParameter
            {
                ParameterName = "UserType",
                DbType = DbType.Int32,
                Value = UserTypeId
            };

            List<UserModel> result = _usersRepository.ExecuteStoredProcedureList<UserModel>("UserMaster_GetUserListByUserType", paraUserTypeId).ToList();
            return result;
        }


        /// <summary>
        /// By Mayank
        /// 12 Mar 2018
        /// Get Currency list
        /// </summary>
        /// <returns></returns>
        public List<CurrencyModel> GetCurrencyList()
        {
            List<CurrencyModel> result = _usersRepository.ExecuteStoredProcedureList<CurrencyModel>("usp_CurrencyMaster_GetCurrencyList").ToList();
            return result;
        }
        public List<CategoryModel> GetCategoryList()
        {
            List<CategoryModel> result = _usersRepository.ExecuteStoredProcedureList<CategoryModel>("usp_BusinessCategoryList").ToList();
            return result;
        }

        /// <summary>
        /// By Mayank
        /// 12 Mar 2018
        /// Get Currency list
        /// </summary>
        /// <returns></returns>
        public List<DealModel> GetDealTradeStatus()
        {
            List<DealModel> result = _usersRepository.ExecuteStoredProcedureList<DealModel>("usp_Deal_GetTStatusList").ToList();
            return result;
        }

        /// <summary>
        /// By Mayank
        /// 04 July 2019
        /// Get Month list
        /// </summary>
        /// <returns></returns>
        public List<MonthModel> GetMonthList()
        {
            List<MonthModel> result = _usersRepository.ExecuteStoredProcedureList<MonthModel>("usp_DD_GetMonthList").ToList();
            return result;

        }

        /// <summary>
        /// By Mayank
        /// 04 July 2019
        /// Get Year list
        /// </summary>
        /// <returns></returns>
        public List<YearModel> GetYearList()
        {
            List<YearModel> result = _usersRepository.ExecuteStoredProcedureList<YearModel>("usp_DD_GetYearList").ToList();
            return result;
        }

        /// <summary>
        /// By Mayank
        /// 17 July 2019
        /// Get Task Type list
        /// </summary>
        /// <returns></returns>
        public List<TaskTypeModel> GetTaskTypeList()
        {
            List<TaskTypeModel> result = _usersRepository.ExecuteStoredProcedureList<TaskTypeModel>("usp_DD_GetTaskTypeList").ToList();
            return result;
        }


        /// <summary>
        /// By Mayank
        /// 19 July 2019
        /// Get User list
        /// </summary>
        /// <returns></returns>
        public List<UserDDModel> GetUserList()
        {
            List<UserDDModel> result = _usersRepository.ExecuteStoredProcedureList<UserDDModel>("usp_DD_GetUserList").ToList();
            return result;
        }

        public List<LeadCategoryModel> GetLeadCategoryList()
        {
            List<LeadCategoryModel> result = _usersRepository.ExecuteStoredProcedureList<LeadCategoryModel>("SP_LeadCategoryMaster_GetList").ToList();
            return result;
        }

        public List<ClientMasterModel> GetClientListForDropdown()
        {
            List<ClientMasterModel> result = _usersRepository.ExecuteStoredProcedureList<ClientMasterModel>("usp_ClientMaster_GetDDL_ClientList").ToList();
            return result;
        }

        public List<ProspectModel> GetProspectListForDropdown()
        {
            List<ProspectModel> result = _usersRepository.ExecuteStoredProcedureList<ProspectModel>("usp_ProspectMaster_GetDDL_ProspectList").ToList();
            return result;
        }
        public List<SelectListItem> GetCompanyList()
        {
            return _usersRepository.ExecuteStoredProcedureList<SelectListItem>("usp_ClientMaster_GetCompanyList").ToList();
        }
        public List<SelectListItem> GetTraderList()
        {
            return _usersRepository.ExecuteStoredProcedureList<SelectListItem>("usp_TraderMaster_GetTraderList").ToList();
        }
        public List<SelectListItem> GetSalesPersonList()
        {
            return _usersRepository.ExecuteStoredProcedureList<SelectListItem>("usp_UserMaster_GetSalesPersonList").ToList();
           
        }
        public List<SectorModel> GetSectorList(long CategoryId)
        {
            SqlParameter ParaCategoryId = new SqlParameter
            {
                ParameterName = "CategoryId",
                DbType = DbType.Int64,
                Value = CategoryId
            };
            return _usersRepository.ExecuteStoredProcedureList<SectorModel>("usp_BusinessCategorySectorList", ParaCategoryId).ToList();
            
        }
        public List<SectorModel> GetBusinessSectorList(int CategoryId)
        {
            SqlParameter ParaCategoryId = new SqlParameter
            {
                ParameterName = "CategoryId",
                DbType = DbType.Int64,
                Value = CategoryId
            };
            return _usersRepository.ExecuteStoredProcedureList<SectorModel>(" usp_BusinessSectorListByCategorytype", ParaCategoryId).ToList();
            
        }

        //------
        public List<SelectListItem> GetSalespersonListByTradeId(int UserId)
        {
            SqlParameter ParaUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = UserId
            };
            return _usersRepository.ExecuteStoredProcedureList<SelectListItem>(" usp_SalesPersonByUserTradeId", ParaUserId).ToList();
           
        }
        //------
    }
}