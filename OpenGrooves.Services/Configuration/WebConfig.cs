using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using StructureMap;

namespace OpenGrooves.Services.Configuration
{
    [Pluggable("Config")]
    public class WebConfig : IConfig
    {
        public T GetSetting<T>(string key)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));

            var value = System.Configuration.ConfigurationManager.AppSettings[key];

            return (T)converter.ConvertFromInvariantString(value);
        }
    }
}