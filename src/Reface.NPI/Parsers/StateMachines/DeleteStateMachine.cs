using Reface.NPI.Parsers.Actions;
using Reface.NPI.Parsers.Events;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;
using Reface.StateMachine;
using Reface.StateMachine.CsvBuilder;
using System;
using System.Collections.Generic;

namespace Reface.NPI.Parsers.StateMachines
{
    public class DeleteStateMachine
    {
        public TokenStack<DeleteToken, DeleteParseActions> TokenStack { get; private set; }
           = new TokenStack<DeleteToken, DeleteParseActions>();

        public event EventHandler<DeleteTokenParsingEvenrArgs> Parsing;

        public Dictionary<string, object> Context { get; private set; } = new Dictionary<string, object>();

        private IStateMachine<DeleteParseStates, DeleteParseActions> machine;

        public DeleteStateMachine()
        {
            machine = CsvStateMachineBuilder<DeleteParseStates, DeleteParseActions>.FromFile(PathProvider.DeleteStateMachine).Build();
            machine.Pushed += Machine_Pushed;
        }

        public void Push(DeleteToken token)
        {
            this.TokenStack.Push(token);
            this.machine.Push(token.Action);
        }

        private void Machine_Pushed(object sender, StateMachine.Events.StateMachinePushedEventArgs<DeleteParseStates, DeleteParseActions> e)
        {
            this.Parsing?.Invoke(this, new DeleteTokenParsingEvenrArgs(e.OldState, e.NewState));
        }
    }
}
