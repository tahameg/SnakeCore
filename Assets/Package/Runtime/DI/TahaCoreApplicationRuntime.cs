using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TahaCore.Config;
using TahaCore.Config.Types;
using TahaCore.DI.ConfigConditions;
using TahaCore.Logging;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ILogger = TahaCore.Logging.ILogger;

namespace TahaCore.DI
{
    /// <summary>
    /// The main runtime scope of the application. This is the root scope that registers all the application
    /// runtime types. This should be considered the entry point of the application. <br/>
    /// Features: <br/>
    /// - Searches and registers all the types that are decorated with <see cref="ApplicationRuntimeRegistryAttribute"/> <br/>
    /// - Provides <see cref="ConfigConditionAttribute"/> to conditionaly register types.<br/>
    /// - Provides Logger functionality. See <see cref="TahaCore.Logging.ILogger"/>.
    /// </summary>
    public class TahaCoreApplicationRuntime : LifetimeScope
    {
        [SerializeField] private bool doNotDestroyOnLoad = true;
        internal static TahaCoreApplicationRuntime Instance { get; private set; }
        internal static string AdditionalConfigData;
        private ILogger m_logger;
        private IConfigManager m_configManager;

        /// <summary>
        /// Log and error through TahaCore.ILogger.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogError(object message) => Instance.m_logger.LogError(message);
        
        /// <summary>
        /// Log a warning through TahaCore.ILogger.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogWarning(object message) => Instance.m_logger.LogWarning(message);
        
        /// <summary>
        /// Log an info through the TahaCore.ILogger.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogInfo(object message) => Instance.m_logger.LogInfo(message);
        
        protected override void Awake()
        {
            // use a temporary logger until the container is built.
            m_logger = new TahaCoreLogger();
            if (Instance != null)
            {
                Destroy(gameObject);
                LogError("There is already an instance of TahaCoreApplicationRuntime in the scene.");
                return;
            }

            Instance = this;
            if(doNotDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            base.Awake();
            m_logger = Container.Resolve<ILogger>();
        }
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigManager(builder);
            
            foreach (var info in GetRegistrationInfos())
            {
                if (info.SelfRegistration)
                {
                    builder.Register(info.Target, info.LifetimeType.ToLifetime()).AsSelf();
                    continue;
                }
                
                var registrationBuilder = builder.Register(info.Target, info.LifetimeType.ToLifetime());
                foreach (var registrationType in info.RegistrationTypes)
                {
                    registrationBuilder = registrationBuilder.As(registrationType);
                }
            }
        }

        private void RegisterConfigManager(IContainerBuilder builder)
        {
            IniConfigDeserializer deserializer = new IniConfigDeserializer();
            ConfigTypeParser parser = new ConfigTypeParser();
            IniConfigManager configManager = new IniConfigManager(parser, deserializer);
            
            if(!string.IsNullOrEmpty(AdditionalConfigData))
                configManager.AppendConfig(AdditionalConfigData);
            
            builder.RegisterInstance(deserializer).As<IConfigDeserializer>();
            builder.RegisterInstance(configManager).As<IConfigManager>();
            m_configManager = configManager;
        }
        private IEnumerable<RegistrationInfo> GetRegistrationInfos()
        {
            var decoratedTypes 
                = GetTypes(type => type.IsDefined(typeof(ApplicationRuntimeRegistryAttribute), false));
            
            var registrationInfos = new List<RegistrationInfo>();

            foreach (var type in decoratedTypes)
            {
                if(!DoesCoverConfigConditions(type)) continue;
                var attribute = type.GetCustomAttribute<ApplicationRuntimeRegistryAttribute>();
                var info = new RegistrationInfo(type, attribute.LifetimeType, attribute.RegisteredTypes?.ToArray());
                registrationInfos.Add(info);
            }
            
            return registrationInfos;
        }

        private bool DoesCoverConfigConditions(Type type)
        {
            var attributes = type.GetCustomAttributes();
            bool metConditions = true;
            foreach (var attribute in attributes)
            {
                if (attribute is ConfigConditionAttribute configCondition)
                {
                    metConditions &= configCondition.Evaluate(m_configManager);
                }
            }
            return metConditions;
        }
        private IEnumerable<Type> GetTypes(Predicate<Type> predicate)
        {
            var decoratedTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                select type).Where(type => predicate(type));

            return decoratedTypes;
        }
    }
}
