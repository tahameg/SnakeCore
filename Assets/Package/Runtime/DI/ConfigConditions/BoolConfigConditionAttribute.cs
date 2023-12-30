// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using TahaCore.Config;

namespace TahaCore.DI.ConfigConditions
{
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
            return configValueProvider.GetParam<bool>(m_section, m_key) == m_expectedValue;
        }
    }

}