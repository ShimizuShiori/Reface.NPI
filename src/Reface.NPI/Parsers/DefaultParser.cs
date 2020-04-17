using Reface.NPI.Models;
using Reface.NPI.Parsers.Events;
using Reface.NPI.Parsers.StateMachines;
using System.Collections.Generic;
using System.Linq;

namespace Reface.NPI.Parsers
{
    public abstract class DefaultParser<TCommandInfo, TMachine, TToken, TStates, TAction> : IParser<TCommandInfo>
        where TCommandInfo : ICommandInfo, new()
        where TMachine : DefaultParseStateMachine<TToken, TStates, TAction>, new()
        where TToken : IToken<TAction>
        where TStates : struct
        where TAction : struct
    {
        private TCommandInfo result;

        public TCommandInfo Parse(string command)
        {
            List<string> words = command.SplitToWords();
            result = new TCommandInfo();
            IEnumerable<TToken> tokens = words.Select(x => GetTokenByWord(x));
            TMachine machine = new TMachine();
            machine.Parsing += Machine_Parsing;
            foreach (var token in tokens)
                machine.Push(token);
            return result;
        }

        private void Machine_Parsing(object sender, TokenParsingEventArgs<TStates> e)
        {
            TMachine machine = (TMachine)sender;
            this.OnParsing(ref this.result, machine, e);
        }

        protected abstract TToken GetTokenByWord(string word);
        protected abstract void OnParsing(ref TCommandInfo info, TMachine machine, TokenParsingEventArgs<TStates> e);
    }
}
