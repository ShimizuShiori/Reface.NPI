using System.Collections.Generic;

namespace Reface.NPI.Generators
{
    public interface ISqlParameterFinder
    {
        IEnumerable<string> Find(string queryCommand);
    }
}
