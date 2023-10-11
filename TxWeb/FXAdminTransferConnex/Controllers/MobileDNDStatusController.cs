using FXAdminTransferConnex.Business.Client;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnex.Entities.LocalizationResource;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace FXAdminTransferConnexApp.Controllers
{
    public class MobileDNDStatusController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MobileDNDStatusController()
        {
            _clientService = EngineContext.Resolve<IClientService>();
        }

        // GET: MobileDNDStatus
        //public JsonResult Index(string MobileNo)     
        [HttpGet]
        public ActionResult Index(string MobileNo)           
        {
            //string MobileNo = "02039727210";
            MobileStatus result = _clientService.GetMobileStatus(MobileNo);
            MobileStatus obj = result;

            if (!obj.isClient)
            {
                bool tpsResult = CheckTPSStatus(MobileNo);
                obj.isTPS = tpsResult;
                //_clientService.UpdateMobileStatus(MobileNo, tpsResult);
            }

            //return new JsonpResult
            //{
            //    Data = obj,
            //    Callback = "processMyData"
            //};
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        private bool CheckTPSStatus(string MobileNo)
        {
            try
            {
                string Url = "https://www.tpschecker.co.uk/api/check-number.aspx";
                StringBuilder Post = new StringBuilder();
                Post.AppendFormat("?email={0}", HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["tpscheckerEmail"]));
                Post.AppendFormat("&password={0}", HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["tpscheckerpasssword"]));
                Post.AppendFormat("&number={0}", HttpUtility.UrlEncode(MobileNo));
                //TPS
                Post.AppendFormat("&list={0}", HttpUtility.UrlEncode("TPS"));
                ////CTPS
                //Post.AppendFormat("&list={0}", HttpUtility.UrlEncode("CTPS"));
                ////FPS
                //Post.AppendFormat("&list={0}", HttpUtility.UrlEncode("FPS"));
                Post.AppendFormat("&privacy={0}", HttpUtility.UrlEncode("false"));
                Post.AppendFormat("&format={0}", HttpUtility.UrlEncode("xml"));
                HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(Url + Post.ToString());
                Req.Method = "GET";
                StreamReader Reader = new StreamReader(Req.GetResponse().GetResponseStream());
                string Result = Reader.ReadToEnd();
                Reader.Close();
                XmlDocument Doc = new XmlDocument();
                Doc.XmlResolver = new XmlUrlResolver();
                Doc.LoadXml(Result);
                //Total Balance
                int Balance = Convert.ToInt32(Doc.DocumentElement.SelectSingleNode("//ChecksRemaining").InnerText);
                //Number
                string Number = Doc.DocumentElement.SelectSingleNode("//Number").InnerText;
                //Global Result
                bool CheckResult = Convert.ToBoolean(Doc.DocumentElement.SelectSingleNode("//Result").InnerText);
                //Detailed Result
                //TPS
                bool TPSResult = Convert.ToBoolean(Doc.DocumentElement.SelectSingleNode("//ResultDetailed/List[@Name='TPS']").InnerText);
                ////CTPS
                //bool CTPSResult = Convert.ToBoolean(Doc.DocumentElement.SelectSingleNode("//ResultDetailed/List[@Name='CTPS']").InnerText);
                ////FPS
                //bool FPSResult = Convert.ToBoolean(Doc.DocumentElement.SelectSingleNode("//ResultDetailed/List[@Name='FPS']").InnerText);
                return TPSResult;
            }
            catch (Exception ex)
            {
                _logger.Error("CheckTPSStatus: " + ex.Message);
                return false;
            }
        }

        protected internal JsonpResult Jsonp(object data)
        {
            return Jsonp(data, null /* contentType */);
        }

        protected internal JsonpResult Jsonp(object data, string contentType)
        {
            return Jsonp(data, contentType, null);
        }

        protected internal virtual JsonpResult Jsonp(object data, string contentType, Encoding contentEncoding)
        {
            return new JsonpResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }

    }

    /// <summary>
    /// Renders result as JSON and also wraps the JSON in a call
    /// to the callback function specified in "JsonpResult.Callback".
    /// http://blogorama.nerdworks.in/entry-EnablingJSONPcallsonASPNETMVC.aspx
    /// </summary>
    public class JsonpResult : JsonResult
    {
        /// <summary>
        /// Gets or sets the javascript callback function that is
        /// to be invoked in the resulting script output.
        /// </summary>
        /// <value>The callback function name.</value>
        public string Callback { get; set; }

        /// <summary>
        /// Enables processing of the result of an action method by a
        /// custom type that inherits from <see cref="T:System.Web.Mvc.ActionResult"/>.
        /// </summary>
        /// <param name="context">The context within which the
        /// result is executed.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;
            if (!String.IsNullOrEmpty(ContentType))
                response.ContentType = ContentType;
            else
                response.ContentType = "application/javascript";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Callback == null || Callback.Length == 0)
                Callback = context.HttpContext.Request.QueryString["callback"];

            if (Data != null)
            {
                // The JavaScriptSerializer type was marked as obsolete
                // prior to .NET Framework 3.5 SP1 
#pragma warning disable 0618
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string ser = serializer.Serialize(Data);
                response.Write(Callback + "(" + ser + ");");
#pragma warning restore 0618
            }
        }
    }
}