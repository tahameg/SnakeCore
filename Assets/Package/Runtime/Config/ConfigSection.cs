using System.Collections;
using System.Reflection;
using TahaCore.DI;
using TahaCore.Runtime.DI;
using TahaCore.Serialization;
using UnityEngine;
using VContainer;

namespace TahaCore.Config
{
    public abstract class ConfigSection
    {
        protected ConfigSection()
        {
            if (TahaCoreApplicationRuntime.Instance == null)
            {
                TahaCoreApplicationRuntime.LogError("TahaCoreApplicationRuntime is not initialized");
                return;
            }
            var type = GetType();
            var typeParser = TahaCoreApplicationRuntime.Instance.Container.Resolve<ITypeParsingProvider>();
            var configValueProvider = TahaCoreApplicationRuntime.Instance.Container.Resolve<IConfigValueProvider>();
            
            var sectionAttribute = GetType().GetCustomAttribute<ConfigSectionAttribute>();
            
            var sectionName = sectionAttribute == null ? GetType().Name : sectionAttribute.SectionName;
            var section = configValueProvider.GetSection(sectionName);
            if(section == null) return;
            
            var properties = type
                .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                ConfigPropertyAttribute attribute = property.GetCustomAttribute<ConfigPropertyAttribute>();
                if(attribute == null || !property.CanWrite) continue;
                string propertyName = attribute.PropertyName;
               
                if (!section.TryGetValue(propertyName, out var propertyStringValue))
                {
                    TahaCoreApplicationRuntime.LogWarning($"No config value found for {sectionName}.{propertyName}");
                    continue;
                }
                
                var propertyValue = typeParser.Parse(property.PropertyType, propertyStringValue);
                property.SetValue(this, propertyValue);
            }
        }
    }
}
