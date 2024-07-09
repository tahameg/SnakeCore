// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using SnakeCore.Config;

namespace SnakeCore.DI.ConfigConditions
{
    /// <summary>
    /// Injects a concrete implementation if the given property name in the given property section
    /// meets the existence criteria
    /// </summary>
    public class KeyExistenceConfigConditionAttribute : ConfigConditionAttribute
    {
        private readonly string m_section;
        private readonly string m_key;
        private readonly ExistenceCompareType m_existenceCompareType;
        
        public KeyExistenceConfigConditionAttribute(string section, string key, ExistenceCompareType existenceCompareType)
        {
            m_section = section;
            m_key = key;
            m_existenceCompareType = existenceCompareType;
        }

        public override bool Evaluate(IConfigValueProvider configValueProvider)
        {
            var section = configValueProvider.GetSection(m_section);
            if (section == null) return false == (m_existenceCompareType == ExistenceCompareType.Exists);
            if (!section.ContainsKey(m_key)) return false == (m_existenceCompareType == ExistenceCompareType.Exists);
            return m_existenceCompareType == ExistenceCompareType.Exists;
        }
    }
}