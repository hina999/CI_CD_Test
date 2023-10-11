using FXAdminTransferConnex.Data.Repository;
using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.DashboardConfiguration
{
    public class DashboardConfigurationService : IDashboardConfigurationService
    {
        
        private readonly IRepository<UserDashboardConfigurationModel> _configurationRepository;

        #region ctor
        public DashboardConfigurationService(IRepository<UserDashboardConfigurationModel> providerDashboardRepository)
        {
            
            _configurationRepository = providerDashboardRepository;

        }
        #endregion

        public List<UserDashboardConfigurationModel> GetAllUserList()
        {

            IList<UserDashboardConfigurationModel> result = _configurationRepository.ExecuteStoredProcedureList<UserDashboardConfigurationModel>("DashboardConfig_GetUserList");
            List<UserDashboardConfigurationModel> userList = result.ToList();
            return userList;
        }

        public List<UserDashboardConfigurationModel> GetWizardListById(int UserId = 0)
        {
            SqlParameter paramCurrentUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int32,
                Value = UserId,
            };
            IList<UserDashboardConfigurationModel> result = _configurationRepository.ExecuteStoredProcedureList<UserDashboardConfigurationModel>("[DashboardConfig_GetWizardListById]", paramCurrentUserId);
            List<UserDashboardConfigurationModel> wizardList = result.ToList();
            return wizardList;
        }

        public int SwapWizardDisplayOrder(int WizardId, bool IsMoveUp,int UserId)
        {
            SqlParameter paramUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int32,
                Value = UserId,
            };

            SqlParameter paramWizardId = new SqlParameter
            {
                ParameterName = "WizardId",
                DbType = DbType.Int32,
                Value = WizardId,
            };

            SqlParameter paramIsMoveUp = new SqlParameter
            {
                ParameterName = "IsMoveUp",
                DbType = DbType.Boolean,
                Value = IsMoveUp,
            };
            return _configurationRepository.ExecuteStoredProcedure<int>("[DashboardConfig_SwapWizardDisplayOrder]", paramUserId, paramWizardId, paramIsMoveUp).ToList().FirstOrDefault() ;
        }

        public int VisibilityChangeWizard(int UserId,int WizardId, bool status)
        {
            SqlParameter paramUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int32,
                Value = UserId,
            };


            SqlParameter paramWizardId = new SqlParameter
            {
                ParameterName = "WizardId",
                DbType = DbType.Int32,
                Value = WizardId,
            };

            SqlParameter paramstatus = new SqlParameter
            {
                ParameterName = "IsMoveUp",
                DbType = DbType.Boolean,
                Value = status,
            };
            return _configurationRepository.ExecuteStoredProcedure<int>("[DashboardConfig_VisibilityChangeWizard]", paramUserId, paramWizardId, paramstatus).ToList().FirstOrDefault();
        }

    }
}
