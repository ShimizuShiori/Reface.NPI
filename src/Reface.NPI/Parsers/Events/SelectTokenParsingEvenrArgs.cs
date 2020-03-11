using Reface.NPI.Parsers.States;

namespace Reface.NPI.Parsers.Events
{
    public class SelectTokenParsingEvenrArgs : TokenParsingEventArgs<SelectParseStates>
    {
        public SelectTokenParsingEvenrArgs(SelectParseStates fromState, SelectParseStates nowState) : base(fromState, nowState)
        {
        }
    }
}
