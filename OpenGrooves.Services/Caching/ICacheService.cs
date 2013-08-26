using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;

namespace OpenGrooves.Services.Caching
{
    [PluginFamily("CacheService")]
    public interface ICacheService
    {
        void Save(string key, string value);
        T Get<T>(string key);
        void SaveToProvider();
        void LoadFromProvider();
        ICacheDataProvider DataProvider { set; }
    }
    
    public interface ICacheDataProvider
    {
        IDictionary<string, string> GetData();
        T GetValue<T>(string key);
        void SaveData(IDictionary<string, string> data);
    }
}
