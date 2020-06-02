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
                        FieldConditionInfo condition = new FieldConditionInfo();
                        info.Condition = condition;
                        machine.Context[CONTEXT_KEY_CONDITION] = condition;
                    }
                    break;
                case CountParseStates.NotCondition:
                    {
                        FieldConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as FieldConditionInfo;
                        conditionInfo.IsNot = true;
                    }
                    break;
                case States.CountParseStates.ConditionField:
                    {
                        FieldConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as FieldConditionInfo;
                        conditionInfo.Field = machine.TokenStack.Pop().Text;
                        conditionInfo.Parameter = conditionInfo.Field;
                    }
                    break;
                case States.CountParseStates.ConditionOperator:
                    {
                        FieldConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as FieldConditionInfo;
                        conditionInfo.Operators = machine.TokenStack.Pop().Text;
                    }
                    break;
                case States.CountParseStates.ConditionParameter:
                    {
                        FieldConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as FieldConditionInfo;
                        conditionInfo.Parameter = machine.TokenStack.Pop().Text;
                    }
                    break;
                case States.CountParseStates.NextCondition:
                    {
                        IConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as IConditionInfo;
                        var token = machine.TokenStack.Pop();

                        ConditionJoiners joiner = token.Action == CountParseActions.And ? ConditionJoiners.And : ConditionJoiners.Or;

                        FieldConditionInfo nextCondition = new FieldConditionInfo();

                        info.Condition = new GroupConditionInfo()
                        {
                            LeftCondition = info.Condition,
                            Joiner = joiner,
                            RightCondition = nextCondition
                        };

                        machine.Context[CONTEXT_KEY_CONDITION] = nextCondition;
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
