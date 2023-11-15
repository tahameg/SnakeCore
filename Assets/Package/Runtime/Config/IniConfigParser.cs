using System.Collections.Generic;
using System.IO;
using System.Linq;
using TahaCore.DI;

namespace TahaCore.Config
{
    /// <summary>
    /// IniConfigParser is a simple implementation of IConfigDeserializer and IConfigSerializer that deserializes and
    /// serializes config files in INI format.
    /// </summary>
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(IConfigDeserializer), typeof(IConfigSerializer))]
    internal class IniConfigParser : IConfigDeserializer, IConfigSerializer
    {
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Deserialize(Stream stream)
        {
            var result = new Dictionary<string, IReadOnlyDictionary<string, string>>();
            string currentSection = null;
            var sectionData = new Dictionary<string, string>();
            var streamReader = new StreamReader(stream);
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                var trimmedLine = line.Trim();
                // Skip comments
                if(trimmedLine.StartsWith("#"))
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
                    }
                    // Invalid line format (ignoring for simplicity)
                }
            }

            // Process the last section, if any
            if (currentSection != null)
            {
                result[currentSection] = sectionData.ToDictionary(kv => kv.Key, kv => kv.Value);
            }

            return result;
        }

        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Deserialize(string config)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    streamWriter.Write(config);
                    memoryStream.Position = 0;
                }
                
                return Deserialize(memoryStream);
            }
        }

        public string Serialize(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> config)
        {
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

        public void Serialize(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> config, Stream outputStream)
        {
            using (var streamWriter = new StreamWriter(outputStream))
            {
                streamWriter.Write(Serialize(config));
            }
        }
    }
}