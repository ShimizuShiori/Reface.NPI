using Reface.NPI.Models;
using Reface.NPI.Parsers.StateMachines;
using Reface.NPI.Parsers.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace Reface.NPI.Parsers
{
    public class DefaultInsertParser : IInsertParser
    {
        private InsertInfo insertInfo;

        public InsertInfo Parse(string command)
        {
            this.insertInfo = new InsertInfo();
            IEnumerable<string> words = command.SplitToWords();
            IEnumerable<InsertToken> tokens = words.Select(x => InsertToken.Create(x));
            var machine = new InsertStateMachine();
            machine.Parsing += Machine_Parsing;
            foreach (var token in tokens)
                machine.Push(token);
            return insertInfo;
        }

        private void Machine_Parsing(object sender, Events.TokenParsingEventArgs<States.InsertParseStates> e)
        {
            var machine = (InsertStateMachine)sender;
            switch (e.NowState)
            {
                case States.InsertParseStates.WithoutField:
                    insertInfo.WithoutFields.Add(machine.TokenStack.Pop().Text);
                    break;
                case States.InsertParseStates.Select:
                    insertInfo.SelectNewRow = true;
                    break;
                case States.InsertParseStates.Start:
                case States.InsertParseStates.Without:
                case States.InsertParseStates.WithoutEnd:
                default:
                    break;
            }
        }
    }
}
