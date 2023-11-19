﻿using TahaCore.Runtime.Config;

namespace TahaCore.Runtime.DI.ConfigConditions
{
    public class KeyExistenceConfigConditionAttribute : ConfigConditionAttribute
    {
        private readonly string m_section;
        private readonly string m_key;
        private readonly ExistenceType m_existenceType;
        
        public KeyExistenceConfigConditionAttribute(string section, string key, ExistenceType existenceType)
        {
            m_section = section;
            m_key = key;
            m_existenceType = existenceType;
        }

        public override bool Evaluate(IConfigManager manager)
        {
            var section = manager.GetSection(m_section);
            if (section == null) return false == (m_existenceType == ExistenceType.Exists);
            if (!section.ContainsKey(m_key)) return false == (m_existenceType == ExistenceType.Exists);
            return m_existenceType == ExistenceType.Exists;
        }
    }
}