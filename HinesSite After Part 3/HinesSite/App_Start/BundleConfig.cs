#region Usings

using System.Web.Optimization;

#endregion

namespace HinesSite {

    /// <summary>
    /// Configures ASPs bundling engine for JS and CSS files
    /// </summary>
    public class BundleConfig {

        /// <summary>
        /// Registers the various JS and CSS bundles. For more information on bundling,
        /// visit http://go.microsoft.com/fwlink/?LinkId=301862
        /// </summary>
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            // Also adding in DatePicker
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/DatePickerReady.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/Scripts/datatables").Include(
                "~/Scripts/jquery.dataTables.js",
                "~/Scripts/dataTables.bootstrap.js",
                "~/Scripts/dataTables.tableTools.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/datatables").Include(
                "~/Content/dataTables.bootstrap.css",
                "~/Content/dataTables.tableTools.css"));

            bundles.Add(new StyleBundle("~/Content/pagedlist").Include(
                "~/Content/PagedList.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            //TODO: Change this back to true
            BundleTable.EnableOptimizations = false;
        }
    }
}
