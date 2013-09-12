using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using Myslik.Utils;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace System.Web.Optimization
{
    public static class Handlebars
    {
        private static string SCRIPT_TEMPLATE = "<script type=\"text/x-handlebars\" id=\"{0}\">\n{1}\n</script>\n";
        private static string SCRIPT_BUNDLE = "<script src=\"{0}\"></script>\n";

        private static HttpServerUtility Server
        {
            get { return HttpContext.Current.Server; }
        }

        private static BundleContext InitBundleContext(string virtualPath)
        {
            return new BundleContext(new HttpContextWrapper(HttpContext.Current), BundleTable.Bundles, virtualPath);
        }

        public static IHtmlString Render(string virtualPath)
        {
            var bundle = BundleTable.Bundles.Single(b => b.Path == virtualPath);
            Debug.Assert(bundle.GetType() == typeof(HandlebarsBundle), "Only HandlebarsBundle can be rendered this way.");
            
            if (BundleTable.EnableOptimizations)
            {
                var path = BundleTable.Bundles.ResolveBundleUrl(virtualPath, true);
                return new HtmlString(String.Format(SCRIPT_BUNDLE, path));
            }
            else
            {
                var builder = new StringBuilder();
                var bundleContext = InitBundleContext(virtualPath);
                var files = bundle.EnumerateFiles(bundleContext);

                foreach (var file in files)
                {
                    builder.AppendFormat(SCRIPT_TEMPLATE, 
                        HandlebarsTransform.GetTemplateName(file), 
                        HandlebarsTransform.GetTemplateContent(file));
                }

                return new HtmlString(builder.ToString());
            }
        }

        private static ITemplateName _templateName = new DefaultTemplateName();
        public static ITemplateName TemplateName
        {
            get { return _templateName; }
            set { _templateName = value; }
        }
    }
}