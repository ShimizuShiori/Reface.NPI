using Reface.NPI.Models;

namespace Reface.NPI.Parsers
{
    public interface IParser<TCommandInfo>
        where TCommandInfo : ICommandInfo, new()
    {
        TCommandInfo Parse(string command);
    }
}
