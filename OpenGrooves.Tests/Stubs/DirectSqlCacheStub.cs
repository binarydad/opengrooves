using OpenGrooves.Services.Caching;
using System.Collections.Generic;

namespace OpenGrooves.Tests.Stubs
{
    public class DirectSqlCacheStub : ICacheService
    {
        private IDictionary<string, string> _data;
        private ICacheDataProvider _provider;

        public void Save(string key, string value)
        {
            _provider.SaveData(_data);
        }

        public T Get<T>(string key)
        {
            return _provider.GetValue<T>(key);
        }

        public void SaveToProvider()
        {
            _provider.SaveData(_data);
        }

        public void LoadFromProvider()
        {
            _data = _provider.GetData();
        }

        public ICacheDataProvider DataProvider { set { this._provider = value; } }
    }
}
