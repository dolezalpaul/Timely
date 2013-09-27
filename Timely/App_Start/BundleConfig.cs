using System.Web;
using System.Web.Optimization;

namespace Moravia.Timely
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            HandlebarsCompiler.PathToHandlebars = HttpContext.Current.Server.MapPath("~/Scripts/handlebars.js");
            HandlebarsCompiler.PathToEmberCompiler = HttpContext.Current.Server.MapPath("~/Scripts/ember-template-compiler.js");

            bundles.Add(new HandlebarsBundle("~/bundles/handlebars")
                .IncludeDirectory("~/Client", "*.html", true));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/jquery-2.0.3.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/moment.js",
                "~/Scripts/fileuploader.js",
                "~/Scripts/handlebars.js",
                "~/Scripts/ember-1.0.0.debug.js",
                "~/Scripts/ember-data.js").IncludeDirectory("~/Client/", "*.js", true));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap/bootstrap.css",
                "~/Content/bootstrap/bootstrap-theme.css",
                "~/Content/site.css",
                "~/Client/components/dropdown-picker/picker.css"));

            //BundleTable.EnableOptimizations = true;
        }
    }
}