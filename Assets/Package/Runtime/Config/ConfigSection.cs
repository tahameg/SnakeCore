// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using TahaCore.DI;
using TahaCore.Serialization.TypeParsers;
using UnityEngine;
using VContainer;

namespace TahaCore.Config
{
    /// <summary>
    /// Base class for ConfigSection Dtos. This Implementation automatically populates the classes that
    /// inherit this base class with values that are given in the config.ini file.
    /// For custom section names <see cref="ConfigSectionAttribute"/>
    /// Name of the config properties are found from <see cref="ConfigPropertyAttribute"/> 
    /// </summary>
    public abstract class ConfigSection
    {
        private readonly string m_sectionName;
        private readonly IReadOnlyDictionary<string, string> m_section;
        private readonly IConfigValueProvider m_configValueProvider;

        protected ConfigSection()
        {
            if (TahaCoreApplicationRuntime.Instance == null)
            {
                TahaCoreApplicationRuntime.LogError("TahaCoreApplicationRuntime is not initialized");
                return;
            }
            var type = GetType();
            m_configValueProvider = TahaCoreApplicationRuntime.Instance.Container.Resolve<IConfigValueProvider>();

            var sectionAttribute = GetType().GetCustomAttribute<ConfigSectionAttribute>();
            
            m_sectionName = sectionAttribute == null ? GetType().Name : sectionAttribute.SectionName;
            m_section = m_configValueProvider.GetSection(m_sectionName);
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
            ParseWithAttribute deserializerAttribute = property.GetCustomAttribute<ParseWithAttribute>();
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
            
            if (deserializerAttribute != null)
            {
                ParseWithParserAndSetValue(property, propertyStringValue, deserializerAttribute.ParserType);
                return;
            }
            
            var propertyValue = m_configValueProvider.GetParamValue(property.PropertyType, m_sectionName, propertyName);
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
