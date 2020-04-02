using Reface.NPI.Models;

namespace Reface.NPI.Parsers
{
    public interface IInsertParser
    {
        InsertInfo Parse(string command);
    }
}
