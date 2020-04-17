using Reface.NPI.Models;
using Reface.NPI.Parsers.Actions;
using Reface.NPI.Parsers.Events;
using Reface.NPI.Parsers.StateMachines;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;

namespace Reface.NPI.Parsers
{
    public class DefaultInsertParser : DefaultParser<InsertInfo, InsertStateMachine, InsertToken, InsertParseStates, InsertParseActions>, IInsertParser
    {

        protected override InsertToken GetTokenByWord(string word)
        {
            return InsertToken.Create(word);
        }

        protected override void OnParsing(ref InsertInfo info, InsertStateMachine machine, TokenParsingEventArgs<InsertParseStates> e)
        {
            switch (e.NowState)
            {
                case States.InsertParseStates.WithoutField:
                    info.WithoutFields.Add(machine.TokenStack.Pop().Text);
                    break;
                case States.InsertParseStates.Select:
                    info.SelectNewRow = true;
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
