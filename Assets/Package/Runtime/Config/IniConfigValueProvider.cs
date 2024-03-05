// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using TahaCore.DI;
using TahaCore.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;
using ConfigCollection = System.Collections.Generic.IReadOnlyDictionary<string
    , System.Collections.Generic.IReadOnlyDictionary<string, string>>;

namespace TahaCore.Config
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
    internal class IniConfigValueProvider : IConfigValueProvider
    {
        /// <summary>
        /// Returns if the config manager is initialized.
        /// </summary>
        public bool IsInitialized => m_isInitialized;
        
        private const string k_configFileName = "config.ini";
        private readonly IConfigDeserializer m_deserializer;
        private readonly IParsingProvider m_parsingProvider;
        private bool m_isInitialized;
        private ConfigCollection m_config;
        private string PersistentDataConfigPath
            => Path.Combine(Application.persistentDataPath, k_configFileName);
        
        [Inject]
        internal IniConfigValueProvider(IParsingProvider parsingProvider, IConfigDeserializer deserializer)
        {
            m_deserializer = deserializer;
            m_parsingProvider = parsingProvider;
            Initialize();
        }
        
        /// <inheritdoc cref="IConfigValueProvider.AppendConfig"/>
        public void AppendConfig(string additionalConfig)
        {
            if (!m_isInitialized)
            {
                TahaCoreApplicationRuntime.LogError("ConfigManager is not initialized");
                return;
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
                TahaCoreApplicationRuntime.LogWarning("ConfigManager is not initialized");
                return null;
            }
            
            if (m_config.TryGetValue(sectionName, out var returnValue))
            {
                return returnValue;
            }
            return null;
        }

        public string GetParam(string sectionName, string key)
        {
            IReadOnlyDictionary<string, string> section = GetSection(sectionName);
            if (section == null) return null;

            if(section.TryGetValue(key, out var returnValue))
            {
                return returnValue;
            }
            return null;
        }
        
        public T GetParamValue<T>(string sectionName, string key)
        {
            string value = GetParam(sectionName, key);
            if (value == null) return default;
            return m_parsingProvider.Parse<T>(value);
        }

        public object GetParamValue(Type type, string sectionName, string key)
        {
            string value = GetParam(sectionName, key);
            if (value == null) return default;
            return m_parsingProvider.Parse(type, value);
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
            return CopyFromStreamingAssetsWeb();
            #endif
        }
        
        private ConfigCollection CopyFromStreamingAssetsFileSystem()
        {
            string fileName = Path.Combine(Application.streamingAssetsPath, k_configFileName);
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
            UnityWebRequest www = UnityWebRequest.Get($"{Application.streamingAssetsPath}/{k_configFileName}");
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