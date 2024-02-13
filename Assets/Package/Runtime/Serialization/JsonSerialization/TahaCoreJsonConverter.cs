using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

namespace TahaCore.Serialization.JsonSerialization
{
    public class TahaCoreJsonConverter : JsonConverter
    {
        private readonly SerializationContext m_context;
        private readonly IParsingProvider m_parsingProvider;
        public TahaCoreJsonConverter(SerializationContext context, IParsingProvider parsingProvider)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            m_context = context;
            m_parsingProvider = parsingProvider;
        }
        
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type valueType = value.GetType();
            SerializationInfo serializationInfo = m_context.GetSerializationInfo(valueType);
            
            
            JToken token = JToken.FromObject(value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Debug.Log(objectType);
            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

    }
}