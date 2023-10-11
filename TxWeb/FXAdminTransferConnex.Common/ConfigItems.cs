//-----------------------------------------------------------------------
// <copyright file="ConfigItems.cs" company="Premiere Digital Services">
//     Copyright Premiere Digital Services. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
namespace FXAdminTransferConnex.Common
{
    /// <summary>
    /// This class will use to get data from web-con-fig file
    /// </summary>
    public static class ConfigItems
    {

        public static string SiteRootPathUrl
        {
            get
            {
                string msRootUrl;
                if (HttpContext.Current.Request.ApplicationPath != null && String.IsNullOrEmpty(HttpContext.Current.Request.ApplicationPath.Split('/')[1]))
                {
                    msRootUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host +
                                ":" +
                                HttpContext.Current.Request.Url.Port;
                }
                else
                {
                    msRootUrl = HttpContext.Current.Request.ApplicationPath;
                }

                return msRootUrl;
            }
        }

        /// <summary>
        /// Gets the current site URL.
        /// </summary>
        /// <value>
        /// The current site URL.
        /// </value>
        public static string CurrentSiteUrl
        {
            get
            {
                string strUrl = HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
                return strUrl;
            }
        }

        /// <summary>
        /// Numeric Validation
        /// </summary>
       public const string NumericExpression = @"^[0-9]*$";
        
        /// <summary>
        /// allow multiple email address with comma(,) speperation
        /// </summary>
        public const string MultipleEmailRegularExpression = @"(([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)(\s*,\s*|\s*$))*";
        /// <summary>
        /// The text box regular expression
        /// </summary>
        public const string TextBoxRegularExpression = @"[^<>]*";

        /// <summary>
        /// The regular expression for file name
        /// </summary>
        public const string RegularExpressionForFileName = @"[<>?/\|*:]*";

        /// <summary>
        /// The name validation expression
        /// </summary>
        public const string NameValidationExpression = @"([a-zA-Z0-9&#32;.&amp;amp;&amp;#39;-]+)";

        /// <summary>
        /// The special character validation expression
        /// </summary>
        public const string SpecialCharacterValidationExpression = @"^[^<>.!@#%/']+$";

        /// <summary>
        /// The decimal validation expression
        /// </summary>
        public const string RegularExprssionForDecimal = @"\d+(\.\d{1,2})?";

        /// <summary>
        /// The website validation expression
        /// </summary>
        public const string RegularExprssionForWebsite = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";

        /// <summary>
        /// The longitude validation expression
        /// </summary>
   
        public const string RegularExprssionForLongitude = @"^-?([1]?[0-7][0-9]|[1]?[1-8][0]|[1-9]?[0-9])\.{1}\d{1,6}";

        /// <summary>
        /// The Latitude validation expression
        /// </summary>
   
        public const string RegularExprssionForLatitude = @"^-?([0-8]?[0-9]|[0-9]0)\.{1}\d{1,6}";
        /// <summary>
        /// The Date Time Format Without Second
        /// </summary>
       
        public const string DateTimeFormatWithoutSecond = "MM/dd/yyyy hh:mm tt";
        public const string DateFormate = "dd/MM/yyyy";
        public const string DateTimeFormate = "dd/MM/yyyy hh:mm tt";

        public const string AmountDisplayFormat = "n2";
        public const string RateDisplayFormat = "0.0000";        
        /// <summary>
        /// Maximum Amount Price
        /// </summary>
        public const double MaxAmount = 9999999.99;

        /// <summary>
        /// Minimum Amount Price
        /// </summary>
        public const double MinAmount = 1.0;

        public static string ProductName
        {
            get
            {
                return ConfigurationManager.AppSettings["PRODUCTNAME"];
            }
        }

