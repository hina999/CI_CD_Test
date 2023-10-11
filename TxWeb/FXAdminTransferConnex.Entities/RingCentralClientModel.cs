using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FXAdminTransferConnex.Entities
{
    public class RingCentralClientModel
    {
        public long ClientId { get; set; }
        public string ClientType { get; set; }
        public string CompanyName { get; set; }
        public string DefaultMargin { get; set; }
        public string ClientName { get; set; }
        public string TraderName { get; set; }
        public string SalesPersonName { get; set; }
    }

    public class RingCentralModel
    {
        public string RingCentralCallId { get; set; }
        public long RingCentralSessionId { get; set; }
        public DateTime CallStartTime { get; set; }
        public int CallDuration { get; set; }
        public double TotalCallDuration { get; set; }
        public double AverageCallDuration { get; set; }
        public string CallType { get; set; }
        public string CallDirection { get; set; }
        public string CallAction { get; set; }
        public string CallResult { get; set; }
        public string CallToName { get; set; }
        public string CallToNumber { get; set; }
        public string CallFromName { get; set; }
        public string CallFromNumber { get; set; }
        public int CallCount { get; set; }
        public int TotalInCount { get; set; }
        public int TotalOutCount { get; set; }
        public int MissedCallCount { get; set; }
        public double AcceptancePercentage { get; set; }
        public string StrCallStartTime { get; set; }
        public int LastMonthCount { get; set; }
        public int ThisMonthCount { get; set; }
        public long? ClientId { get; set; }
        public string ClientCompanyName { get; set; }
        public long? ProspectId { get; set; }
        public string ProspectCompanyName { get; set; }
        public int TotalCount { get; set; }
        public string ImageName { get; set; }
        public string ImageURL
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ImageName))
                {
                    var request = HttpContext.Current.Request;
                    var appUrl = HttpRuntime.AppDomainAppVirtualPath;

                    if (appUrl != "/")
                        appUrl = "/" + appUrl;

                    var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

                    return baseUrl + "/Images/Profiles/" + this.ImageName;
                }
                else
                {
                    return this.ImageName;
                }


            }
            set { }
        }

        public decimal TotalCommission { get; set; }
        public decimal TargetCommission { get; set; }
    }

    public class RingCentralTokenModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public int refresh_token_expires_in { get; set; }
        public string scope { get; set; }
        public string owner_id { get; set; }
        public string endpoint_id { get; set; }
    }

    public class RingCentralResponceModel
    {
        public string uri { get; set; }
        public List<RingCentralRecordModel> records { get; set; }
    }

    public class RingCentralRecordModel
    {
        public string uri { get; set; }
        public string id { get; set; }
        public long sessionId { get; set; }
        public DateTime startTime { get; set; }
        public int duration { get; set; }
        public string type { get; set; }
        public string direction { get; set; }
        public string action { get; set; }
        public string result { get; set; }
        public RingCentralCallToModel to { get; set; }
        public RingCentralCallFromModel from { get; set; }
    }

    public class RingCentralCallFromModel
    {
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public string extensionId { get; set; }
    }

    public class RingCentralCallToModel
    {
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public string extensionId { get; set; }
    }

    public class CurrencyPairModel
    {
        public decimal GBPUSD { get; set; }
        public decimal GBPEUR { get; set; }
        public decimal EURUSD { get; set; }
        public decimal GBPCNY { get; set; }
        public decimal GBPAUD { get; set; }
        public decimal GBPCAD { get; set; }
        public decimal GBPJPY { get; set; }
        public decimal GBPNOK { get; set; }
        public decimal GBPDKK { get; set; }
    }
}
