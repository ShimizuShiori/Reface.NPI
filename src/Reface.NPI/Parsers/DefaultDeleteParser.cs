using Reface.NPI.Models;
using Reface.NPI.Parsers.Actions;
using Reface.NPI.Parsers.Events;
using Reface.NPI.Parsers.StateMachines;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;

namespace Reface.NPI.Parsers
{
    public class DefaultDeleteParser : DefaultParser<DeleteInfo, DeleteStateMachine, DeleteToken, DeleteParseStates, DeleteParseActions>, IDeleteParser
    {

        protected override DeleteToken GetTokenByWord(string word)
        {
            return DeleteToken.Create(word);
        }

        protected override void OnParsing(ref DeleteInfo info, DeleteStateMachine machine, TokenParsingEventArgs<DeleteParseStates> e)
        {
            const string CONTEXT_KEY_CONDITION = "CONDITION";
            switch (e.NowState)
            {
                case DeleteParseStates.Start:
                    break;
                case DeleteParseStates.Condition:
                    {
                        FieldConditionInfo conditionInfo = new FieldConditionInfo();
                        machine.Context[CONTEXT_KEY_CONDITION] = conditionInfo;
                        info.Condition = conditionInfo;
                    }
                    break;
                case DeleteParseStates.NotCondition:
                    {
                        FieldConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as FieldConditionInfo;
                        conditionInfo.IsNot = true;
                    }
                    break;
                case DeleteParseStates.ConditionField:
                    {
                        FieldConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as
                            FieldConditionInfo;
                        conditionInfo.Field = machine.TokenStack.Pop().Text;
                        conditionInfo.Parameter = conditionInfo.Field;
                    }
                    break;
                case DeleteParseStates.ConditionOperator:
                    {
                        FieldConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as FieldConditionInfo;
                        conditionInfo.Operators = machine.TokenStack.Pop().Text;
                    }
                    break;
                case DeleteParseStates.ConditionParameter:
                    {
                        FieldConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as FieldConditionInfo;
                        conditionInfo.Parameter = machine.TokenStack.Pop().Text;
                    }
                    break;
                case DeleteParseStates.NextCondition:
                    {
                        DeleteToken token = machine.TokenStack.Pop();
                        ConditionJoiners joiner = token.Action == Actions.DeleteParseActions.And ?
                             ConditionJoiners.And :
                             ConditionJoiners.Or;

                        FieldConditionInfo nextCondition = new FieldConditionInfo();
                        machine.Context[CONTEXT_KEY_CONDITION] = nextCondition;

                        info.Condition = new GroupConditionInfo()
                        {
                            LeftCondition = info.Condition,
                            Joiner = joiner,
                            RightCondition = nextCondition
                        };
                    }
                    break;
                case DeleteParseStates.End:
                    break;
                default:
                    break;
            }
        }
    }
}
