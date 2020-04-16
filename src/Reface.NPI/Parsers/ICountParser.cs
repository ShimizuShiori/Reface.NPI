using Reface.NPI.Models;

namespace Reface.NPI.Parsers
{
    public interface ICountParser
    {
        CountInfo Parse(string command);
    }
}
