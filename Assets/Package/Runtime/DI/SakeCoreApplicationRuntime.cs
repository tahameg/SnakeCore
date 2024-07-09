// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SnakeCore.Config;
using SnakeCore.DI.ConfigConditions;
using SnakeCore.Logging;
using SnakeCore.Reflection;
using SnakeCore.Serialization;
using SnakeCore.Serialization.JsonSerialization;
using VContainer;
using VContainer.Unity;
using ILogger = SnakeCore.Logging.ILogger;

namespace SnakeCore.DI
{
    /// <summary>
    /// The main runtime scope of the application. This is the root scope that registers all the application
    /// runtime types. This should be considered the entry point of the application. <br/>
    /// Features: <br/>
    /// - Searches and registers all the types that are decorated with <see cref="ApplicationRuntimeRegistryAttribute"/> <br/>
    /// - Provides <see cref="ConfigConditionAttribute"/> to conditionally register types.<br/>
    /// - Provides Logger functionality. See <see cref="ILogger"/>.
    /// </summary>
    public class SakeCoreApplicationRuntime : LifetimeScope
    {
        internal static SakeCoreApplicationRuntime Instance { get; private set; }
        internal static string AdditionalConfigData;
        private ILogger m_logger;
        private IConfigValueProvider m_configValueProvider;

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
                return;
            }

            Instance = this;
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
            IniConfigDeserializer configDeserializer = new IniConfigDeserializer();
            ISerializationContext serializationContext = new SerializationContext();
            IJsonSerializer jsonSerializer = new NewtonsoftJsonSerializer(serializationContext);
            IParsingProvider parsingProvider = new ParsingProvider(jsonSerializer);
            IniConfigValueProvider configValueProvider = new IniConfigValueProvider(parsingProvider, configDeserializer);

            if (!string.IsNullOrEmpty(AdditionalConfigData))
            {
                configValueProvider.AppendConfig(AdditionalConfigData);
            }
            
            builder.RegisterInstance(configDeserializer).As<IConfigDeserializer>();
            builder.RegisterInstance(configValueProvider).As<IConfigValueProvider>();
            builder.RegisterInstance(serializationContext).As<ISerializationContext>();
            builder.RegisterInstance(jsonSerializer).As<IJsonSerializer>();
            builder.RegisterInstance(parsingProvider).As<IParsingProvider>();
            m_configValueProvider = configValueProvider;
        }
        
        private IEnumerable<RegistrationInfo> GetRegistrationInfos()
        {
            var decoratedTypes 
                = TypeUtility.GetTypes(type => type.IsDefined(typeof(ApplicationRuntimeRegistryAttribute), false));
            
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

        private bool DoesCoverConfigConditions(MemberInfo type)
        {
            var attributes = type.GetCustomAttributes();
            bool metConditions = true;
            foreach (var attribute in attributes)
            {
                if (attribute is ConfigConditionAttribute configCondition)
                {
                    metConditions &= configCondition.Evaluate(m_configValueProvider);
                }
            }
            return metConditions;
        }
    }
}
