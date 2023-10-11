using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace FXAdminTransferConnex.Business.Commission
{
    public interface ICommissionService
    {
        long SaveTargetCommison(TargetCommissionModel model, DataTable dtTargetDuration);
        List<TargetCommissionModel> GetTargetCommissionlist();
        TargetCommissionModel GetTargetCommissionById(long TargetCommissionId);
        long DeleteTargetCommission(long TargetCommissionId);
        List<TargetDurationModel> GetDailyTargetCommissionById(long TargetCommissionId, int TargetYear);
        List<TargetDurationModel> GetWeeklyTargetCommissionById(long TargetCommissionId,int TargetYear);
        List<TargetDurationModel> GetMonthlyTargetCommissionById(long TargetCommissionId, int TargetYear);
        long ActivateDeactivateTargetCommission(long TargetCommissionId);
        long ImportTargetCommisions(long LoggedinUserId);
    }
}

