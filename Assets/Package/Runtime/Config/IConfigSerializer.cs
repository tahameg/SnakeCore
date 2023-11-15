using System.Collections.Generic;
using System.IO;

namespace TahaCore.Config
{
    internal interface IConfigSerializer
    {
        string Serialize(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> config);
        void Serialize(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> config, Stream outputStream);
    }
}