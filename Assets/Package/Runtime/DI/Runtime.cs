using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SnakeCore.Config;
using SnakeCore.DI.ConfigConditions;
using SnakeCore.Logging;
using SnakeCore.Reflection;
using SnakeCore.Serialization;
using SnakeCore.Serialization.JsonSerialization;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SnakeCore.DI
{
    public abstract class Runtime : LifetimeScope
    {
        internal static string AdditionalConfigData;
        
        protected IConfigValueProvider ConfigValueProvider;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigManager(builder);
            EntryPointsBuilder.EnsureDispatcherRegistered(builder);
            var registrationInfos = GetRegistrationInfos();
            
            
            foreach (var info in registrationInfos)
            {
                if (info.IsEntryPoint)
                {
                    var entryPointInterfaces = info.EntryPointInterfaces.ToList();
                    if (info.SelfRegistration)
                    {
                        entryPointInterfaces.Add(info.Target);
                        builder.Register(info.Target, info.Lifetime).As(entryPointInterfaces.ToArray());
                        continue;
                    }
                    builder.Register(info.Target, info.Lifetime).As(entryPointInterfaces.ToArray());
                    
                }
                else if (info.SelfRegistration)
                {
                    builder.Register(info.Target, info.Lifetime).AsSelf();
                    continue;
                }
                
                builder.Register(info.Target, info.Lifetime).As(info.RegistrationTypes.ToArray());
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
            ConfigValueProvider = configValueProvider;
        }

        protected abstract IEnumerable<RegistrationInfo> GetRegistrationInfos();
        

        protected bool DoesCoverConfigConditions(MemberInfo type)
        {
            var attributes = type.GetCustomAttributes();
            bool metConditions = true;
            foreach (var attribute in attributes)
            {
                if (attribute is ConfigConditionAttribute configCondition)
                {
                    metConditions &= configCondition.Evaluate(ConfigValueProvider);
                }
            }
            return metConditions;
        }
    }
}