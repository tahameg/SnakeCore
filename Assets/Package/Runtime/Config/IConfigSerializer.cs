using System.IO;
using ConfigCollection = System.Collections.Generic.IReadOnlyDictionary<string
    , System.Collections.Generic.IReadOnlyDictionary<string, string>>;

namespace TahaCore.Runtime.Config
{
    /// <summary>
    /// Serializes a collection that contains config values to a string.
    /// A config is consists of sections and each section contains key-value pairs.
    /// </summary>
    internal interface IConfigSerializer
    {
        /// <summary>
        /// Serializes a collection that contains config values to a string.
        /// </summary>
        /// <param name="config">Config values to serialize.</param>
        /// <returns>Serialized config data.</returns>
        string Serialize(ConfigCollection config);
        
        /// <summary>
        /// Serializes a collection that contains config values to a stream.
        /// </summary>
        /// <param name="config">Config values to serialize.</param>
        /// <param name="outputStream">Stream to write the serialized data.</param>
        void Serialize(ConfigCollection config, Stream outputStream);
    }
}