using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.Utilities
{
    public class MJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jo = JObject.FromObject(value);
            foreach (var property in jo)
            {
                if (property.Value.Type == JTokenType.Array || property.Value.Type == JTokenType.Object)
                {
                    //continue;
                    property.Value.Replace(null);
                }
            }
            jo.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return JToken.ReadFrom(reader).ToObject(objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
