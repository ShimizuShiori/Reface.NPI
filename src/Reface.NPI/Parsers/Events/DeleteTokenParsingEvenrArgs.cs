using Reface.NPI.Parsers.States;
using System;

namespace Reface.NPI.Parsers.Events
{
    public class DeleteTokenParsingEvenrArgs : EventArgs
    {
        public DeleteParseStates FromState { get; private set; }
        public DeleteParseStates NowState { get; private set; }

        public DeleteTokenParsingEvenrArgs(DeleteParseStates fromState, DeleteParseStates nowState)
        {
            FromState = fromState;
            NowState = nowState;
        }

        public override string ToString()
        {
            return $"[{FromState.ToString()}] --> [{NowState.ToString()}]";
        }
    }
}
