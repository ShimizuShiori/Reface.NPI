using Reface.NPI.Parsers.States;

namespace Reface.NPI.Parsers.Events
{
    public class DeleteTokenParsingEvenrArgs : TokenParsingEventArgs<DeleteParseStates>
    {
        public DeleteTokenParsingEvenrArgs(DeleteParseStates fromState, DeleteParseStates nowState)
            : base(fromState, nowState)
        {
        }
    }
}
