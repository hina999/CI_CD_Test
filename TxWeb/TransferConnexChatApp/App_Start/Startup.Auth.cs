using System;
using System.Configuration;
using System.Diagnostics;
using FX_System;
using FX_System.Models;
using FXAdminTransferConnex.Data.Helper;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using TransferConnexChatApp.Common;

namespace FXBackOfficeSystemApp
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(60),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});

            string encryptedConnectionString = ConfigurationManager.ConnectionStrings["FXBackOfficeSystemContext"].ConnectionString;
            string decryptedConnectionString = AESEncryptionDecryptionHelper.Decrypt(encryptedConnectionString);
            GlobalConfiguration.Configuration.UseSqlServerStorage(decryptedConnectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            });
            //app.UseHangfireDashboard();
            //new change
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new MyAuthorizationFilter() }
            //});
            //app.UseHangfireServer();
            //var timer = ConfigurationManager.AppSettings["GCPartnerMarketOrderNotificationTime"];
            //RecurringJob.AddOrUpdate("MarketOrderTriggerWithGCPartnerEverySpecificMinute", () => MarketValueNotification.MarketOrderTriggerWithGCPartner(), "*/" + timer.ToString() + " * * * *");

            ////FXAdminTransferConnex.Business.Deal.IDealService _DealService = FXAdminTransferConnexApp.EngineContext.Resolve<FXAdminTransferConnex.Business.Deal.IDealService>();
            //MarketValueNotification deal = new MarketValueNotification();
            //string importDealTimer = ConfigurationManager.AppSettings["CCImportDealTime"];
            //if (!string.IsNullOrEmpty(importDealTimer))
            //{
            //    RecurringJob.AddOrUpdate("ImportDealEverySpecificMinute", () => deal.ImportDeal(), "*/" + importDealTimer + " * * * *");
            //}

            ////string importLogTimerHour = ConfigurationManager.AppSettings["CallLogImportTimeHour"];
            ////string importLogTimerMinute = ConfigurationManager.AppSettings["CallLogImportTimeMinute"];
            ////RecurringJob.AddOrUpdate("GetRingCentralCallLogEverydayOnce", () => deal.GetRingCentralCallLog(false, 0), importLogTimerMinute + " " + importLogTimerHour + " * * *");

            //string importRingcentralLogSpecificTime = ConfigurationManager.AppSettings["importRingcentralLogSpecificTime"];
            //if (!string.IsNullOrEmpty(importRingcentralLogSpecificTime))
            //{
            //    int time = Convert.ToInt32(importRingcentralLogSpecificTime);
            //    RecurringJob.AddOrUpdate("GetRingCentralCallLog", () => deal.GetRingCentralCallLog(true, time), "*/" + importRingcentralLogSpecificTime + " * * * *");
            //}
        }

        public class MyAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                //var httpContext = context.GetHttpContext();

                //// Allow all authenticated users to see the Dashboard (potentially dangerous).
                //return httpContext.User.Identity.IsAuthenticated;
                return true;
            }
        }
    }
}