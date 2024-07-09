// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using SnakeCore.Config;

namespace SnakeCore.DI.ConfigConditions
{
    /// <summary>
    /// Injects a implementation to the DI framework if there is a boolean-parsable config property
    /// exists and it is equal to the expectedValue in the config.ini file.
    /// </summary>
    public class BoolConfigConditionAttribute : ConfigConditionAttribute
    {
        private readonly string m_section;
        private readonly string m_key;
        private readonly bool m_expectedValue;
        
        public BoolConfigConditionAttribute(string section, string key, bool expectedValue)
        {
            m_section = section;
            m_key = key;
            m_expectedValue = expectedValue;
        }
        
        public override bool Evaluate(IConfigValueProvider configValueProvider)
        {
            var section = configValueProvider.GetSection(m_section);
            if(section == null) return false;
            if (!section.ContainsKey(m_key)) return false;
            return configValueProvider.GetParamValue<bool>(m_section, m_key) == m_expectedValue;
        }
    }

}