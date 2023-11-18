using System;
using TahaCore.Config;

namespace TahaCore.DI.ConfigConditions
{
    public class IntValueConfigConditionAttribute : ConfigConditionAttribute
    {
        private readonly string m_section;
        private readonly string m_key;
        private readonly int m_compareValue;
        private readonly NumericValueCompareType m_compareType;

        public IntValueConfigConditionAttribute(string section, string key
            ,int compareValue, NumericValueCompareType compareType)
        {
            m_section = section;
            m_key = key;
            m_compareValue = compareValue;
            m_compareType = compareType;
        }

        public override bool Evaluate(IConfigManager manager)
        {
            try
            {
                int value = manager.GetParam<int>(m_section, m_key);
                return EvaluateInt(value, m_compareValue, m_compareType);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool EvaluateInt(int value, int compareValue, NumericValueCompareType compareType)
        {
            switch (compareType)
            {
                case NumericValueCompareType.Equal:
                    return value == compareValue;
                case NumericValueCompareType.NotEqual:
                    return value != compareValue;
                case NumericValueCompareType.GreaterThan:
                    return value > compareValue;
                case NumericValueCompareType.GreaterThanOrEqual:
                    return value >= compareValue;
                case NumericValueCompareType.LessThan:
                    return value < compareValue;
                case NumericValueCompareType.LessThanOrEqual:
                    return value <= compareValue;
            }

            return false;
        }
    }
}