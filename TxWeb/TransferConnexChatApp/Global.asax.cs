using Autofac;
using FX_System;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TransferConnexChatApp
{
    public class MvcApplication : System.Web.HttpApplication
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

            //AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
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
