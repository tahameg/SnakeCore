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
using VContainer;
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
    public class SnakeCoreApplicationRuntime : Runtime
    {
        internal static SnakeCoreApplicationRuntime Instance { get; private set; }
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
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            // use a temporary logger until the container is built.
            m_logger = new SnakeCoreLogger();
            Instance = this;
            DontDestroyOnLoad(this);
            base.Awake();
            m_logger = Container.Resolve<ILogger>();
        }
        
        protected override IEnumerable<RegistrationInfo> GetRegistrationInfos()
        {
            var decoratedTypes 
                = TypeUtility.GetTypes(type => type.IsDefined(typeof(ApplicationRuntimeRegistryAttribute), false));
            
            var registrationInfos = new List<RegistrationInfo>();

            foreach (var type in decoratedTypes)
            {
                if(!DoesCoverConfigConditions(type)) continue;
                var attribute = type.GetCustomAttribute<ApplicationRuntimeRegistryAttribute>();
                bool isEntryPoint = type.IsDefined(typeof(EntryPointRegistryAttribute), true);
                var info = new RegistrationInfo(type, attribute.LifetimeType.ToLifetime(), isEntryPoint, attribute.RegisteredTypes?.ToArray());
                registrationInfos.Add(info);
            }
            
            return registrationInfos;
        }
    }
}
