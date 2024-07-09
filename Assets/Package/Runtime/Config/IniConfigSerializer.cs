// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System.IO;
using System.Text;
using SnakeCore.DI;
using ConfigCollection = System.Collections.Generic.IReadOnlyDictionary<string
    , System.Collections.Generic.IReadOnlyDictionary<string, string>>;

namespace SnakeCore.Config
{
    /// <summary>
    /// IniConfigParser is a simple implementation of IConfigDeserializer and IConfigSerializer that deserializes and
    /// serializes config files in INI format.
    /// </summary>
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(IConfigSerializer))]
    internal class IniConfigSerializer : IConfigSerializer
    {
        /// <inheritdoc cref="IConfigSerializer.Serialize(ConfigCollection)"/>
        public string Serialize(ConfigCollection config)
        {
            if(config == null)
                return "";
            var result = "";
            foreach (var section in config)
            {
                result += $"[{section.Key}]\n";
                foreach (var keyValuePair in section.Value)
                {
                    result += $"{keyValuePair.Key}={keyValuePair.Value}\n";
                }
            }
            return result;
        }
        
        /// <inheritdoc cref="IConfigSerializer.Serialize(ConfigCollection, Stream)"/>
        public void Serialize(ConfigCollection config, Stream outputStream)
        {
            using var streamWriter = new StreamWriter(outputStream, Encoding.UTF8);
            streamWriter.Write(Serialize(config));
        }
    }
}