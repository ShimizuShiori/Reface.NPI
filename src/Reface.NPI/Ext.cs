using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static List<string> SplitToWords(this string text)
        {
            List<string> result = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (var c in text)
            {
                if (!Char.IsUpper(c) || sb.Length == 0)
                {
                    sb.Append(c);
                    continue;
                }

                result.Add(sb.ToString());
                sb.Clear();
                sb.Append(c);
            }
            if (sb.Length != 0)
                result.Add(sb.ToString());
            return result;
        }
    }
}