        public static string FXBackOfficeSystemUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["FXBackOfficeSystemUrl"];
            }
        }

       
       

        /// <summary>
        /// Gets the facebook app id
        /// </summary>
        /// <value>
        /// The quiver URL.
        /// </value>
        public static string FacebookAppId
        {
            get
            {
                return ConfigurationManager.AppSettings["Facebook:AppId"];
            }
        }

        /// <summary>
        /// Gets the facebook app secret
        /// </summary>
        /// <value>
        /// The quiver URL.
        /// </value>
        public static string FacebookAppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["Facebook:AppSecret"];
            }
        }

       


        /// <summary>
        /// Gets the quiver URL.
        /// </summary>
        /// <value>
        /// The quiver URL.
        /// </value>
        public static string QuiverUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["QuiverUrl"];
            }
        }

        /// <summary>
        /// Gets the amazon s3 service image URL.
        /// </summary>
        /// <value>
        /// The amazon s3 service image URL.
        /// </value>
        public static string AmazonS3ServiceImageUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["AmazonS3ServiceImageUrl"];
            }
        }

       
        /// <summary>
        /// Gets the amazon s3 contract URL.
        /// </summary>
        /// <value>
        /// The amazon s3 contract URL.
        /// </value>
        public static string AmazonS3ContractUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["AmazonS3ContractUrl"];
            }
        }

        /// <summary>
        /// Gets the amazon s3 asset URL.
        /// </summary>
        /// <value>
        /// The amazon s3 asset URL.
        /// </value>
        public static string AmazonS3AssetUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["AmazonS3AssetUrl"];
            }
        }

        /// <summary>
        /// Gets the AWS service image bucket.
        /// </summary>
        /// <value>
        /// The AWS service image bucket.
        /// </value>
        public static string AwsServiceImageBucket
        {
            get
            {
                return ConfigurationManager.AppSettings["AWSServiceImageBucket"];
            }
        }

        /// <summary>
        /// Gets the AWS asset bucket.
        /// </summary>
        /// <value>
        /// The AWS asset bucket.
        /// </value>
        public static string AwsAssetBucket
        {
            get
            {
                return ConfigurationManager.AppSettings["AWSAssetBucket"];
            }
        }

        /// <summary>
        /// Gets the AWS contract bucket.
        /// </summary>
        /// <value>
        /// The AWS contract bucket.
        /// </value>
        public static string AwsContractBucket
        {
            get
            {
                return ConfigurationManager.AppSettings["AWSContractBucket"];
            }
        }

        /// <summary>
        /// Gets the AWS contract bucket.
        /// </summary>
        /// <value>
        /// The AWS contract bucket.
        /// </value>
        public static string AwsSiteMaintenance
        {
            get
            {
                return ConfigurationManager.AppSettings["AWSSiteMaintenance"];
            }
        }

        /// <summary>
        /// Gets the date format.
        /// </summary>
        /// <value>
        /// The date format.
        /// </value>
        public static string DateFormat
        {
            get
            {
                return ConfigurationManager.AppSettings["DateFormat"];
            }
        }

        /// <summary>
        /// Gets the date format grid.
        /// </summary>
        /// <value>
        /// The date format grid.
        /// </value>
        public static string DateFormatGrid
        {
            get
            {
                return "{0: " + ConfigurationManager.AppSettings["DateFormat"] + "}";
            }
        }

        /// <summary>
        /// Gets the date time format.
        /// </summary>
        /// <value>
        /// The date time format.
        /// </value>
        public static string DateTimeFormat
        {
            get
            {
                return ConfigurationManager.AppSettings["DateTimeFormat"];
            }
        }

        /// <summary>
        /// Gets the date time format single digit.
        /// </summary>
        /// <value>The date time format single digit.</value>
        public static string DateTimeFormatSingleDigit
        {
            get
            {
                return ConfigurationManager.AppSettings["DateTimeFormat"].Replace(":ss", string.Empty).Replace("hh", "h").Replace("MM", "M").Replace("dd", "d");
            }
        }

        /// <summary>
        /// Gets the date format for date picker.
        /// </summary>
        /// <value>
        /// The date format for date picker.
        /// </value>
        public static string DateFormatForDatePicker
        {
            get
            {
                return ConfigurationManager.AppSettings["DateFormatForDatePicker"];
            }
        }

        /// <summary>
        /// Gets the date time format grid.
        /// </summary>
        /// <value>
        /// The date time format grid.
        /// </value>
        public static string DateTimeFormatGrid
        {
            get
            {
                return "{0: " + ConfigurationManager.AppSettings["DateTimeFormat"] + "}";
            }
        }

        #region Oanda

        /// <summary>
        /// Gets Currency Cloud api server key
        /// </summary>
        /// <value>
        /// </value>
        public static string CurrencyCloudAPIServer
        {
            get
            {
                return ConfigurationManager.AppSettings["CurrencyCloudAPIServer"];
            }
        }

        /// <summary>
        /// Gets Oanda api Currency Cloud Key
        /// </summary>
        /// <value>
        /// </value>
        public static string CurrencyCloudAPIKey
        {
            get
            {
                return ConfigurationManager.AppSettings["CurrencyCloudAPIKey"];
            }
        }

        /// <summary>
        /// Gets Oanda api Currency Cloud Key
        /// </summary>
        /// <value>
        /// </value>
        public static string CurrencyCloudAPIEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["CurrencyCloudAPIEmail"];
            }
        }

        /// <summary>
        /// Gets the CDN URL.
        /// </summary>
        /// <value>The CDN URL.</value>
        public static string CdnUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["CdnUrl"];
            }
        }

        /// <summary>
        /// Converts the UTC to local.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="timeDifference">The time difference.</param>
        /// <returns>Return DateTime</returns>
        public static DateTime? ConvertUtcToLocal(DateTime? date, string timeDifference)
        {
            if (date != null)
            {
                int hourDifference = Convert.ToInt32(timeDifference.Split(':')[0]);
                int minDifference = Convert.ToInt32(timeDifference.Split(':')[1]);
                return date.Value.AddHours(-hourDifference).AddMinutes(-minDifference);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets Transfer Cost Multiplier Value
        /// </summary>
        /// <value>
        /// </value>
        public static int CostMultiplier 
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["CostMultiplier"]);
            }
        }

        public static int GridPageSize
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            }
        }

        public static string getAuthCanUpdateGrossCommsGBP
        {
            get
            {
                return ConfigurationManager.AppSettings["CanUpdateGrossCommsGBP"];
            }
        }

        /// <summary>
        /// GCPartnerAPIToken 
        /// </summary>
        /// <value>
        /// </value>
        public static string GCPartnerAPIToken
        {
            get
            {
                return ConfigurationManager.AppSettings["GCPartnerAPIToken"];
            }
        }
        /// <summary>
        /// GCPartner BaseAPI
        /// </summary>
        /// <value>
        /// </value>
        public static string GCPartnerAPIServer
        {
            get
            {
                return ConfigurationManager.AppSettings["GCPartnerAPIServer"];
            }
        }

        /// <summary>
        /// Gets Ring Central API Base address
        /// </summary>
        /// <value>
        /// </value>
        public static string RingCentralAPIServer
        {
            get
            {
                return ConfigurationManager.AppSettings["RingCentralAPIServer"];
            }
        }

        /// <summary>
        /// Gets Ring Central API Base address
        /// </summary>
        /// <value>
        /// </value>
        public static string RingCentralUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["RingCentralUserName"];
            }
        }

        /// <summary>
        /// Gets Ring Central API Base address
        /// </summary>
        /// <value>
        /// </value>
        public static string RingCentralPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["RingCentralPassword"];
            }
        }

        /// <summary>
        /// Gets Ring Central API Base address
        /// </summary>
        /// <value>
        /// </value>
        public static string RingCentralExtension
        {
            get
            {
                return ConfigurationManager.AppSettings["RingCentralExtension"];
            }
        }

        /// <summary>
        /// Gets Ring Central API Base address
        /// </summary>
        /// <value>
        /// </value>
        public static string RingCentralGrantType
        {
            get
            {
                return ConfigurationManager.AppSettings["RingCentralGrantType"];
            }
        }
        /// <summary>
        /// Gets GeckoBoard API Base address
        /// </summary>
        /// <value>
        /// </value>
        public static string GeckoboardBaseServer
        {
            get
            {
                return ConfigurationManager.AppSettings["GeckoBoardBaseServer"];
            }
        }
    }
}
#endregion