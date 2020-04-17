using System.Collections.Generic;
using System.Linq;

namespace Reface.NPI.Generators
{
    public class DefaultSqlParameterFinder : ISqlParameterFinder
    {
        private readonly ICache cache;

        public DefaultSqlParameterFinder()
        {
            cache = NpiServicesCollection.GetService<ICache>();
        }

        public IEnumerable<string> Find(string queryCommand)
        {
            string key = string.Format("{0}.{1}-{2}", nameof(DefaultSqlParameterFinder),
                nameof(Find),
                queryCommand);
            return cache.GetOrCreate<IEnumerable<string>>(key, k =>
             {
                 queryCommand = queryCommand.Replace("\r", " ");
                 queryCommand = queryCommand.Replace("\n", " ");
                 queryCommand = queryCommand.Replace("\t", " ");
                 string[] words = queryCommand.Split(new char[] { ' ' });
                 return words.Select(x => x.Trim())
                     .Where(x => x.StartsWith("@"))
                     .Select(x => x.Replace("@", ""));
             });
        }
    }
}
