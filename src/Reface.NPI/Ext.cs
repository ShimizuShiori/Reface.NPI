using System;
using System.Collections.Generic;
using System.Linq;

namespace Reface.NPI
{
    public static class Ext
    {
        public static string Join<T>(this IEnumerable<T> list, string joiner, Func<T, string> map)
        {
            if (!list.Any()) return "";
            if (list.Count() == 1) return map(list.First());
            return list.Select(x => map(x)).Aggregate((a, b) => $"{a}{joiner}{b}");
        }
    }
}
