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
    public class SelectStateMachine : DefaultParseStateMachine<SelectToken, SelectParseStates, SelectParseActions>
    {
        //public Stack<SelectToken> TokenStack { get; private set; }
        //public Dictionary<string, object> Context { get; private set; }

        //private IStateMachine<SelectParseStates, SelectParseActions>
        //    machine;

        //public event EventHandler<SelectTokenParsingEvenrArgs> Parsing;

        //public SelectStateMachine()
        //{
        //    this.TokenStack = new Stack<SelectToken>();
        //    this.Context = new Dictionary<string, object>();
        //    machine = CsvStateMachineBuilder<SelectParseStates, SelectParseActions>
        //        .FromFile(PathProvider.SelectStateMachine).Build();
        //    machine.Pushed += Machine_Pushed;
        //}

        //public void Push(SelectToken token)
        //{
        //    this.TokenStack.Push(token);
        //    this.machine.Push(token.Action);
        //}

        //private void Machine_Pushed(object sender, StateMachine.Events.StateMachinePushedEventArgs<SelectParseStates, SelectParseActions> e)
        //{
        //    this.Parsing?.Invoke(this, new SelectTokenParsingEvenrArgs(e.OldState, e.NewState));
        //}
        public SelectStateMachine() : base(PathProvider.MACHINE_NAME_SELECT)
        {
        }
    }
}
