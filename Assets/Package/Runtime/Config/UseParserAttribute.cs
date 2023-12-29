using System;
using TahaCore.DI;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEngine;

namespace TahaCore.Config
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UseParserAttribute : Attribute
    {
        public Type ParserType { get; private set; }
        public UseParserAttribute(Type parserType)
        {
            if (!typeof(IParser).IsAssignableFrom(parserType))
            {
                TahaCoreApplicationRuntime.LogError($"Parser type {parserType} is not assignable from {typeof(IParser)}");
                return;
            }
            ParserType = parserType;
        }
    }
}