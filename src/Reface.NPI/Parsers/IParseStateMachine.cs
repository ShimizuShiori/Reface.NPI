using Reface.NPI.Parsers.Events;
using System;
using System.Collections.Generic;

namespace Reface.NPI.Parsers
{
    public interface IParseStateMachine<TToken, TState, TAction>
        where TToken : IToken<TAction>
    {
        TokenStack<TToken, TAction> TokenStack { get; }
        Dictionary<string, object> Context { get; }
        event EventHandler<TokenParsingEventArgs<TState>> Parsing;

        void Push(TToken token);
    }
}
