using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

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

        public static string ToXml<T>(this T value)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, value);
                byte[] buffer = new byte[memoryStream.Length];
                memoryStream.Position = 0;
                memoryStream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static T ToObjectAsXml<T>(this string xml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(xml);
                memoryStream.Write(buffer, 0, buffer.Length);
                memoryStream.Position = 0;
                return (T)xmlSerializer.Deserialize(memoryStream);
            }
        }
    }
}
