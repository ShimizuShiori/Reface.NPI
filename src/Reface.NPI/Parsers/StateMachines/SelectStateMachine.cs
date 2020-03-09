using Reface.NPI.Parsers.Events;
using Reface.StateMachine;
using Reface.StateMachine.CsvBuilder;
using System;
using System.Collections.Generic;

namespace Reface.NPI.Parsers.StateMachines
{
    public class SelectStateMachine
    {
        public Stack<SelectToken> TokenStacks { get; private set; }
        public Dictionary<string, object> Context { get; private set; }

        private IStateMachine<SelectParseStates, SelectParseActions>
            machine;

        public event EventHandler<SelectTokenParsingEvenrArgs> Parsing;

        public SelectStateMachine()
        {
            this.TokenStacks = new Stack<SelectToken>();
            this.Context = new Dictionary<string, object>();
            machine = CsvStateMachineBuilder<SelectParseStates, SelectParseActions>
                .FromFile("./resources/NPI.csv").Build();
            machine.Pushed += Machine_Pushed;
        }

        public void Push(SelectToken token)
        {
            this.TokenStacks.Push(token);
            this.machine.Push(token.Action);
        }

        private void Machine_Pushed(object sender, StateMachine.Events.StateMachinePushedEventArgs<SelectParseStates, SelectParseActions> e)
        {
            this.Parsing?.Invoke(this, new SelectTokenParsingEvenrArgs(e.OldState, e.NewState));
        }
    }
}
