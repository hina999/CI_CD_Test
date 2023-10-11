using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.Prospect
{
    public interface IProspectService
    {
        ProspectModel GetProspectById(long ProspectId);

        List<ProspectModel> GetProspectlist(int pageNo, int pageSize, string fullName, string companyName, string mobileNo, string email, long? SalesPersonId, long? JuniorSalesPersonId, long? TraderId,long? loggedinUserId, int UserTypeId, long? LoggedinTraderId, string boughtCurrency, string soldCurrency,string leadCategory, string CommunicationDetail, long? sectorCategoryId, long? businessCategoryId);

        int DeleteProspect(long ProspectId);

        long SaveProspect(ProspectModel model);

        List<TaskReminderModel> GetTaskReminderlist(long ProspectId);

        int SaveTaskReminder(TaskReminderModel model);

        bool DeleteTaskReminder(long TaskId);

        bool UpdateCommunicationDetail(long ProspectId, string CommunicationDetail);
        List<ClientMasterModel> GetClientByCompany(string CompanyName);
    }
}
