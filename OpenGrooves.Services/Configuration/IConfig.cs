using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;

namespace OpenGrooves.Services.Configuration
{
    [PluginFamily("Config", Scope = InstanceScope.Singleton)]
    public interface IConfig
    {
        T GetSetting<T>(string key);
    }
}
