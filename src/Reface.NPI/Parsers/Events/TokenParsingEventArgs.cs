using System;

namespace Reface.NPI.Parsers.Events
{
    public class TokenParsingEventArgs<TState> : EventArgs
    {
        public TState FromState { get; private set; }
        public TState NowState { get; private set; }

        public TokenParsingEventArgs(TState fromState, TState nowState)
        {
            FromState = fromState;
            NowState = nowState;
        }

        public override string ToString()
        {
            return $"[{FromState.ToString()}]-->[{NowState.ToString()}]";
        }
    }
}
