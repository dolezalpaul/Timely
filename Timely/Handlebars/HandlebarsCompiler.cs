using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Myslik.Utils;

namespace System.Web.Optimization
{
    public class HandlebarsCompiler
    {
        private static string COMPILER_WRAPPER = @"
            var exports = {};

            var precompile = function (template) {
                return exports.precompile(template).toString();
            };";

        public static string PathToHandlebars { get; set; }
        public static string PathToEmberCompiler { get; set; }

        private static string _handlebarsScript;
        private static string GetHandlebars(string path)
        {
            if (_handlebarsScript == null)
            {
                _handlebarsScript = File.ReadAllText(path);
            }
            return _handlebarsScript;
        }

        private static string _emberScript;
        private static string GetEmber(string path)
        {
            if (_emberScript == null)
            {
                _emberScript = File.ReadAllText(path);
            }
            return _emberScript;
        }

        private static string CompilerScript
        {
            get
            {
                return String.Format("{0}\n{1}\n{2}", 
                    COMPILER_WRAPPER, 
                    GetHandlebars(PathToHandlebars), 
                    GetEmber(PathToEmberCompiler));
            }
        }

        private static Dictionary<string, string> _versions = new Dictionary<string, string>();
        private static Dictionary<string, string> _compiled = new Dictionary<string, string>();

        public static string Compile(IEnumerable<HandlebarTemplate> templates)
        {
            var toBeCompiled = new List<HandlebarTemplate>();
            foreach (var template in templates)
            {
                if (!_versions.ContainsKey(template.Name) || _versions[template.Name] != template.Hash)
                {
                    toBeCompiled.Add(template);
                }
            }
            if (toBeCompiled.Count > 0)
            {
                using (var _engine = new ScriptEngine("jscript"))
                {
                    var _vm = _engine.Parse(CompilerScript);

                    foreach (var template in toBeCompiled)
                    {
                        var compiled = String.Format("Ember.TEMPLATES[\"{0}\"] = Ember.Handlebars.template({1});\n",
                            template.Name,
                            PrecompileTemplate(_vm, template.Content));
                        _versions[template.Name] = template.Hash;
                        _compiled[template.Name] = compiled;
                    }
                }
            }
            var builder = new StringBuilder();
            foreach (var template in templates)
            {
                builder.AppendLine(_compiled[template.Name]);
            }
            return builder.ToString();
        }

        private static string PrecompileTemplate(ParsedScript _vm, string template)
        {
            return (string)_vm.CallMethod("precompile", template);
        }
    }
}