﻿using Reface.NPI.Models;
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
                        ConditionInfo conditionInfo = new ConditionInfo();
                        machine.Context[CONTEXT_KEY_CONDITION] = conditionInfo;
                        info.ConditionInfos.Add(conditionInfo);
                    }
                    break;
                case DeleteParseStates.NotCondition:
                    {
                        ConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as ConditionInfo;
                        conditionInfo.IsNot = true;
                    }
                    break;
                case DeleteParseStates.ConditionField:
                    {
                        ConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as
                            ConditionInfo;
                        conditionInfo.Field = machine.TokenStack.Pop().Text;
                        conditionInfo.Parameter = conditionInfo.Field;
                    }
                    break;
                case DeleteParseStates.ConditionOperator:
                    {
                        ConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as ConditionInfo;
                        conditionInfo.Operators = machine.TokenStack.Pop().Text;
                    }
                    break;
                case DeleteParseStates.ConditionParameter:
                    {
                        ConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as ConditionInfo;
                        conditionInfo.Parameter = machine.TokenStack.Pop().Text;
                    }
                    break;
                case DeleteParseStates.NextCondition:
                    {
                        ConditionInfo conditionInfo = machine.Context[CONTEXT_KEY_CONDITION] as ConditionInfo;
                        DeleteToken token = machine.TokenStack.Pop();
                        conditionInfo.JoinerToNext = token.Action == Actions.DeleteParseActions.And ?
                             ConditionJoiners.And :
                             ConditionJoiners.Or;

                        conditionInfo = new ConditionInfo();
                        info.ConditionInfos.Add(conditionInfo);
                        machine.Context[CONTEXT_KEY_CONDITION] = conditionInfo;
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
