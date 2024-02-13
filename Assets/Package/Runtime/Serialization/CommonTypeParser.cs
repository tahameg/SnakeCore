using System;
using System.Linq;

namespace TahaCore.Serialization
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
    public static class CommonTypeParser
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
            typeof(Guid)
        };
        
        public static bool IsCommonType(Type type)
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
        public static bool TryParsePrimitive(Type targetType, string value, out object result)
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
                var returnVal = float.TryParse(value, out var tempResult);
                result = returnVal ? tempResult : null;
                return returnVal;
            }
            
            if(targetType == typeof(double))
            {
                var returnVal = double.TryParse(value, out var tempResult);
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
            
            result = null;
            return false;
        }
    }
}