using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FX_System;
using Autofac;
using System.Threading;
using System.Globalization;
using Microsoft.AspNet.SignalR;
using System.IO;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnex.Common;
using System.Configuration;

namespace FXAdminTransferConnexApp
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ContainerBuilder builder = new ContainerBuilder();
            Bootstrapper.Resolve(builder);// builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            log4net.Config.XmlConfigurator.Configure();
            BundleTable.EnableOptimizations = true;
            // Wait a maximum of 30 minutes after a transport connection is lost
            // before raising the Disconnected event to terminate the SignalR connection.
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromMinutes(240);
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromMinutes(120);
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromMinutes(30);
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            //// Code that runs when an unhandled error occurs           

            log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //// Get the exception object.
            Exception exc = Server.GetLastError();
            logger.Error(exc.Message, exc);
            //// Handle HTTP errors            

            //// Clear the error from the server
            Server.ClearError();

            string fileName = "Exception_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
            string logDirectory = Server.MapPath("~/LoggerInfo/ErrorLogs/");

            // Create the log directory if it doesn't exist
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            string filePath = Path.Combine(logDirectory, fileName);
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine("");
                sw.WriteLine("--------------------------------- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ----------------------------------");
                sw.WriteLine("Requested URL: " + HttpContext.Current.Request.Url.ToString());
                sw.WriteLine("Exception: " + exc.Message);
                sw.WriteLine("Exception: " + exc.InnerException);

                if (exc.InnerException != null)
                {
                    sw.WriteLine("Exception: " + exc.InnerException.InnerException);
                }
            }

            string emailBody = string.Empty;
            string ErrorLogEmail = ConfigurationManager.AppSettings["ErrorLogEmail"];
            string BasePath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstantData.ExceptionReportPath);
            string BasePath = Path.Combine(Directory.GetCurrentDirectory(), ConstantData.ExceptionReportPath);

            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            bool isSuccess = false;

            using (StreamReader reader = new StreamReader(Path.Combine(BasePath1, ConstantData.ExceptionReport)))
            {
                emailBody = reader.ReadToEnd();
            }
            if (ProjectSession.LoginUserDetails != null)
            {
                string userName = ProjectSession.LoginUserDetails.FirstName + " " + ProjectSession.LoginUserDetails.LastName;
                string userId = ProjectSession.LoginUserDetails.UserId.ToString();
                emailBody = emailBody.Replace("##LogoURL##", ConstantData.logoUrl);
                emailBody = emailBody.Replace("##DateTime##", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                emailBody = emailBody.Replace("##RequestedURL##", HttpContext.Current.Request.Url.ToString());
                emailBody = emailBody.Replace("##ExceptionMessage##", exc.Message);
                //emailBody = emailBody.Replace("##RequestParams##", HttpContext.Current.Request.Params.ToString());
                //emailBody = emailBody.Replace("##QueryStringParams##", HttpContext.Current.Request.Url.Query);
                emailBody = exc.InnerException != null ? emailBody.Replace("##InnerException##", exc.InnerException.ToString()) : emailBody.Replace("##InnerException##", string.Empty);
                emailBody = emailBody.Replace("##UserId##", userId);
                emailBody = emailBody.Replace("##UserName##", userName);
                // FTFS.Common.EmailNotification.SendMailMessage(_appSettings.ErrorLogEmail, null, null, "Exception Log Alert !", emailBody, setting, null);


                isSuccess = EmailNotification.SendAsyncEmail(ErrorLogEmail, null, null, "Exception Log Alert !", emailBody, null);
            }
            
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (url.Contains("http://") && !url.ToLower().Contains("localhost") && !url.ToLower().Contains("5.77.39.57"))
            {
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl);
            }
            //string[] MVC_URL = new string[] { "/", "/Home/GetAdminUserDetail", "/Home/GetStoreDetail", "/Home/GetAdminUserDetail", "/Admin/Login", "/Home/accessdenied" };
            //var RquestedUrl = Convert.ToString(Request.Url.AbsolutePath);
            //var aa=!((Array.IndexOf(MVC_URL, RquestedUrl) >= 0));
            //var sca = !(RquestedUrl.Contains(".cshtml"));
            //var cac = !(RquestedUrl.Contains(".html"));

            //if (!(Array.IndexOf(MVC_URL, RquestedUrl) >= 0) && !(RquestedUrl.Contains(".cshtml")) && !(RquestedUrl.Contains(".html")))
            //{
            //    var PathAndQuery =Convert.ToString(Request.Url.PathAndQuery);
            //    var Authority =Convert.ToString(Request.Url.Authority);
            //    var Scheme = Convert.ToString(Request.Url.Scheme);

            //    Response.Redirect(Scheme + "://" + Authority + "/#/" + PathAndQuery);

            //}

        }

        /// <summary>
        /// Handles the End event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_End(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Fired when a user's session times out, ends, or they leave the application Web site.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Session_End(object sender, EventArgs e)
        {
            
        }
    }
}
