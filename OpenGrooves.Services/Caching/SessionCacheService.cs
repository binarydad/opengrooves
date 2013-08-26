using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using StructureMap;

namespace OpenGrooves.Services.Caching
{
    [Pluggable("CacheService")]
    public class SessionCacheService : ICacheService
    {
        private const string _sessKey = "OpenGroovesSessCache";
        private ICacheDataProvider _provider;

        private void CheckCacheLoaded()
        {
            if (HttpContext.Current.Session[_sessKey] == null)
            {
                LoadFromProvider();
            }
        }

        public void Save(string key, string value)
        {
            if (!String.IsNullOrWhiteSpace(key) && !String.IsNullOrWhiteSpace(value))
            {
                CheckCacheLoaded();

                var session = HttpContext.Current.Session[_sessKey] as Dictionary<string, string>;

                if (session == null)
                {
                    session = new Dictionary<string, string>();
                }
                
                session[key] = value;
                HttpContext.Current.Session[_sessKey] = session;
            }
        }

        public T Get<T>(string key)
        {
            CheckCacheLoaded();

            var session = HttpContext.Current.Session[_sessKey] as Dictionary<string, string>;

            if (session != null)
            {
                if (!session.ContainsKey(key))
                {
                    return default(T);
                }

                var value = session[key];

                if (value != null)
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    var retVal = (T)converter.ConvertFromInvariantString(value);
                    return retVal;
                }
                else
                {
                    return _provider.GetValue<T>(key);
                }
            }

            return default(T);
        }

        public ICacheDataProvider DataProvider
        {
            set
            {
                this._provider = value;
            }
        }

        public void SaveToProvider()
        {
            var session = HttpContext.Current.Session[_sessKey] as Dictionary<string, string>;
            _provider.SaveData(session);
        }

        public void LoadFromProvider()
        {
            var data = _provider.GetData();
            HttpContext.Current.Session[_sessKey] = data;
        }
    }
}