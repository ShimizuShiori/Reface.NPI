using Reface.NPI.Models;

namespace Reface.NPI.Parsers
{
    public interface ISelectParser
    {
        SelectInfo Parse(string command);
    }
}
