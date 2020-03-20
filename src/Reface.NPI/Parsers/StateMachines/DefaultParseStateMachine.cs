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
        where TState : struct
        where TAction : struct
    {
        private static readonly IStateMachineBuilderFactory stateMachineBuilderFactory;

        static DefaultParseStateMachine()
        {
            stateMachineBuilderFactory = NpiServicesCollection.GetService<IStateMachineBuilderFactory>();
        }

        public TokenStack<TToken, TAction> TokenStack { get; private set; }

        public Dictionary<string, object> Context { get; private set; }

        public event EventHandler<TokenParsingEventArgs<TState>> Parsing;

        private readonly IStateMachine<TState, TAction> machine;


        public DefaultParseStateMachine(string stateMachineName)
        {

            this.TokenStack = new TokenStack<TToken, TAction>();
            this.Context = new Dictionary<string, object>();
            machine = stateMachineBuilderFactory.Create<TState, TAction>(stateMachineName).Build();
            machine.Pushed += Machine_Pushed;
        }

        private void Machine_Pushed(object sender, StateMachine.Events.StateMachinePushedEventArgs<TState, TAction> e)
        {
            this.Parsing?.Invoke(this, new TokenParsingEventArgs<TState>(e.OldState, e.NewState));
        }

        public void Push(TToken token)
        {
            this.TokenStack.Push(token);
            this.machine.Push(token.Action);
        }
    }
}
