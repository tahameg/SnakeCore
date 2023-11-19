using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using TahaCore.Runtime.DI;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;
using ConfigCollection = System.Collections.Generic.IReadOnlyDictionary<string
    , System.Collections.Generic.IReadOnlyDictionary<string, string>>;

namespace TahaCore.Runtime.Config
{
    /// <summary>
    /// Manages IniConfig files.<br/>
    /// - config.ini file is expected to be found in the root of the streaming assets folder.<br/>
    /// - If not found, the config will be looked for in the persistent data path.<br/>
    /// - If not found, the config will be created in the persistent data path.<br/>
    /// - If it is found in the streaming assets folder but not in the persistent data path, it will be copied to the
    /// persistent data path.<br/>
    /// - If it is found in the both places the persistent data path config will be used.<br/>
    /// - If it is found in the persistent data path but not in the streaming assets folder, persistent data path config will
    /// be used.<br/>
    /// </summary>
    internal class IniConfigManager : IConfigManager
    {
        /// <summary>
        /// Returns if the config manager is initialized.
        /// </summary>
        public bool IsInitialized => m_isInitialized;
        
        private const string CONFIG_FILE_NAME = "config.ini";
        private readonly IConfigDeserializer m_deserializer;

        private bool m_isInitialized;
        private ConfigCollection m_config;
        private IConfigTypeParser m_parser;
        private string PersistentDataConfigPath
            => Path.Combine(Application.persistentDataPath, CONFIG_FILE_NAME);
        
        
        [Inject]
        internal IniConfigManager(IConfigTypeParser parser, IConfigDeserializer deserializer)
        {
            m_deserializer = deserializer;
            m_parser = parser;
            Initialize();
        }
        
        /// <inheritdoc cref="IConfigManager.AppendConfig"/>
        public void AppendConfig(string additionalConfig)
        {
            if (!m_isInitialized)
            {
                TahaCoreApplicationRuntime.LogError("ConfigManager is not initialized");
            }
            
            var additionalConfigCollection = m_deserializer.Deserialize(additionalConfig);
            var newConfig = new Dictionary<string, IReadOnlyDictionary<string, string>>();
            foreach (var section in m_config)
            {
                var newEntry = new Dictionary<string, string>();
                foreach (var keyValuePair in section.Value)
                {
                    newEntry[keyValuePair.Key] = keyValuePair.Value;
                }

                foreach (var additionalSection in additionalConfigCollection)
                {
                    if(additionalSection.Key != section.Key) continue;
                    foreach (var keyValuePair in additionalSection.Value)
                    {
                        newEntry[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
                newConfig[section.Key] = newEntry;
            }
            
            foreach (var additionalSection in additionalConfigCollection)
            {
                var newEntry = new Dictionary<string, string>();
                foreach (var keyValuePair in additionalSection.Value)
                {
                    newEntry[keyValuePair.Key] = keyValuePair.Value;
                }
                newConfig[additionalSection.Key] = newEntry;
            }
            m_config = newConfig;
        }

        public IReadOnlyDictionary<string, string> GetSection(string sectionName)
        {
            if (!m_isInitialized)
            {
                TahaCoreApplicationRuntime.LogError("ConfigManager is not initialized");
            }

            return m_config.TryGetValue(sectionName, out var returnValue) ? returnValue : null;
        }

        public string GetParam(string sectionName, string key)
        {
            if (!m_isInitialized)
            {
                TahaCoreApplicationRuntime.LogError("ConfigManager is not initialized");
            }
            IReadOnlyDictionary<string, string> section = GetSection(sectionName);
            if(section == null) return null;
            return section.TryGetValue(key, out var returnValue) ? returnValue : null;
        }
        
        /// <summary>
        /// Gets the value of the given key in the given section and parses it to the given type.<br/>
        /// Supported types: <br/>
        /// int,<br/>
        /// float,<br/>
        /// double,<br/>
        /// bool,<br/>
        /// int[],<br/>
        /// float[],<br/>
        /// double[],<br/>
        /// Vector2,<br/>
        /// Vector3,<br/>
        /// Vector4,<br/>
        /// Quaternion,<br/>
        /// string,<br/>
        /// DateTime,<br/>
        /// Guid<br/>
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <returns>Parsed value.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the given type is not supported.</exception>
        public T GetParam<T>(string sectionName, string key)
        {
            string value = GetParam(sectionName, key);
            if (value == null) return default;
            return m_parser.Parse<T>(GetParam(sectionName, key));
        }
        
        private void Initialize()
        {
            if (!File.Exists(PersistentDataConfigPath))
            {
                m_config = CopyFromStreamingAssets();
                m_isInitialized = true;
                return;
            }

            using FileStream fileStream = new FileStream(PersistentDataConfigPath, FileMode.Open);
            m_config = m_deserializer.Deserialize(fileStream);
            m_isInitialized = true;
        }
        
        private ConfigCollection CopyFromStreamingAssets()
        {
            #if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX
            return CopyFromStreamingAssetsFileSystem();
            #elif UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL
            return await CopyFromStreamingAssetsWeb();
            #endif
        }
        
        private ConfigCollection CopyFromStreamingAssetsFileSystem()
        {
            string fileName = Path.Combine(Application.streamingAssetsPath, CONFIG_FILE_NAME);
            if(File.Exists(fileName))
            {
                File.Copy(fileName, PersistentDataConfigPath, true);
                return m_deserializer.Deserialize(File.OpenRead(PersistentDataConfigPath));
            }
            TahaCoreApplicationRuntime.LogWarning("Config file not found in streaming assets. Creating empty config.");
            return CreateConfig();
        }
        
        private ConfigCollection CopyFromStreamingAssetsWeb()
        {
            UnityWebRequest www = UnityWebRequest.Get($"{Application.streamingAssetsPath}/{CONFIG_FILE_NAME}");
            www.timeout = 3; // This is absolutely expected to take less than 3 seconds in a local environment.
            var result = www.SendWebRequest().GetAwaiter().GetResult();
            if (result.result == UnityWebRequest.Result.Success)
            { 
                string data = www.downloadHandler.text;
                var configToReturn = m_deserializer.Deserialize(data);
                File.WriteAllTextAsync(PersistentDataConfigPath, data);
                return configToReturn;
            }
            TahaCoreApplicationRuntime.LogWarning("Config file not found in streaming assets. Creating empty config.");
            return CreateConfig();
        }
        
        private ConfigCollection CreateConfig()
        {
            File.WriteAllText(PersistentDataConfigPath, "");
            return new Dictionary<string, IReadOnlyDictionary<string, string>>();
        }
    }
}