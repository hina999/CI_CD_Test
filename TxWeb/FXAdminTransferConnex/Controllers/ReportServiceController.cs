using System;
using System.Web;
using System.Web.Http.Description;
using Telerik.Reporting.Cache.Interfaces;
using Telerik.Reporting.Services.Engine;
using Telerik.Reporting.Services.WebApi;
using CacheFactory = Telerik.Reporting.Services.Engine.CacheFactory;

namespace FXBackOfficeSystemApp.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ReportServiceController : ReportsControllerBase
    {
        #region Methods
        [Obsolete("CreateReportResolver method is now obsolete. Please provide service setup using the Telerik.Reporting.Services.WebApi.ReportsControllerBase.ReportServiceConfiguration property.")]
        protected override IReportResolver CreateReportResolver()
        {
            string reportsPath = HttpContext.Current.Server.MapPath("~/");

            return new ReportFileResolver(reportsPath)
                .AddFallbackResolver(new ReportTypeResolver());
        }

        [Obsolete("CreateReportResolver method is now obsolete. Please provide service setup using the Telerik.Reporting.Services.WebApi.ReportsControllerBase.ReportServiceConfiguration property.")]
        protected override ICache CreateCache()
        {
            return CacheFactory.CreateFileCache();
        }
        #endregion
    }
}