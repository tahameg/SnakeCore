using System.Collections.Generic;
using Cysharp.Threading.Tasks;

using ConfigCollection = System.Collections.Generic.IReadOnlyDictionary<string
    , System.Collections.Generic.IReadOnlyDictionary<string, string>>;

namespace TahaCore.Config
{
    /// <summary>
    /// Manages config operations.
    /// A config is consists of sections and each section contains key-value pairs.
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// Append additional config to the current config. Additional config will not be saved to the file
        /// but it will be used for the current session.
        /// </summary>
        /// <param name="additionalConfig">Content of the additional config.</param>
        void AppendConfig(string additionalConfig);
        
        /// <summary>
        /// Retrieves a config section values as a dictionary.
        /// </summary>
        /// <param name="sectionName">Name of the section to retreive.</param>
        /// <returns>Retreived section. Null if section is not found.</returns>
        IReadOnlyDictionary<string, string> GetSection(string sectionName);
        
        /// <summary>
        /// Retrieves a config parameter value from a section.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="key">Name of the parameter key.</param>
        /// <returns>Returns the parameter value in string format.</returns>
        string GetParam(string sectionName, string key);
        
        T GetParam<T>(string sectionName, string key);
    }
}