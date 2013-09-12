using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
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

                // var properties = (data[prefix] as JObject).Properties().Select(p => p.Name);

                var serializer = JsonSerializer.Create(SerializerSettings);

                Entity entity = data[prefix].ToObject(type, serializer) as Entity;
                return entity;
            });
        }

        public override System.Threading.Tasks.Task WriteToStreamAsync(Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            try
            {
                var wrapper = new Dictionary<string, object>();
                if (value is IEnumerable)
                {
                    var values = value as IEnumerable;
                    foreach (var entity in values)
                    {
                        var entityName = entity.GetType().Name.ToLower().Pluralize();
                        if (!wrapper.ContainsKey(entityName))
                        {
                            wrapper.Add(entityName, new List<object>());
                        }
                        ((List<object>)wrapper[entityName]).Add(entity);
                    }
                }
                else
                {
                    wrapper.Add(type.Name.ToLower(), value);
                }
                return base.WriteToStreamAsync(type, wrapper, writeStream, content, transportContext);
            }
            catch (Exception)
            {
                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
            }
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