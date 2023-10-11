//-----------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Premiere Digital Services">
//     Copyright Premiere Digital Services. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using FXAdminTransferConnex.Data.Databases;
using FXAdminTransferConnex.Entities;
using System.Collections.Generic;
using System.Data;
using FXAdminTransferConnex.Data.Models;

namespace FXAdminTransferConnex.Business.User
{
    /// <summary>
    /// Interface IUserService
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Prototype for Authenticating user
        /// </summary>
        /// <returns></returns>
        Entities.User.LoginUser AuthenticateUser(string emailAddress, string password);


        List<UserAccessPermissions> UserAccessPermissions(int userTypeId, long userId);

        /// <summary>
        /// Prototype for resetting admin's forgot password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //bool ForgotPassword(LoginViewModel model);

        /// <summary>
        /// Prototype for setting newly reset password
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool ResetPassword(LoginViewModel model, string userId);

        /// <summary>
        /// Prototype for get user details
        /// </summary>
        /// <returns></returns>
        List<UserModel> GetUserlist(string search);

        /// <summary>
        /// This Procedure will used for get user by usertype
        /// </summary>
        /// <returns></returns>
        List<UserModel> GetUserlistByUserType(int UserType);

        ///This Procedure Will Return Agent And Broker List
        List<UserModel> GetAgentAndBrokerList();

        /// <summary>
        /// Prototype for add user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddUser(UserModel model);

        /// <summary>
        /// Prototype for change user status
        /// </summary>
        /// <returns></returns>
        bool Changestatus(long userId, bool status);

        /// <summary>
        /// Prototype for delete user
        /// </summary>
        /// <returns></returns>
        bool DeleteUser(long userId);

        /// <summary>
        /// Prototype for get user details by userId
        /// </summary>
        /// <returns></returns>
        UserModel GetUserDetailsById(long userId);



        /// <summary>
        /// Prototype for get user details
        /// </summary>
        /// <returns></returns>
        List<UserType> GetUserTypelist();


        /// <summary>
        /// Prototype for get user details
        /// </summary>
        /// <returns></returns>
        List<UserTypeMenuSearch> GetUserTypeMenuSearchlist(short userTypeId);


        int SaveUserTypeActions(DataTable model);

        List<UserModel> GetUserDetailOnEmail(string email);


        /// <summary>
        /// Prototype for get 18 month of trade user list
        /// </summary>
        /// <returns></returns>
        List<UserModel> GetTrade18MonthUserlist();

        /// <summary>
        /// Prototype for Update last Execution date
        /// </summary>
        /// <returns></returns>
        bool UpdateLastExecutionDate();


        bool ChangePassword(SettingModel model);
        List<UserAccessPermissions> GetAccessPermissionsByUserId(int UserTypeId, long UserId);

        int SaveUserRightsAssignment(UserAssignRightsModel model);
    }
}
