using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.IO;

namespace CorParques.Transversales.Util
{
    public static class Cache
    {
        private static MemoryCache memCache = MemoryCache.Default;

        public static DateTimeOffset Long = DateTimeOffset.UtcNow.AddHours(10);
        public static DateTimeOffset Medium = DateTimeOffset.UtcNow.AddHours(1);
        public static DateTimeOffset Short = DateTimeOffset.UtcNow.AddMinutes(10);

        public static T GetCache<T>(string name) where T : class
        {
            return memCache.Get(name) as T;
        }
        public static bool SetCache<T>(string name, T data, DateTimeOffset time) where T : class
        {
            return memCache.Add(name, data, time); ;
        }
    }
}
