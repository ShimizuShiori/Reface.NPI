using Reface.NPI.Models;

namespace Reface.NPI.Parsers
{
    public interface IUpdateParser
    {
        UpdateInfo Parse(string command);
    }
}
