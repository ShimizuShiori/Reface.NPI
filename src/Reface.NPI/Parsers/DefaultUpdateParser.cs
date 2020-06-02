﻿using Reface.NPI.Models;
using Reface.NPI.Parsers.Actions;
using Reface.NPI.Parsers.Events;
using Reface.NPI.Parsers.StateMachines;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace Reface.NPI.Parsers
{
    public class DefaultUpdateParser : DefaultParser<UpdateInfo, UpdateStateMachine, UpdateToken, UpdateParseStates, UpdateParseActions>, IUpdateParser
    {

        protected override UpdateToken GetTokenByWord(string word)
        {
            return UpdateToken.Create(word);
        }

        protected override void OnParsing(ref UpdateInfo info, UpdateStateMachine machine, TokenParsingEventArgs<UpdateParseStates> e)
        {
            const string CONTEXT_KEY_CONDITION = "Condition";
            const string CONTEXT_KEY_SET = "Set";
            switch (e.NowState)
            {
                case States.UpdateParseStates.SetField:
                    {
                        string field = machine.TokenStack.Pop().Text;
                        SetInfo setInfo = new SetInfo();
                        setInfo.Field = field;
                        setInfo.Parameter = field;
                        machine.Context[CONTEXT_KEY_SET] = setInfo;
                        info.SetFields.Add(setInfo);
                    }
                    break;
                case States.UpdateParseStates.SetParameter:
                    {
                        SetInfo setInfo = (SetInfo)machine.Context[CONTEXT_KEY_SET];
                        setInfo.Parameter = machine.TokenStack.Pop().Text;
                    }
                    break;
                case UpdateParseStates.Condition:
                    {
                        FieldConditionInfo condition = new FieldConditionInfo();
                        machine.Context[CONTEXT_KEY_CONDITION] = condition;
                        info.Condition = condition;
                    }
                    break;
                case UpdateParseStates.NotCondition:
                    {
                        FieldConditionInfo condition = machine.Context[CONTEXT_KEY_CONDITION] as FieldConditionInfo;
                        condition.IsNot = true;
                    }
                    break;
                case States.UpdateParseStates.ConditionField:
                    {
                        FieldConditionInfo condition = machine.Context[CONTEXT_KEY_CONDITION] as FieldConditionInfo;
                        string field = machine.TokenStack.Pop().Text;
                        condition.Field = field;
                        condition.Parameter = field;
                    }
                    break;
                case States.UpdateParseStates.ConditionOperator:
                    {
                        string opr = machine.TokenStack.Pop().Text;
                        FieldConditionInfo condition = machine.Context[CONTEXT_KEY_CONDITION] as FieldConditionInfo;
                        condition.Operators = opr;
                    }
                    break;
                case States.UpdateParseStates.ConditionParameter:
                    {
                        FieldConditionInfo condition = machine.Context[CONTEXT_KEY_CONDITION] as FieldConditionInfo;
                        condition.Parameter = machine.TokenStack.Pop().Text;
                    }
                    break;
                case States.UpdateParseStates.NextCondition:
                    {
                        var token = machine.TokenStack.Pop();
                        ConditionJoiners joiners = token.Action == Actions.UpdateParseActions.Or
                            ? ConditionJoiners.Or
                            : ConditionJoiners.And;

                        FieldConditionInfo nextCondition = new FieldConditionInfo();
                        info.Condition = new GroupConditionInfo()
                        {
                            LeftCondition = info.Condition,
                            Joiner = ConditionJoiners.And,
                            RightCondition = nextCondition
                        };

                        machine.Context[CONTEXT_KEY_CONDITION] = nextCondition;
                    }
                    break;
                case States.UpdateParseStates.WithoutField:
                    {
                        info.WithoutFields.Add(machine.TokenStack.Pop().Text);
                    }
                    break;
                case States.UpdateParseStates.NextWithoutField:
                case States.UpdateParseStates.SetEquals:
                case States.UpdateParseStates.NextSetField:
                case States.UpdateParseStates.Without:
                    machine.TokenStack.Pop();
                    break;
                default:
                    break;
            }
        }
    }
}
