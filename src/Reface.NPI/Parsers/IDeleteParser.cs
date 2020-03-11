using Reface.NPI.Models;

namespace Reface.NPI.Parsers
{
    public interface IDeleteParser
    {
        DeleteInfo Parse(string command);
    }
}
