using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using OpenGrooves.Data;
using System.ComponentModel;
using StructureMap;

namespace OpenGrooves.Services.Caching
{
    public class UserSettingsCacheDataProvider : ICacheDataProvider
    {
        private Guid _userId;

        public UserSettingsCacheDataProvider(Guid userId)
        {
            this._userId = userId;
        }

        public IDictionary<string, string> GetData()
        {
            // using EF directly, cool? EDIT: No, not cool
            using (var ctx = new OpenGroovesEntities())
            {
                var settings = ctx.UserSettings.Where(s => s.UserId == _userId).ToDictionary(u => u.Key, u => u.Value);

                if (settings != null)
                {
                    return settings;
                }

                return null;
            }
        }

        public void SaveData(IDictionary<string, string> data)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var settings = ctx.UserSettings.ToList().Where(s => data.ContainsKey(s.Key) && s.UserId == _userId);
                settings.ToList().ForEach(s => ctx.UserSettings.DeleteObject(s));

                data.ToList().ForEach(s => ctx.AddToUserSettings(new UserSetting { Key = s.Key, Value = s.Value, UserId = _userId }));

                ctx.SaveChanges();
            }
        }

        public T GetValue<T>(string key)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var setting = ctx.UserSettings.SingleOrDefault(s => s.Key == key && s.UserId == _userId);

                if (setting != null)
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    return (T)converter.ConvertFromInvariantString(setting.Value); 
                }

                return default(T);
            }
        }
    }
}