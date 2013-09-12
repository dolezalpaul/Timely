using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace System.Web.Optimization
{
    public class DefaultTemplateName : ITemplateName
    {
        private static Regex NAME_HANDLER = new Regex(@"(\.html)$", RegexOptions.Compiled);

        public string Transform(string relativePath)
        {
            var full = NAME_HANDLER.Replace(relativePath, "").Split('/');
            var parts = full.Skip(1).ToArray();
            if (parts.Length == 1) return parts[0];
            var reversed = parts.Reverse();
            if (reversed.First() == "default" || reversed.ElementAt(0) == reversed.ElementAt(1))
            {
                reversed = reversed.Skip(1);
            }
            return (full[0] == "components" ? "components/" : "") + String.Join("/", reversed.Reverse());
        }
    }
}