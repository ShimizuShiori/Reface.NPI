using Reface.NPI.Models;
using Reface.NPI.Parsers.Actions;
using Reface.NPI.Parsers.Events;
using Reface.NPI.Parsers.StateMachines;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;
using System.Net;
using System.Runtime.Remoting.Contexts;

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
                    {
                        ConditionInfo condition = new ConditionInfo();
                        info.ConditionInfos.Add(condition);
                        machine.Context[CONTEXT_KEY_CONDITION] = condition;
                    }
                    break;
                case CountParseStates.NotCondition:
                    {
                        ConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as ConditionInfo;
                        conditionInfo.IsNot = true;
                    }
                    break;
                case States.CountParseStates.ConditionField:
                    {
                        ConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as ConditionInfo;
                        conditionInfo.Field = machine.TokenStack.Pop().Text;
                        conditionInfo.Parameter = conditionInfo.Field;
                        machine.Context[CONTEXT_KEY_CONDITION] = conditionInfo;
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

                        conditionInfo = new ConditionInfo();
                        machine.Context[CONTEXT_KEY_CONDITION] = conditionInfo;
                        info.ConditionInfos.Add(conditionInfo);
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
