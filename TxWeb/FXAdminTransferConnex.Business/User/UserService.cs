using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using FXAdminTransferConnex.Business.Common;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Data.Databases;
using FXAdminTransferConnex.Data.Repository;
using FXAdminTransferConnex.Entities;
using log4net;
using System.Collections.Generic;
using FXAdminTransferConnex.Data.Models;

namespace FXAdminTransferConnex.Business.User
{
    public class UserService : IUserService
    {
        #region Fields

        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<UserMaster> _usersRepository;

        #endregion

        #region ctor

        public UserService(IRepository<UserMaster> providerRepository)
        {
            _usersRepository = providerRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// By Karan Trivedi
        /// 10 FEB 2017
        /// Service that authenticates admin
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Entities.User.LoginUser AuthenticateUser(string emailAddress, string password)
        {
            Entities.User.LoginUser loginUser = null;

            SqlParameter emailParam = new SqlParameter
            {
                ParameterName = "Email",
                DbType = DbType.String,
                Value = emailAddress
            };

            SqlParameter passwordParam = new SqlParameter
            {
                ParameterName = "Password",
                DbType = DbType.String,
                Value = Security.Encrypt(password)
            };

            IList<Entities.User.LoginUser> userList =
                    _usersRepository.ExecuteStoredProcedureList<Entities.User.LoginUser>(
                        "UserMaster_AuthenticateUser", emailParam, passwordParam);

            if (userList != null && userList.Any())
                loginUser = userList.FirstOrDefault();

            return loginUser;
        }

        /// <summary>
        /// Method to get menus based on user access permissions
        /// </summary>
        /// <param name="userTypeId">User TypeId</param>
        /// <returns></returns>
        public List<UserAccessPermissions> UserAccessPermissions(int userTypeId, long userId)
        {
            SqlParameter userTypeIdParam = new SqlParameter
            {
                ParameterName = "UserTypeId",
                DbType = DbType.Int32,
                Value = userTypeId
            };
            SqlParameter userIdParam = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = userId
            };
            List<UserAccessPermissions> userAccessPermissionsList = _usersRepository.ExecuteStoredProcedureList<UserAccessPermissions>("sp_UserAccessPermissions", userTypeIdParam, userIdParam).ToList();
            return userAccessPermissionsList;
        }


        /// <summary>
        /// By Karan Trivedi
        /// 13 FEB 2017
        /// Resets the password
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool ResetPassword(LoginViewModel model, string userId)
        {
            SqlParameter useridParam = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int32,
                Value = Convert.ToInt64(Security.Decrypt(userId))
            };

            SqlParameter passwordParam = new SqlParameter
            {
                ParameterName = "Password",
                DbType = DbType.String,
                Value = Security.Encrypt(model.Password)
            };

            IEnumerable<int> result = _usersRepository.ExecuteStoredProcedure<int>("usp_UserMaster_UpdatePassword", useridParam, passwordParam);
            int rowCount = result.FirstOrDefault();
            return (rowCount == 1);
        }

