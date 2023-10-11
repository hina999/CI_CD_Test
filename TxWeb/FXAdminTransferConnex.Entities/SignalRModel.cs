using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FXAdminTransferConnex.Entities
{
    public class SignalRModel
    {
        public long ClientId { get; set; }
        public string CompanyName { get; set; }
    }

    public class PrivateConversationModel
    {
        public long PersonalChatId { get; set; }
        public long MsgFrom { get; set; }
        public long MsgTo { get; set; }
        public string Msg { get; set; }
        public DateTime MsgDateTime { get; set; }
        public string StrMsgDateTime { get; set; }
        public string MessageDay { get; set; }

        public bool? IsEdited { get; set; }
    }

    public class GroupConversationModel
    {
        public long GroupChatId { get; set; }
        public long MsgFrom { get; set; }
        public long SignalRGroupId { get; set; }
        public string Msg { get; set; }
        public DateTime MsgDateTime { get; set; }
        public string StrMsgDateTime { get; set; }
        public string UserName { get; set; }
        public bool? IsEdited { get; set; }
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
    }

    public class SignalRContactModel
    {
        public long SignalRUserId { get; set; }
        public long SignalRGroupId { get; set; }
        public string SignalRId { get; set; }
        public string UserName { get; set; }
        public string UserInitials { get; set; }
        public bool IsPrivate { get; set; }
        public long IsOnline { get; set; }
        public Nullable<long> UserStatus { get; set; }
        public string OnlineDateTime { get; set; }
        public bool IsGroupOwner { get; set; }
        public int UnReadCount { get; set; }
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
        public bool IsStickedUser { get; set; }

        public string Msg { get; set; }

        public DateTime? MaxDate { get; set; }
    }

    public class SignalRGroupModel
    {
        public long SignalRGroupId { get; set; }

        [Required]
        public string GroupName { get; set; }

        public List<SignalRUsersModel> UserList { get; set; }
    }

    public class SignalRUsersModel
    {
        public long SignalRUserId { get; set; }
        public string UserName { get; set; }
        public bool IsInGroup { get; set; }
    }
    public class SignalRUsersStatus
    {
        public long UserId { get; set; }
        public long UserStatus { get; set; }
    }
}
