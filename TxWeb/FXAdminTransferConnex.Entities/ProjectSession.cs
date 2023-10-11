using System;
using System.Collections.Generic;
using System.Web;


namespace FXAdminTransferConnex.Entities
{
    public static class ProjectSession
    {
        /// <summary>
        /// Gets or sets the login user details.
        /// </summary>
        /// <value>The login user details.</value>
        public static User.LoginUser LoginUserDetails
        {
            get
            {
                return 
                    HttpContext.Current.Session["LoginUserDetailsApp"] == null 
                    ? null 
                    : Newtonsoft.Json.JsonConvert.DeserializeObject<User.LoginUser>(Convert.ToString(HttpContext.Current.Session["LoginUserDetailsApp"]));
            }

            set
            {
                HttpContext.Current.Session["LoginUserDetailsApp"] = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }
        }

        public static User.LoginUser LoginChatUserDetails
        {
            get
            {
                return
                    HttpContext.Current.Session["LoginChatUserDetailsApp"] == null
                    ? null
                    : Newtonsoft.Json.JsonConvert.DeserializeObject<User.LoginUser>(Convert.ToString(HttpContext.Current.Session["LoginChatUserDetailsApp"]));
            }
            set
            {
                HttpContext.Current.Session["LoginChatUserDetailsApp"] = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }
        }

        public static List<UserAccessPermissions> UserAccessPermissions
        {
            get
            {
                return HttpContext.Current.Session["UserAccessPermissions"] == null ? new List<UserAccessPermissions>() : HttpContext.Current.Session["UserAccessPermissions"] as List<UserAccessPermissions>;
            }
            set
            {
                HttpContext.Current.Session["UserAccessPermissions"] = value;
            }
        }

        public static DateTime LastJobExecutedDate
        {
            get
            {
                return HttpContext.Current.Session["LastJobExecutedDate"] != null ? Convert.ToDateTime(HttpContext.Current.Session["LastJobExecutedDate"]) : DateTime.MinValue;
            }
            set
            {
                HttpContext.Current.Session["LastJobExecutedDate"] = value;
            }
        }

        public static bool IsMenuSidebarStrip
        {
            get
            {
                if (HttpContext.Current.Session["IsMenuSidebarStrip"] == null)
                {
                    return false;
                }
                else
                {
                    return (bool)HttpContext.Current.Session["IsMenuSidebarStrip"];
                }
            }

            set
            {
                HttpContext.Current.Session["IsMenuSidebarStrip"] = value;
            }
        }

        
        public static string CurrencyCloudToken
        {
            get
            {
                return HttpContext.Current.Session["CurrencyCloudToken"] != null ? Convert.ToString(HttpContext.Current.Session["CurrencyCloudToken"]) : string.Empty;
            }
            set
            {
                HttpContext.Current.Session["CurrencyCloudToken"] = value;
            }
        }
    }
}
