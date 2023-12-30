// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;

namespace TahaCore.Config
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ConfigSectionAttribute : Attribute
    {
        public string SectionName { get; private set; }
        public ConfigSectionAttribute(string sectionName)
        {
            SectionName = sectionName;
        }
    }
}
