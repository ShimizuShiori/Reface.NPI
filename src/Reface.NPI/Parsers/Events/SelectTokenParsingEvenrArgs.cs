using System;

namespace Reface.NPI.Parsers.Events
{
    public class SelectTokenParsingEvenrArgs : EventArgs
    {
        public SelectParseStates State { get; private set; }

        public SelectTokenParsingEvenrArgs(SelectParseStates state)
        {
            State = state;
        }
    }
}
