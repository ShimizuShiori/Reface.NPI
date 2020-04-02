using Reface.NPI.Models;
using Reface.NPI.Parsers.StateMachines;
using Reface.NPI.Parsers.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace Reface.NPI.Parsers
{
    public class DefaultDeleteParser : IDeleteParser
    {
        private readonly DeleteInfo deleteInfo = new DeleteInfo();

        public DeleteInfo Parse(string command)
        {
            List<string> words = command.SplitToWords();
            IEnumerable<DeleteToken> tokens = words.Select(x => DeleteToken.Create(x));
            DeleteStateMachine deleteStateMachine = new DeleteStateMachine();
            deleteStateMachine.Parsing += DeleteStateMachine_Parsing;
            foreach (var token in tokens)
            {
                deleteStateMachine.Push(token);
            }
            return deleteInfo;
        }

        private void DeleteStateMachine_Parsing(object sender, Events.TokenParsingEventArgs<States.DeleteParseStates> e)
        {
            const string CONTEXT_KEY_CONDITION = "CONDITION";
            DeleteStateMachine machine = sender as DeleteStateMachine;
            switch (e.NowState)
            {
                case States.DeleteParseStates.Start:
                    break;
                case States.DeleteParseStates.Condition:
                    break;
                case States.DeleteParseStates.ConditionField:
                    {
                        ConditionInfo conditionInfo = new ConditionInfo();
                        conditionInfo.Field = machine.TokenStack.Pop().Text;
                        conditionInfo.Parameter = conditionInfo.Field;
                        machine.Context[CONTEXT_KEY_CONDITION] = conditionInfo;
                        deleteInfo.ConditionInfos.Add(conditionInfo);
                    }
                    break;
                case States.DeleteParseStates.ConditionOperator:
                    {
                        ConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as ConditionInfo;
                        conditionInfo.Operators = machine.TokenStack.Pop().Text;
                    }
                    break;
                case States.DeleteParseStates.ConditionParameter:
                    {
                        ConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as ConditionInfo;
                        conditionInfo.Parameter = machine.TokenStack.Pop().Text;
                    }
                    break;
                case States.DeleteParseStates.NextCondition:
                    {
                        ConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as ConditionInfo;
                        DeleteToken token = machine.TokenStack.Pop();
                        conditionInfo.JoinerToNext = token.Action == Actions.DeleteParseActions.And ?
                             ConditionJoiners.And :
                             ConditionJoiners.Or;
                        machine.Context[CONTEXT_KEY_CONDITION] = null;
                    }
                    break;
                case States.DeleteParseStates.End:
                    break;
                default:
                    break;
            }
        }
    }
}
