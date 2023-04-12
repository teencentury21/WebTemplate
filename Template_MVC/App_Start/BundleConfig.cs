using System.Web;
using System.Web.Optimization;

namespace Template_MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/captcha").Include(
                        "~/Scripts/jquery-captcha.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/vue").Include(
                        "~/Scripts/vue.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/axios").Include(
                        "~/Scripts/axios.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
