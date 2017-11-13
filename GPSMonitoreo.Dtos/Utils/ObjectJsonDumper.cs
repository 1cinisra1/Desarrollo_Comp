using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Reflection;


namespace GPSMonitoreo.Dtos.Utils
{
    public class ObjectJsonDumper
    {
        public class CustomJsonTextWriter : JsonTextWriter
        {
            public CustomJsonTextWriter(TextWriter textWriter) : base(textWriter) { }

            public int CurrentDepth { get; private set; }

            public override void WriteStartObject()
            {
                CurrentDepth++;
                base.WriteStartObject();
            }

            public override void WriteEndObject()
            {
                CurrentDepth--;
                base.WriteEndObject();
            }
        }

        public class CustomContractResolver : DefaultContractResolver
        {
            private readonly Func<bool> _includeProperty;

            public CustomContractResolver(Func<bool> includeProperty)
            {
                _includeProperty = includeProperty;
            }

            protected override JsonProperty CreateProperty(
                MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);
                var shouldSerialize = property.ShouldSerialize;
                property.ShouldSerialize = obj => _includeProperty() &&
                                                  (shouldSerialize == null ||
                                                   shouldSerialize(obj));
                return property;
            }
        }

        public static void Dump(object obj, int maxDepth)
        {
            Console.WriteLine(SerializeObject(obj, maxDepth));
        }

        public static string SerializeObject(object obj, int maxDepth)
        {
            /*
            var settings = new Newtonsoft.Json.JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                MaxDepth = 1,
                
                Error = (sender, errorArgs) => {
                    var currentError = errorArgs.ErrorContext.Error.Message;
                    errorArgs.ErrorContext.Handled = true;
                }
            };
            */


            using (var strWriter = new StringWriter())
            {
                using (var jsonWriter = new CustomJsonTextWriter(strWriter))
                {
                    Func<bool> include = () => jsonWriter.CurrentDepth <= maxDepth;
                    var resolver = new CustomContractResolver(include);
                    var serializer = new JsonSerializer() { ContractResolver = resolver, Formatting = Formatting.Indented, ReferenceLoopHandling = ReferenceLoopHandling.Ignore};
                    serializer.Serialize(jsonWriter, obj);
                }
                return strWriter.ToString();
            }
        }

		public static string ToJsonString(object obj)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
		}
    }
}
