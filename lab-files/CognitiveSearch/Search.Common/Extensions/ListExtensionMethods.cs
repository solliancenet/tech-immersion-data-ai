using Search.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace Search.Common.Extensions
{
    public static class ListExtensionMethods
    {
        public static T Get<T>(this IList<T> list, string name) where T: INamed
        {
            if (list == null)
            {
                return default(T);
            }
            return list.FirstOrDefault(i => i.Name == name);
        }

        public static bool Exists<T>(this IList<T> list, string name) where T: INamed
        {
            if (list == null)
            {
                return false;
            }
            return list.Any(i => i.Name == name);
        }
    }
}