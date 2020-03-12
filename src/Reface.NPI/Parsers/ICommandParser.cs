using Reface.NPI.Models;

namespace Reface.NPI.Parsers
{
    public interface ICommandParser
    {
        ICommandInfo Parse(string command);
    }
}
