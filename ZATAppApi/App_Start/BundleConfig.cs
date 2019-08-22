using System.Web;
using System.Web.Optimization;

namespace ZATApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-{version}.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*",
                        "~/Scripts/modernizr.custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/Validator.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/scrolling").Include(
                "~/Scripts/jquery.nicescroll.js",
                "~/Scripts/scripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"
                      ));
            

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.css",
                      "~/Content/SidebarNav.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/metisMenu").Include(
                "~/Scripts/metisMenu.min.js",
                "~/Scripts/custom.js"));
            bundles.Add(new StyleBundle("~/Content/metisMenu").Include(
                "~/Content/custom.css"));

            bundles.Add(new ScriptBundle("~/bundles/owl-carousel").Include(
                    "~/Scripts/owl.carousel.js"));
            bundles.Add(new StyleBundle("~/Content/owl-carousel").Include(
                "~/Content/owl.carousel.css"));

        }
    }
}
