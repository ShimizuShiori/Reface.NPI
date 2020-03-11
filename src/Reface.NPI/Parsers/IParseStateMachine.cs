using Reface.NPI.Parsers.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reface.NPI.Parsers
{
    public interface IParseStateMachine<TToken, TState, TAction>
        where TToken : IToken<TAction>
    {
        TokenStack<TToken, TAction> TokenStacks { get; }
        Dictionary<string, object> Context { get; }
        event EventHandler<TokenParsingEventArgs<TState>> Parsing;

        void Push(TToken token);
    }
}
