using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnakeCore.Reflection;
using UnityEngine;

namespace SnakeCore.Serialization.JsonSerialization
{
    public class TahaCoreJsonConverter : JsonConverter
    {
        private ISerializationContext m_serializationContext;
        public TahaCoreJsonConverter(ISerializationContext serializationContext)
        {
            m_serializationContext = serializationContext;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) return;
            Serialize(writer, value);

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(objectType.IsArray || typeof(IList).IsAssignableFrom(objectType))
            {
                JArray array = JArray.Load(reader);
                return DeserializeArray(array, objectType);
            }

            if (objectType == typeof(object))
            {
                bool found = true;
                
                while (reader.TokenType != JsonToken.StartObject && reader.TokenType != JsonToken.StartArray)
                {
                    found = reader.Read();
                }

                if (!found)
                {
                    throw new JsonException("Cannot deserialize object to an unknown type without an $type parameter.");
                }
                
                if(reader.TokenType == JsonToken.StartArray)
                {
                    JArray array = JArray.Load(reader);
                    return DeserializeArray(array, objectType);
                }
            }
            
            
            JObject obj = JObject.Load(reader);
            return Deserialize(obj, objectType);
        }
        
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
        
        private object DeserializeArray(JArray array, Type objectType)
        {
            if(objectType == typeof(object))
            {
                throw new JsonException("Cannot deserialize to an array of unknown type.");
            }
            
            if (objectType.IsArray)
            {
                Type itemType = objectType.GetElementType();
                if (itemType == null) return null;
            
                Array result = Array.CreateInstance(itemType, array.Count);
                int i = 0;
                foreach (var item in array)
                {
                    var deserialized = Deserialize(item, itemType);
                    result.SetValue(deserialized, i);
                    i++;
                }

                return result;
            }
            
            if (typeof(IList).IsAssignableFrom(objectType))
            {   
                Type itemType = objectType.IsGenericType ? objectType.GetGenericArguments()[0] : typeof(object);
                IList result = (IList)Activator.CreateInstance(objectType);
                foreach (var item in array)
                {
                    var deserialized = Deserialize(item, itemType);
                    result.Add(deserialized);
                }

                return result;
            }
            
            throw new JsonException($"Type {objectType} is not serializable.");
        }

        private object Deserialize(JToken token, Type elementType)
        {
            if(token.Type == JTokenType.Object)
            {
                return DeserializeSerializableType(token as JObject, elementType);
            }
            
            if (token.Type == JTokenType.Array)
            {
                return DeserializeArray(token as JArray, elementType);
            }
            
            return DeserializePrimitive(token, elementType);
        }
        
        private object DeserializeSerializableType(JObject obj, Type objectType)
        {
            object result;
            SerializationInfo serializationInfo;
            string typeParameter = obj["$type"].Value<string>();
            
            if (objectType == typeof(object))
            {
                if (typeParameter == null)
                {
                    throw new JsonException("Cannot deserialize object to an unknown type without an object parameter.");
                }
                var type = TypeUtility.GetType(typeParameter);
                if (type == null)
                {
                    throw new JsonException($"Type {typeParameter} could not be found.");
                }

                if (!m_serializationContext.TryGetSerializationInfo(type, out serializationInfo))
                {
                    throw new JsonException($"Type {type} is not serializable.");
                }
                
                result = Activator.CreateInstance(type);
                foreach (var property in serializationInfo.SerializableProperties)
                {
                    PropertyInfo info = property.Value;
                    var propertyToken = obj[property.Key];
                    if(propertyToken == null) continue;
                    object value = Deserialize(propertyToken, info.PropertyType);
                    info.SetValue(result, value);
                }

                return result;
            }
            
            Type typeToDeserilize = typeParameter == null ? objectType : TypeUtility.GetType(typeParameter);
            typeToDeserilize ??= objectType;
            
            if (!m_serializationContext.TryGetSerializationInfo(typeToDeserilize, out serializationInfo))
            {
                throw new JsonException($"Type {objectType} is not serializable.");
            }
            
            if(objectType != typeToDeserilize && !objectType.IsAssignableFrom(typeToDeserilize))
            {
                throw new JsonException($"The json object with type parameter{typeParameter} cannot be deserialized to {objectType}");
            }
            
            result = Activator.CreateInstance(typeToDeserilize);
            foreach (var property in serializationInfo.SerializableProperties)
            {
                PropertyInfo info = objectType != typeToDeserilize 
                    ? typeToDeserilize.GetProperty(property.Value.Name) : property.Value;
                
                //This is unlikely to happen but just in case.
                if(info == null) continue;
                
                var propertyToken = obj[property.Key];
                if(propertyToken == null) continue;
                object value = Deserialize(propertyToken, info.PropertyType);
                info.SetValue(result, value);
            }

            return result;
        }

        private object DeserializePrimitive(JToken token, Type elementType)
        {
            if (PrimitiveSerialization.TryDeserialize(elementType, token.Value<string>(),  out var result))
            {
                return result;
            }

            throw new JsonException($"Cannot deserialize {token} to {elementType}");
        }
        
        private void Serialize(JsonWriter writer, object value,
            bool writePrimitives = false)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            Type objectType = value.GetType();
            if (m_serializationContext.IsSerializable(objectType))
            {
                SerializeSerializableType(writer, value);
            }
            else if (PrimitiveSerialization.IsPrimitive(objectType) && writePrimitives)
            {
                writer.WriteValue(GetPrimitiveValueOrString(value));
            }
            else if (objectType.IsArray)
            {
                SerializeArray(writer, value);
            }
            else if(value is IList)
            {
                SerializeList(writer, value);
            }
            else
            {
                throw new JsonException($"Type {objectType} is not serializable.");
            }
        }

        private void SerializeSerializableType(JsonWriter writer, object value)
        {
            Type objectType = value.GetType();
            writer.WriteStartObject();
            writer.WritePropertyName("$type");
            writer.WriteValue(objectType.FullName);
            if (m_serializationContext.TryGetSerializationInfo(objectType, out var serializationInfo))
            {
                foreach (var property in serializationInfo.SerializableProperties)
                {
                    writer.WritePropertyName(property.Key);
                    object propertyValue = property.Value.GetValue(value);
                    Serialize(writer, propertyValue, true);
                }
            }
            writer.WriteEndObject();
        }
        
        private void SerializeArray(JsonWriter writer, object value)
        {
            writer.WriteStartArray();
            var asArray = (Array)value;
            foreach (var item in asArray)
            {
                Serialize(writer, item, true);
            }

            writer.WriteEndArray();
        }
        
        private void SerializeList(JsonWriter writer, object value)
        {
            writer.WriteStartArray();
            foreach (var item in (IEnumerable)value)
            {
                Serialize(writer, item, true);
            }
            writer.WriteEndArray();
        }
        
        private object GetPrimitiveValueOrString(object value)
        {
            Type valueType = value.GetType();
            Type[] unsupportedTypes = new Type[]
            {
                typeof(Vector2), 
                typeof(Vector3), 
                typeof(Vector4), 
                typeof(Vector2Int), 
                typeof(Vector3Int),
                typeof(Quaternion),
                typeof(Color)
            };

            if (unsupportedTypes.Contains(valueType) && PrimitiveSerialization.TrySerialize(value, out var result))
            {
                return result;
            }
            
            return value;
        }
        
    }
}