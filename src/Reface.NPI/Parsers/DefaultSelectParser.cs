using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reface.NPI.Models;

namespace Reface.NPI.Parsers
{
    public class DefaultSelectParser : ISelectParser
    {
        private SelectInfo selectInfo;
        private Stack<SelectToken> tokenStack;

        public SelectInfo Parse(string command)
        {
            selectInfo = new SelectInfo();
            tokenStack = new Stack<SelectToken>();
            List<SelectToken> tokens = this.SplitCommandToTokens(command);
            var machine = SelectParseStateMachineBuilder.Default.Build();
            machine.Pushed += Machine_Pushed;

            foreach (var token in tokens)
            {
                tokenStack.Push(token);
                machine.Push(token.Action);
            }
            return selectInfo;
        }

        private void Machine_Pushed(object sender, StateMachine.Events.StateMachinePushedEventArgs<SelectParseStates, SelectParseActions> e)
        {
            string field;
            string opr;
            string joiner;
        }

        public List<SelectToken> SplitCommandToTokens(string command)
        {
            List<SelectToken> result = new List<SelectToken>();
            StringBuilder sb = new StringBuilder();
            foreach (var c in command)
            {
                if (!Char.IsUpper(c))
                {
                    sb.Append(c);
                    continue;
                }

                if (sb.Length == 0)
                {
                    sb.Append(c);
                    continue;
                }

                result.Add(SelectToken.Create(sb.ToString()));
                sb.Clear();
                sb.Append(c);
            }
            result.Add(SelectToken.Create(sb.ToString()));
            return result;
        }
    }
}
