using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.DashboardConfiguration
{
    public interface IDashboardConfigurationService
    {
        List<UserDashboardConfigurationModel> GetAllUserList();

        List<UserDashboardConfigurationModel> GetWizardListById(int UserId = 0);

        int SwapWizardDisplayOrder(int WizardId, bool IsMoveUp, int UserId);

        int VisibilityChangeWizard(int UserId,int WizardId, bool status);
    }
}
