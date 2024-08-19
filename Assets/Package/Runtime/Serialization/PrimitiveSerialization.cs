using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SnakeCore.Serialization
{
    /// <summary>
    /// Class that provides parsing of string values to a given common type.
    /// Supported types are: <br/>
    /// - string<br/>
    /// - int<br/>
    /// - float<br/>
    /// - double<br/>
    /// - bool<br/>
    /// - char<br/>
    /// - byte<br/>
    /// - short<br/>
    /// - long<br/>
    /// - byte<br/>
    /// - uint<br/>
    /// - ushort<br/>
    /// - ulong<br/>
    /// - decimal<br/>
    /// - DateTime<br/>
    /// - TimeSpan<br/>
    /// - Guid<br/>
    /// </summary>
    public static class PrimitiveSerialization
    {
        private static readonly Type[] CommonTypes = new Type[]
        {
            typeof(string),
            typeof(int),
            typeof(float),
            typeof(double),
            typeof(bool),
            typeof(char),
            typeof(byte),
            typeof(short),
            typeof(long),
            typeof(byte),
            typeof(uint),
            typeof(ushort),
            typeof(ulong),
            typeof(decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(Vector2Int),
            typeof(Vector3Int),
            typeof(Quaternion),
            typeof(Color)
        };
        
        private static readonly Dictionary<Type, string> CommonTypeToPattern = new()
        {
            {typeof(int), @"^-?\d{1,5}$"},
            {typeof(uint), @"^\d{5,10}$"},
            {typeof(long), @"^-?\d{1,19}$"},
            {typeof(ulong), @"^\d{19,20}$"},
            {typeof(float), @"^-?\d+(\.\d+)?$"},
            {typeof(bool), @"^(true|false)$"},
            {typeof(DateTime), @"^\d{4}-\d{2}-\d{2}(T|\s)\d{2}:\d{2}:\d{2}(\.\d{1,7})?(Z|([+-]\d{2}:\d{2}))?$"},
            {typeof(TimeSpan), @"^(-?)((\d+(\.\d+)?):)?([0-5]?\d:[0-5]?\d(\.\d+)?|(60(\.\d+)?))$"},
            {typeof(Guid), @"^[{(]?[0-9a-fA-F]{8}[-]?[0-9a-fA-F]{4}[-]?[0-9a-fA-F]{4}[-]?[0-9a-fA-F]{4}[-]?[0-9a-fA-F]{12}[)}]?$"},
            {typeof(Vector2), @"^\((-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?)\)$"},
            {typeof(Vector3), @"^\((-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?)\)$"},
            {typeof(Vector4), @"^\((-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?)\)$"},
            {typeof(Vector2Int), @"^\((-?\d+),\s*(-?\d+)\)$"},
            {typeof(Vector3Int), @"^\((-?\d+),\s*(-?\d+),\s*(-?\d+)\)$"},
            {typeof(Quaternion), @"^\((-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?)\)$"},
            {typeof(Color), @"^\((\d{1,3}),\s*(\d{1,3}),\s*(\d{1,3}),\s*(\d+(\.\d+)?)\)$"},
            {typeof(string), ".*"}
        };
            
            
        
        public static bool IsPrimitive(Type type)
        {
            bool isPrimitive = CommonTypes.Contains(type);
            if (isPrimitive) return true;
            return type.IsEnum;
        }
        
        /// <summary>
        /// Tries to parser the given value to the given type.
        /// If successful, the result is stored in the out parameter and true is returned. Otherwise, false is returned.
        /// </summary>
        /// <param name="targetType">Type to parser the value to.</param>
        /// <param name="value">Value to parser.</param>
        /// <param name="result">Parsed result.</param>
        /// <returns>True if parsing is successful, false otherwise.</returns>
        /// <exception cref="FormatException">If the given value is not a valid value for the given type.</exception>
        public static bool TryDeserialize(Type targetType, string value, out object result)
        {
            if (targetType.IsEnum)
            {
                if (value == null)
                {   
                   result = null;
                   return true;
                }
                string trimmedValue = value.Trim();
                if (int.TryParse(trimmedValue, out int intValue))
                {
                    if (!Enum.IsDefined(targetType, intValue))
                    {
                        throw new FormatException($"Could not parser value to Enum Type since it is out of range: {value}");   
                    }
                    result = Enum.ToObject(targetType, intValue);
                    return true;
                }

                result = Enum.TryParse(targetType, trimmedValue, false, out result) ? result : null;
                return true;
            }
            
            if(targetType == typeof(string))
            {
                result = value;
                return true;
            }
            
            if(targetType == typeof(int))
            {
                var returnVal = int.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(float))
            {
                var returnVal = float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(double))
            {
                var returnVal = double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(bool))
            {
                var returnVal = bool.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(char))
            {
                var returnVal = char.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(byte))
            {
                var returnVal = byte.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(short))
            {
                var returnVal = short.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(long))
            {
                var returnVal = long.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(uint))
            {
                var returnVal = uint.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(ulong))
            {
                var returnVal = ulong.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(ushort))
            {
                var returnVal = ushort.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(decimal))
            {
                var returnVal = decimal.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(DateTime))
            {
                var returnVal = DateTime.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(TimeSpan))
            {
                var returnVal = TimeSpan.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(Guid))
            {
                var returnVal = Guid.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(Vector2))
            {
                var returnVal = TryParseVector2(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(Vector3))
            {
                var returnVal = TryParseVector3(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(Vector4))
            {
                var returnVal = TryParseVector4(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(Vector2Int))
            {
                var returnVal = TryParseVector2Int(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(Vector3Int))
            {
                var returnVal = TryParseVector3Int(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(Quaternion))
            {
                var returnVal = TryParseQuaternion(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(Color))
            {
                var returnVal = TryParseColor(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            result = null;
            return false;
        }

        public static bool TrySerialize(object value, out string result)
        {
            Type type = value.GetType();
            result = default;
            if (!IsPrimitive(type))
            {
                return false;
            }

            result = value.ToString();
            return true;
        }

        public static Type DetectType(string value)
        {
            if (value == null) return null;
            foreach (var keyValue in CommonTypeToPattern)
            {
                if (Regex.IsMatch(value, keyValue.Value))
                {
                    return keyValue.Key;
                }
            }

            return null;
        }
        private static bool TryParseVector2(string value, out Vector2 result)
        {
            value = value.Trim().Substring(1, value.Length - 2); // Remove the parenthesis ( and )
            string[] parts = value.Split(',');
            if (parts.Length != 2)
            {
                result = default;
                return false;
            }
            if (float.TryParse(parts[0], out float x) && float.TryParse(parts[1], out float y))
            {
                result = new Vector2(x, y);
                return true;
            }
            result = default;
            return false;
        }
        
        private static bool TryParseVector3(string value, out Vector3 result)
        {
            value = value.Trim().Substring(1, value.Length - 2); // Remove the parenthesis ( and )
            string[] parts = value.Split(',');
            if (parts.Length != 3)
            {
                result = default;
                return false;
            }
            if (float.TryParse(parts[0], out float x) && float.TryParse(parts[1], out float y) && float.TryParse(parts[2], out float z))
            {
                result = new Vector3(x, y, z);
                return true;
            }
            result = default;
            return false;
        }
        
        private static bool TryParseVector4(string value, out Vector4 result)
        {
            value = value.Trim().Substring(1, value.Length - 2); // Remove the parenthesis ( and )
            string[] parts = value.Split(',');
            if (parts.Length != 4)
            {
                result = default;
                return false;
            }
            if (float.TryParse(parts[0], out float x) && float.TryParse(parts[1], out float y) && float.TryParse(parts[2], out float z) && float.TryParse(parts[3], out float w))
            {
                result = new Vector4(x, y, z, w);
                return true;
            }
            result = default;
            return false;
        }
        
        private static bool TryParseVector2Int(string value, out Vector2Int result)
        {
            value = value.Trim().Substring(1, value.Length - 2); // Remove the parenthesis ( and )
            string[] parts = value.Split(',');
            if (parts.Length != 2)
            {
                result = default;
                return false;
            }
            if (int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
            {
                result = new Vector2Int(x, y);
                return true;
            }
            result = default;
            return false;
        }
        
        private static bool TryParseVector3Int(string value, out Vector3Int result)
        {
            value = value.Trim().Substring(1, value.Length - 2); // Remove the parenthesis ( and )
            string[] parts = value.Split(',');
            if (parts.Length != 3)
            {
                result = default;
                return false;
            }
            if (int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y) && int.TryParse(parts[2], out int z))
            {
                result = new Vector3Int(x, y, z);
                return true;
            }
            result = default;
            return false;
        }
        
        private static bool TryParseQuaternion(string value, out Quaternion result)
        {
            value = value.Trim().Substring(1, value.Length - 2); // Remove the parenthesis ( and )
            string[] parts = value.Split(',');
            if (parts.Length != 4)
            {
                result = default;
                return false;
            }
            if (float.TryParse(parts[0], out float x) && float.TryParse(parts[1], out float y) && float.TryParse(parts[2], out float z) && float.TryParse(parts[3], out float w))
            {
                result = new Quaternion(x, y, z, w);
                return true;
            }
            result = default;
            return false;
        }
        
        private static bool TryParseColor(string value, out Color result)
        {
            result = default;
            value = value.Trim();
            if (value.StartsWith("RGB("))
            {
                value = value.Substring(4, value.Length - 5); // Remove the RGB( and )
                string[] parts = value.Split(',');
                if (parts.Length != 3)
                {
                    return false;
                }
                if (float.TryParse(parts[0], out float r) && float.TryParse(parts[1], out float g) && float.TryParse(parts[2], out float b))
                {
                    result = new Color(r, g, b);
                    return true;
                }
            }
            else if (value.StartsWith("RGBA("))
            {
                value = value.Substring(4, value.Length - 6); // Remove the RGB( and )
                string[] parts = value.Split(',');
                if (parts.Length != 3)
                {
                    return false;
                }
                if (float.TryParse(parts[0], out float r) && float.TryParse(parts[1], out float g) && float.TryParse(parts[2], out float b) && float.TryParse(parts[3], out float a))
                {
                    result = new Color(r, g, b, a);
                    return true;
                }
            }
            else
            {
                return ColorUtility.TryParseHtmlString(value, out result);
            }

            return false;
        }
    }
}