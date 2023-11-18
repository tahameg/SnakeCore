using System;
using System.Collections;
using System.Collections.Generic;
using TahaCore.DI;
using UnityEngine;

namespace TahaCore.Config.Types
{
    /// <summary>
    /// Base class for all types that can be parsed config values.
    /// </summary>
    /// <typeparam name="T">Generic type that is intended to be supported.</typeparam>
    public abstract class ConfigType<T> 
    {
        /// <summary>
        /// Parsed value of the config type.
        /// </summary>
        public T Value { get; private set; }
        
        /// <summary>
        /// Parse string value to the generic type and set the value. Sets to default value if the given string is null.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <exception cref="FormatException">Thrown if the value cannot be parsed to type.</exception>
        public void SetValue(string value)
        {
            try
            {
                Value = Parse(value);
            }
            catch
            {
                TahaCoreApplicationRuntime.LogError($"Cannot parse {value} to {typeof(T).Name}");
                throw new FormatException($"Cannot parse {value} to {typeof(T).Name}");
            }
        }
        
        /// <summary>
        /// Create a new instance of ConfigType with given value parsed. Sets to default value if the given string is null.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <exception cref="FormatException">Thrown if the value cannot be parsed to type.</exception>
        protected ConfigType(string value)
        {
            SetValue(value);
        }
        
        /// <summary>
        /// Create a new instance of ConfigType with default value.
        /// </summary>
        protected ConfigType()
        {
            Value = default;
        }

        /// <summary>
        /// Parse string value to generic type.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <returns>The parsed value. Returns default if the given value is null.</returns>
        /// <exception cref="FormatException">Thrown if the value cannot be parsed to type.</exception>
        protected abstract T Parse(string value);
    }
}