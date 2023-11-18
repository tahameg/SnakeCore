using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using TahaCore.DI;

using ConfigCollection = System.Collections.Generic.IReadOnlyDictionary<string
    , System.Collections.Generic.IReadOnlyDictionary<string, string>>;

namespace TahaCore.Config
{
    /// <summary>
    /// Config Deserializer for INI format.
    /// </summary>
    public class IniConfigDeserializer : IConfigDeserializer
    {
        /// <inheritdoc cref="IConfigDeserializer.Deserialize(Stream)"/>
        /// <exception cref="InvalidConfigFormatException">Thrown if dublicate section names present.</exception>
        public ConfigCollection Deserialize(Stream stream)
        {
            var result = new Dictionary<string, IReadOnlyDictionary<string, string>>();
            string currentSection = null;
            var sectionData = new Dictionary<string, string>();
            var streamReader = new StreamReader(stream);
            string line;
            int lineCount = 0;
            while ((line = streamReader.ReadLine()) != null)
            {
                lineCount++;
                var trimmedLine = line.Trim();
                
                // Skip comments
                if (trimmedLine.StartsWith("#"))
                    continue;
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    // Start of a new section
                    if (currentSection != null)
                    {
                        result[currentSection]
                            = sectionData.ToDictionary(kv => kv.Key, kv => kv.Value);
                    }

                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                    if (result.ContainsKey(currentSection))
                    {
                        // Duplicate section name
                        throw new InvalidConfigFormatException($"Duplicate section name: {currentSection}", lineCount);
                    }
                    sectionData = new Dictionary<string, string>();
                }
                else
                {
                    // Key-value pair within a section
                    var keyValue = trimmedLine.Split('=');
                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Trim();
                        sectionData[key] = value;
                        continue;
                    }

                    if (trimmedLine == "") continue;
                    throw new InvalidConfigFormatException($"Invalid config format -> {trimmedLine}", lineCount);
                }
            }

            // Process the last section, if any
            if (currentSection != null)
            {
                result[currentSection] = sectionData.ToDictionary(kv => kv.Key, kv => kv.Value);
            }

            return result;
        }

        /// <inheritdoc cref="IConfigDeserializer.Deserialize(string)"/>
        public ConfigCollection Deserialize(string config)
        {
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(config));
            return Deserialize(memoryStream);
        }
    }
}