using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.ReconciliationTeam
{
    public interface IReconciliationTeamService
    {
        List<ReconciliationTeamMemberModel> GetReconciliationTeamReport(DateTime? FromDate, DateTime? ToDate);
        List<ReconciliationTeamMemberModel> GetReconciliationTeamData(long? TraderId, DateTime RecordDate);
        List<UserModel> GetSalesPersoListByTraderId(long? TraderId);
        bool AddUpdateReconciliationTeamData(long CreatedBy, DateTime RecordDate, DataTable TeamMember);
        List<ReconciliationTeamMemberModel> GetReconciliationTopReport();
        List<ReconciliationTeamMemberModel> GetReconciliationTeamCurrentYearReport();
        List<ReconciliationTeamMemberModel> GetReconciliationTeamCurrentMonthReport();
    }
}
