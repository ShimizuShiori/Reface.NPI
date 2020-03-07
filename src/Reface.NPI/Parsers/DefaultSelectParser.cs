using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reface.NPI.Models;

namespace Reface.NPI.Parsers
{
    public class DefaultSelectParser : ISelectParser
    {


        public SelectInfo Parse(string command)
        {
            List<SelectToken> tokens = this.SplitCommandToTokens(command);

            throw new NotImplementedException();
        }

        public List<SelectToken> SplitCommandToTokens(string command)
        {
            List<SelectToken> result = new List<SelectToken>();
            StringBuilder sb = new StringBuilder();
            foreach (var c in command)
            {
                if (!Char.IsUpper(c))
                {
                    sb.Append(c);
                    continue;
                }

                if (sb.Length == 0)
                {
                    sb.Append(c);
                    continue;
                }

                result.Add(SelectToken.Create(sb.ToString()));
                sb.Clear();
                sb.Append(c);
            }
            result.Add(SelectToken.Create(sb.ToString()));
            return result;
        }
    }
}
