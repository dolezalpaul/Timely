using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using Moravia.Timely.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moravia.Timely.Formatters
{
    // TODO: Right now highly experimental
    public class EmberJsonMediaTypeFormatter : JsonMediaTypeFormatter
    {
        public override System.Threading.Tasks.Task<object> ReadFromStreamAsync(Type type, System.IO.Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return base.ReadFromStreamAsync(typeof(JObject), readStream, content, formatterLogger).ContinueWith<object>((task) =>
            {
                var data = task.Result as JObject;
                var prefix = type.Name.ToLower();

                if (data[prefix] == null)
                {
                    return GetDefaultValueForType(type);
                }

                var serializer = JsonSerializer.Create(SerializerSettings);

                ViewModel viewModel = data[prefix].ToObject(type, serializer) as ViewModel;
                return viewModel;
            });
        }

        public override System.Threading.Tasks.Task WriteToStreamAsync(Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            try
            {
                if (value is System.Web.Http.HttpError)
                {
                    var wrapper = new Dictionary<string, object>();
                    wrapper.Add("error", value);
                    return base.WriteToStreamAsync(type, wrapper, writeStream, content, transportContext);
                }
                else if (value is IEnumerable<ViewModel>)
                {
                    var wrapper = new Dictionary<string, object>();
                    var viewModels = value as IEnumerable<ViewModel>;
                    wrapper.Add(GetEntityName(type.GenericTypeArguments.First()).Pluralize(), viewModels);
                    var groups = viewModels.SelectMany(vm => vm._sideloads).GroupBy(vm => vm.GetType());
                    foreach (var group in groups)
                    {
                        wrapper.Add(GetEntityName(group.Key).Pluralize(), new HashSet<ViewModel>(group.AsEnumerable()));
                    }
                    return base.WriteToStreamAsync(type, wrapper, writeStream, content, transportContext);
                }
                else
                {
                    var wrapper = new Dictionary<string, object>();
                    var viewModel = value as ViewModel;
                    wrapper.Add(GetEntityName(type), viewModel);
                    var groups = viewModel._sideloads.GroupBy(vm => vm.GetType());
                    foreach (var group in groups)
                    {
                        wrapper.Add(GetEntityName(group.Key).Pluralize(), new HashSet<ViewModel>(group.AsEnumerable()));
                    }
                    return base.WriteToStreamAsync(type, wrapper, writeStream, content, transportContext);
                }
            }
            catch (Exception)
            {
                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
            }
        }

        private string GetEntityName(Type type)
        {
            var namePart = type.Name.Split('_').First();
            return namePart.ToLower();
        }
    }

    public static class StringExtension
    {
        public static string Pluralize(this string text)
        {
            return text + "s";
        }
    }

}