// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System.IO;
using ConfigCollection = System.Collections.Generic.IReadOnlyDictionary<string
    , System.Collections.Generic.IReadOnlyDictionary<string, string>>;

namespace TahaCore.Config
{
    /// <summary>
    /// Deserializes config files to a dictionary.
    /// A config is consists of sections and each section contains key-value pairs.
    /// </summary>
    internal interface IConfigDeserializer
    {
        /// <summary>
        /// Reads a config file from a stream and returns a dictionary.
        /// </summary>
        /// <param name="config">File stream for the config file</param>
        /// <returns>Deserialized collection of config values.</returns>
        /// <exception cref="InvalidConfigFormatException">If the config format is invalid.</exception>
        ConfigCollection Deserialize(Stream config);
        
        /// <summary>
        /// Deserializes content of a config and parses it to a dictionary.
        /// </summary>
        /// <param name="config">Contents of the config.</param>
        /// <returns>Deserialized collection of config values.</returns>
        /// <exception cref="InvalidConfigFormatException">If the config format is invalid.</exception>
        ConfigCollection Deserialize(string config);
    }
}