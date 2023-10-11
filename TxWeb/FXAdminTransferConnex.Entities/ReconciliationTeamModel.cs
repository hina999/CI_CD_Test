using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Kendo.Mvc.UI;

namespace FXAdminTransferConnex.Entities
{
    public class ReconciliationTeamModel
    {
        public long? TraderId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime RecordDate { get; set; }
        public List<ReconciliationTeamMemberModel> MemberList { get; set; }

        public List<IGrouping<string,ReconciliationTeamMemberModel>> MemberList1 { get; set; }
    }

    public class ReconciliationTeamMemberModel
    {
        public long? TraderId { get; set; }
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string TraderName { get; set; }
        public int? LeadCount { get; set; }
        public int? RNCCount { get; set; }
        public int? REGCount { get; set; }
        public int? DingCount { get; set; }
        public decimal? Commission { get; set; }
        public string ImageName { get; set; }
        public string UserInitials { get; set; }
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
    }
}
