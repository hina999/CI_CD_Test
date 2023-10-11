using System.Web;
using System.Web.Optimization;

namespace FX_System
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region CSS

            bundles.Add(new StyleBundle("~/Content/ui")
                .Include("~/Content/css/bootstrap.min.css",
               "~/Content/css/core.css",
               "~/Content/css/font-awesome.min.css",
               "~/Content/css/Kendo/kendo.common-material.min.css",
               "~/Content/css/Kendo/kendo.material.min.css"));

            #endregion

            #region js
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/Scripts/jquery-1.12.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                "~/Scripts/bootbox.js",
                "~/Scripts/kendo.all.min.js",
                "~/Scripts/kendo.aspnetmvc.min.js",
                "~/Scripts/kendo.culture.en-GB.min.js",
                "~/Content/js/app.js",
                "~/Scripts/Common.js",
                "~/Scripts/jszip.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            #endregion
        }
    }
}
