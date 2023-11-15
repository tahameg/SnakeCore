using System.Collections.Generic;
using System.IO;

namespace TahaCore.Config
{
    internal interface IConfigDeserializer
    {
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Deserialize(Stream config);
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Deserialize(string config);
    }
}