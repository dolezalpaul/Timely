using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Ajax.Utilities;
using Myslik.Utils;

namespace System.Web.Optimization
{
    public class HandlebarsTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            var templates = new List<HandlebarTemplate>();
            foreach (var file in response.Files)
            {
                templates.Add(new HandlebarTemplate(GetTemplateName(file), GetTemplateContent(file)));
            }
            var compiled = HandlebarsCompiler.Compile(templates);
            var minifier = new Minifier();
            response.Content = minifier.MinifyJavaScript(compiled);
            if (minifier.ErrorList.Count > 0)
            {
                response.Content = compiled;
            }
            response.Cacheability = HttpCacheability.Private;
            response.ContentType = "application/javascript";
        }

        public static string GetTemplateName(BundleFile file)
        {
            string root = file.IncludedVirtualPath.Split('\\')[0] + "/";
            string filePath = "~" + file.VirtualFile.VirtualPath;
            string relative = VirtualPathUtility.MakeRelative(root, filePath);
            return Handlebars.TemplateName.Transform(relative);
        }

        public static string GetTemplateContent(BundleFile file)
        {
            using (var reader = new StreamReader(file.VirtualFile.Open()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}