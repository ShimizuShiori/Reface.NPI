using System.Collections.Generic;

namespace Reface.NPI.Parsers
{
    public class TokenStack<TToken, TAction>
        where TToken : IToken<TAction>
    {
        private readonly Stack<TToken> tokens = new Stack<TToken>();

        public void Push(TToken token)
        {
            this.tokens.Push(token);
        }

        public TToken Pop()
        {
            return this.tokens.Pop();
        }

        public IEnumerable<TToken> Tokens
        {
            get
            {
                return this.tokens;
            }
        }

        public override string ToString()
        {
            return this.tokens.Join(",", x => $"[{x.ToString()}]");
        }
    }
}
