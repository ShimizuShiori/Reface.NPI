using System;

namespace Reface.NPI.Parsers.Events
{
    public class SelectTokenParsingEvenrArgs : EventArgs
    {
        public SelectParseStates FromState { get; private set; }
        public SelectParseStates NowState { get; private set; }

        public SelectTokenParsingEvenrArgs(SelectParseStates fromState, SelectParseStates nowState)
        {
            FromState = fromState;
            NowState = nowState;
        }
    }
}
