using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.Ringcentral
{
    public interface IRingcentralService
    {
        List<RingCentralModel> GetRingcentralCallList(DateTime? FromDate, DateTime? ToDate, string MobileNo, string Name, string CallDirection, int PageNumber, int PageSize);
        List<RingCentralModel> GetOutboundCount();
        List<RingCentralModel> GetInboundCount();
        List<RingCentralModel> GetCallAcceptancePercentage();
        List<RingCentralModel> GetTop3Caller();
        List<RingCentralModel> GetEmployeeWiseCallCount();
        List<RingCentralModel> Last5DayInOutCallLog();
        List<RingCentralModel> GetClientProspectCallLogList(string MobileNo, string AltMobileNo);
        List<RingCentralModel> GetTodaysCommission();
        long SaveCurrencyRate(CurrencyPairModel model);
        List<CurrencyPairModel> GetCurrencyRatePair();
    }
}
