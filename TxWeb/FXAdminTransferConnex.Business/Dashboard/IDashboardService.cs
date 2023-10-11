using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXAdminTransferConnex.Entities;

namespace FXAdminTransferConnex.Business.Dashboard
{
    public interface IDashboardService
    {
        List<Last12MonthsCommissionModel> GetLast12MonthsProfitList(long userId, long traderId);

        List<Last30DaysCommissionModel> GetLast30DaysProfitList(long userId, long traderId);

        List<Last30DaysCommissionModel> GetLast5DaysProfitList(long userId, long traderId);

        List<Last6WeeksCommissionModel> GetLast6WeeksCommissionList(long userId, long traderId);

        List<TraderCommissions> GetTraderCommissionsList(long userId);

        List<SalesPersonCommissions> GetSalesPersonCommissionsList(long userId);

        List<Top10ClientCommissions> GetTop10ClientCommissionsList(long traderId, long salespersonId);

        List<ClientMasterModel> GetAwaitingActionCount();

        List<MarketOrderSettingModel> GetMarketOrderCount(long TraderId,long SalesPersonId);

        List<TaskReminderModel> GetTodayPopupTaskRemider(DateTime SysDate);
        List<ReconciliationTeamMemberModel> GetReconciliationTeamCurrentMonthReport();

        int TaskSnooze(int TaskId, DateTime SysDate);

        int TaskSnoozeAll(long UserId, DateTime SysDate);

        int TaskDismiss(int TaskId);

        int RescheduleTaskReminder(DashboardTaskReminderModel model);

        List<TaskReminderModel> GetPopupTaskRemider();

        List<MarketValueNotificationModel> GetMarketOrderNotificationList(long TraderId, long SalesPersonId);
        bool MarketOrderNotificationUpdate(long NotificationId);
        List<RingCentralModel> GetLast5DayInCallLog();
        List<RingCentralModel> GetLast5DayOutCallLog();
        List<RingCentralModel> GetYesterdaysOutCallLog();
        List<RingCentralModel> GetYesterdaysInCallLog();
        List<GlobalSearchModel> GetGlobalSearchData(string searchString);

        List<Last12MonthsCommissionModel> GetLast12MonthsCompanyCommission();
        List<Last30DaysCommissionModel> GetLast30DaysCompanyCommission();
        List<Last30DaysCommissionModel> GetLast5DaysCompanyCommission();
        List<Last6WeeksCommissionModel> GetLast6WeeksCompanyCommission();

    }
}
