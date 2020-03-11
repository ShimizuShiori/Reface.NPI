using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reface.NPI.Parsers.Events;
using Reface.StateMachine;
using Reface.StateMachine.CsvBuilder;

namespace Reface.NPI.Parsers.StateMachines
{
    public class DefaultParseStateMachine<TToken, TState, TAction> : IParseStateMachine<TToken, TState, TAction>
        where TToken : IToken<TAction>
    {
        public TokenStack<TToken, TAction> TokenStacks { get; private set; }

        public Dictionary<string, object> Context { get; private set; }

        public event EventHandler<TokenParsingEventArgs<TState>> Parsing;

        private readonly IStateMachine<TState, TAction> machine;

        public DefaultParseStateMachine(string stateMachineName)
        {
            machine = CsvStateMachineBuilder<TState, TAction>.FromFile(PathProvider.GetStateMachine(stateMachineName)).Build();
            machine.Pushed += Machine_Pushed;
        }

        private void Machine_Pushed(object sender, StateMachine.Events.StateMachinePushedEventArgs<TState, TAction> e)
        {
            this.Parsing?.Invoke(this, new TokenParsingEventArgs<TState>(e.OldState, e.NewState));
        }

        public void Push(TToken token)
        {
            this.TokenStacks.Push(token);
            this.machine.Push(token.Action);
        }
    }
}
