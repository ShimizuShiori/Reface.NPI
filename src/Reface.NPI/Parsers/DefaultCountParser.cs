using Reface.NPI.Models;
using Reface.NPI.Parsers.Actions;
using Reface.NPI.Parsers.Events;
using Reface.NPI.Parsers.StateMachines;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;

namespace Reface.NPI.Parsers
{
    public class DefaultCountParser : DefaultParser<CountInfo, CountStateMachine, CountToken, CountParseStates, CountParseActions>, ICountParser
    {
        protected override CountToken GetTokenByWord(string word)
        {
            return CountToken.Create(word);
        }

        protected override void OnParsing(ref CountInfo info, CountStateMachine machine, TokenParsingEventArgs<CountParseStates> e)
        {
            const string CONTEXT_KEY_CONDITION = "CONDITION";
            switch (e.NowState)
            {
                case States.CountParseStates.Start:
                    break;
                case States.CountParseStates.Condition:
                    break;
                case States.CountParseStates.ConditionField:
                    {
                        ConditionInfo conditionInfo = new ConditionInfo();
                        conditionInfo.Field = machine.TokenStack.Pop().Text;
                        conditionInfo.Parameter = conditionInfo.Field;
                        machine.Context[CONTEXT_KEY_CONDITION] = conditionInfo;
                        info.ConditionInfos.Add(conditionInfo);
                    }
                    break;
                case States.CountParseStates.ConditionOperator:
                    {
                        ConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as ConditionInfo;
                        conditionInfo.Operators = machine.TokenStack.Pop().Text;
                    }
                    break;
                case States.CountParseStates.ConditionParameter:
                    {
                        ConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as ConditionInfo;
                        conditionInfo.Parameter = machine.TokenStack.Pop().Text;
                    }
                    break;
                case States.CountParseStates.NextCondition:
                    {
                        ConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as ConditionInfo;
                        var token = machine.TokenStack.Pop();
                        conditionInfo.JoinerToNext = token.Action == Actions.CountParseActions.And ?
                             ConditionJoiners.And :
                             ConditionJoiners.Or;
                        machine.Context[CONTEXT_KEY_CONDITION] = null;
                    }
                    break;
                case States.CountParseStates.End:
                    break;
                default:
                    break;
            }
        }
    }
}