        /// <summary>
        /// By Mayank
        /// 15 FEB 2017
        /// Get user details list
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetUserlist(string search)
        {
            SqlParameter SearchKey = new SqlParameter
            {
                ParameterName = "Search",
                DbType = DbType.String,
                Value = (object)search ?? DBNull.Value
            };

            SqlParameter empiduser = new SqlParameter
            {
                ParameterName = "EmpId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };
            IList<UserModel> result = _usersRepository.ExecuteStoredProcedureList<UserModel>("UserMaster_GetUserList", SearchKey, empiduser);
            List<UserModel> userList = result.Where(m => m.UserTypeId != (int)SystemEnum.UserType.Client).ToList();
            return userList;
        }

        /// <summary>
        /// By Mayank
        /// 15 FEB 2017
        /// Get user details list
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetUserlistByUserType(int UserType)
        {
            SqlParameter UserTypeId = new SqlParameter
            {
                ParameterName = "UserType",
                DbType = DbType.Int32,
                Value = UserType
            };

            SqlParameter paramempid = new SqlParameter
            {
                ParameterName = "EmpId",
                DbType = DbType.Int64,
                Value = ProjectSession.LoginUserDetails.UserId
            };

            IList<UserModel> result = _usersRepository.ExecuteStoredProcedureList<UserModel>("UserMaster_GetUserListByUserType", UserTypeId, paramempid);
            List<UserModel> userList = result.Where(m => m.UserTypeId != (int)SystemEnum.UserType.Client).ToList();
            return userList;
        }

        public List<UserModel> GetAgentAndBrokerList()
        {
            IList<UserModel> result = _usersRepository.ExecuteStoredProcedureList<UserModel>("UserMaster_GetAgentAndBrokerList");
            List<UserModel> userList = result.ToList();
            return userList;
        }

        /// <summary>
        /// By Mayank
        /// 15 FEB 2017
        /// Add/Edit user details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddUser(UserModel model)
        {
            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = (object)model.UserId ?? DBNull.Value
            };

            SqlParameter paraUserTitle = new SqlParameter
            {
                ParameterName = "UserTitle",
                DbType = DbType.String,
                Value = (object)model.UserTitle ?? DBNull.Value
            };

            SqlParameter paraUserTypeId = new SqlParameter
            {
                ParameterName = "UserTypeId",
                DbType = DbType.Int32,
                Value = (object)model.UserTypeId ?? DBNull.Value
            };

            SqlParameter paraTraderId = new SqlParameter
            {
                ParameterName = "TraderId",
                DbType = DbType.Int32,
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

            SqlParameter paraPassword = new SqlParameter
            {
                ParameterName = "Password",
                DbType = DbType.String,
                Value = Security.Encrypt(Convert.ToString(model.Password))
            };

            SqlParameter paraImageName = new SqlParameter
            {
                ParameterName = "ImageName",
                DbType = DbType.String,
                Value = (object)model.ImageName ?? DBNull.Value
            };

            SqlParameter paraCreatedBy = new SqlParameter
            {
                ParameterName = "CreatedBy",
                DbType = DbType.Int64,
                Value = (object)model.CreatedBy ?? DBNull.Value
            };

            SqlParameter paraUpdatedBy = new SqlParameter
            {
                ParameterName = "UpdatedBy",
                DbType = DbType.Int64,
                Value = (object)model.UpdatedBy ?? DBNull.Value
            };
            int result;
            if (model.UserId > 0)
            {
                result = _usersRepository.ExecuteStoredProcedure<int>("usp_UserMaster_UpdateUser", paraUserId, paraUserTitle, paraUserTypeId, paraTraderId, paraFirstName, paraLastName, paraEmailAddress, paraPassword, paraImageName, paraUpdatedBy).FirstOrDefault();
            }
            else
            {
                result = _usersRepository.ExecuteStoredProcedure<int>("usp_UserMaster_InsertUser", paraUserTitle, paraUserTypeId, paraTraderId, paraFirstName, paraLastName, paraEmailAddress, paraPassword, paraImageName, paraCreatedBy).FirstOrDefault();
            }
            return result;
        }

        /// <summary>
        /// By Mayank
        /// 15 FEB 2017
        /// Change user active status
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool Changestatus(long userId, bool status)
        {
            SqlParameter useridParam = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = userId
            };

            SqlParameter action = new SqlParameter
            {
                ParameterName = "Status",
                DbType = DbType.Boolean,
                Value = status
            };

            IEnumerable<int> result = _usersRepository.ExecuteStoredProcedure<int>("usp_UserMaster_UpdateStatus", useridParam, action);
            int rowCount = result.FirstOrDefault();
            return (rowCount == 1);
        }

