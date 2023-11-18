using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TahaCore.Config;
using TahaCore.Config.Types;
using TahaCore.Logging;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ILogger = TahaCore.Logging.ILogger;

namespace TahaCore.DI
{
    public class TahaCoreApplicationRuntime : LifetimeScope
    {
        internal static TahaCoreApplicationRuntime Instance { get; private set; }
        internal static string AdditionalConfigData;
        private ILogger m_logger;
        private IConfigManager m_configManager;
        public static void LogError(object message) => Instance.m_logger.LogError(message);

        public static void LogWarning(object message) => Instance.m_logger.LogWarning(message);

        public static void LogInfo(object message) => Instance.m_logger.LogInfo(message);
        
        protected override void Awake()
        {
            m_logger = new TahaCoreLogger();
            if (Instance != null)
            {
                Destroy(gameObject);
                LogError("There is already an instance of TahaCoreApplicationRuntime in the scene.");
                return;
            }

            Instance = this;
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
                var attribute = type.GetCustomAttribute<ApplicationRuntimeRegistryAttribute>();
                registrationInfos.Add(
                    new RegistrationInfo(type, attribute.LifetimeType, attribute.RegisteredTypes?.ToArray()));
            }
            
            return registrationInfos;
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
