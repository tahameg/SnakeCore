// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using TahaCore.DI;
using TahaCore.Serialization;
using VContainer;

namespace TahaCore.Config
{
    public abstract class ConfigSection
    {
        private readonly string m_sectionName;
        private readonly IReadOnlyDictionary<string, string> m_section;
        private readonly ITypeParsingProvider m_typeParser;

        protected ConfigSection()
        {
            if (TahaCoreApplicationRuntime.Instance == null)
            {
                TahaCoreApplicationRuntime.LogError("TahaCoreApplicationRuntime is not initialized");
                return;
            }
            var type = GetType();
            m_typeParser = TahaCoreApplicationRuntime.Instance.Container.Resolve<ITypeParsingProvider>();
            var configValueProvider = TahaCoreApplicationRuntime.Instance.Container.Resolve<IConfigValueProvider>();
            
            var sectionAttribute = GetType().GetCustomAttribute<ConfigSectionAttribute>();
            
            m_sectionName = sectionAttribute == null ? GetType().Name : sectionAttribute.SectionName;
            m_section = configValueProvider.GetSection(m_sectionName);
            if (m_section == null)
            {
                TahaCoreApplicationRuntime.LogWarning($"No config section found for {m_sectionName} in the config " +
                                                      $"file. Properties of the section {type.Name} will be invalid.");
                return;
            }
            
            var properties = type
                .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                ParseAndSet(property);
            }
        }

        private void ParseAndSet(PropertyInfo property)
        {
            ConfigPropertyAttribute attribute = property.GetCustomAttribute<ConfigPropertyAttribute>();
            UseParserAttribute parserAttribute = property.GetCustomAttribute<UseParserAttribute>();
            if (attribute == null || !property.CanWrite)
            {
                return;
            }
            string propertyName = attribute.PropertyName;
               
            if (!m_section.TryGetValue(propertyName, out var propertyStringValue))
            {
                TahaCoreApplicationRuntime.LogWarning($"No config value found for {m_sectionName}.{propertyName}");
                return;
            }
            
            if (parserAttribute != null)
            {
                ParseWithParserAndSetValue(property, propertyStringValue, parserAttribute.ParserType);
                return;
            }
            
            var propertyValue = m_typeParser.Parse(property.PropertyType, propertyStringValue);
            property.SetValue(this, propertyValue);
        }
        
        private void ParseWithParserAndSetValue(PropertyInfo property, string propertyStringValue, Type parserType)
        {
            var parser = (ITypeParser) Activator.CreateInstance(parserType);
            var propertyValue = parser.Parse(propertyStringValue);
            property.SetValue(this, propertyValue);
        }
    }
}