        /// <summary>
        /// By Mayank
        /// 15 FEB 2017
        /// delete user
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool DeleteUser(long userId)
        {
            SqlParameter useridParam = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = userId
            };
            IEnumerable<int> result = _usersRepository.ExecuteStoredProcedure<int>("usp_UserMaster_Delete", useridParam);
            int rowCount = result.FirstOrDefault();
            return (rowCount == 1);
        }

        /// <summary>
        /// By Mayank
        /// 17 FEB 2017
        /// Get user details list by userid
        /// </summary>
        /// <returns></returns>
        public UserModel GetUserDetailsById(long userId)
        {
            SqlParameter id = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = userId
            };
            IList<UserModel> result = _usersRepository.ExecuteStoredProcedureList<UserModel>("UserMaster_GetUserListByUserId", id);
            UserModel userList = result.FirstOrDefault();
            if (userList != null && (!string.IsNullOrEmpty(userList.Password)))
                userList.Password = Security.Decrypt(userList.Password);

            return userList;
        }

        public List<UserType> GetUserTypelist()
        {
            List<UserType> result = _usersRepository.ExecuteStoredProcedureList<UserType>("UserTypes_GetUserTypesList").ToList();
            return result;
        }
        #endregion


        public List<UserTypeMenuSearch> GetUserTypeMenuSearchlist(short userTypeId)
        {
            SqlParameter userTypeIdParam = new SqlParameter
            {
                ParameterName = "UserTypeId",
                DbType = DbType.Int16,
                Value = userTypeId
            };

            List<UserTypeMenuSearch> result = _usersRepository.ExecuteStoredProcedureList<UserTypeMenuSearch>("sp_UserTypeMenuSearch", userTypeIdParam).ToList();
            return result;
        }


        public int SaveUserTypeActions(DataTable model)
        {
            SqlParameter userTypeMenusDataParam = new SqlParameter
            {
                ParameterName = "UserTypeMenusData",
                SqlDbType = SqlDbType.Structured,
                Value = model,
                TypeName = "dbo.UserTypeMenusTable"
            };
            IEnumerable<int> result = _usersRepository.ExecuteStoredProcedure<int>("UserTypes_SaveUserTypeActions", userTypeMenusDataParam);
            return result.FirstOrDefault();
        }


        /// <summary>
        /// By Viral Patel
        /// 06 april 2017
        /// Get user details by email
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetUserDetailOnEmail(string email)
        {
            SqlParameter Searchemail = new SqlParameter
            {
                ParameterName = "Email",
                DbType = DbType.String,
                Value = (object)email ?? DBNull.Value
            };
            List<UserModel> result = _usersRepository.ExecuteStoredProcedureList<UserModel>("usp_UserMaster_GetUserDetailByEmail", Searchemail).ToList();
            return result;
        }


        /// <summary>
        /// By Mayank
        /// 06 Nov 2017
        /// Get user has no any trade in 18 months
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetTrade18MonthUserlist()
        {
            List<UserModel> result = _usersRepository.ExecuteStoredProcedureList<UserModel>("UserMaster_GetUserTrade18Months").ToList();
            return result;
        }

        /// <summary>
        /// By Mayank
        /// 06 Nov 2017
        /// Update last Execution date
        /// </summary>
        /// <returns></returns>
        public bool UpdateLastExecutionDate()
        {
            IEnumerable<int> result = _usersRepository.ExecuteStoredProcedure<int>("usp_ConfigurationMaster_UpdateLastJobExecutionDate");
            int rowCount = result.FirstOrDefault();
            return (rowCount == 1);
        }


        /// <summary>
        /// By Mayank
        /// 16 July 2019
        /// Change Password
        /// </summary>
        /// <returns></returns>
        public bool ChangePassword(SettingModel model)
        {
            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = (object)model.UserId ?? DBNull.Value
            };

            SqlParameter paraPassword = new SqlParameter
            {
                ParameterName = "Password",
                DbType = DbType.String,
                Value = Security.Encrypt(model.NewPassword)
            };

            SqlParameter paraUpdatedBy = new SqlParameter
            {
                ParameterName = "UpdatedBy",
                DbType = DbType.Int64,
                Value = (object)ProjectSession.LoginUserDetails.UserId ?? DBNull.Value
            };

            IEnumerable<int> result = _usersRepository.ExecuteStoredProcedure<int>("usp_UserMaster_ChangePassword", paraUserId, paraPassword, paraUpdatedBy);
            int rowCount = result.FirstOrDefault();
            return (rowCount == 1);
        }

        #region Individual Access Rights

        public List<UserAccessPermissions> GetAccessPermissionsByUserId(int UserTypeId, long UserId)
        {
            SqlParameter userTypeIdParam = new SqlParameter
            {
                ParameterName = "UserTypeId",
                DbType = DbType.Int32,
                Value = UserTypeId
            };
            SqlParameter userIdParam = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = UserId
            };
            return _usersRepository.ExecuteStoredProcedureList<UserAccessPermissions>("usp_GetIndividualUserRights",
                userTypeIdParam,
                userIdParam
                ).ToList();

        }
        public int SaveUserRightsAssignment(UserAssignRightsModel model)
        {
            DataTable dtTable = new DataTable("UserRoleRights");
            dtTable.Columns.Add("MenuId");
            dtTable.Columns.Add("IsView");
            dtTable.Columns.Add("IsAdd");
            dtTable.Columns.Add("IsDelete");
            dtTable.Columns.Add("IsEdit");


            foreach (UserRoleOperation item in model.roleOperationList)
            {
                DataRow dtRow = dtTable.NewRow();
                dtRow["MenuId"] = item.MenuId;
                dtRow["IsView"] = item.IsView;
                dtRow["IsAdd"] = item.IsAdd;
                dtRow["IsDelete"] = item.IsDelete;
                dtRow["IsEdit"] = item.IsEdit;
                dtTable.Rows.Add(dtRow);
            }

            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = (object)model.UserId ?? DBNull.Value
            };

            SqlParameter paraUserType = new SqlParameter
            {
                ParameterName = "UserTypeId",
                DbType = DbType.Int32,
                Value = (object)model.UserTypeId ?? DBNull.Value
            };

            SqlParameter paramImportReconciliationTable = new SqlParameter
            {
                ParameterName = "ImportUserRightsTable",
                SqlDbType = SqlDbType.Structured,
                Value = dtTable,
                TypeName = "dbo.ImportUserRightsTable"
            };
            return _usersRepository.ExecuteStoredProcedure<int>("usp_SaveIndividualUserRights",
                paraUserId, paraUserType, paramImportReconciliationTable).FirstOrDefault();
        }
        #endregion

    }
}