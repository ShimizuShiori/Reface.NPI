using Reface.NPI.Models;
using Reface.NPI.Parsers.StateMachines;
using Reface.NPI.Parsers.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace Reface.NPI.Parsers
{
    public class DefaultUpdateParser : IUpdateParser
    {
        private UpdateInfo info;

        public UpdateInfo Parse(string command)
        {
            IEnumerable<string> words = command.SplitToWords();
            var tokens = words.Select(x => UpdateToken.Create(x));
            info = new UpdateInfo();
            UpdateStateMachine machine = new UpdateStateMachine();
            machine.Parsing += Machine_Parsing;
            foreach (var token in tokens)
            {
                machine.Push(token);
            }
            return info;
        }

        private void Machine_Parsing(object sender, Events.TokenParsingEventArgs<States.UpdateParseStates> e)
        {
            const string CONTEXT_KEY_CONDITION = "Condition";
            var machine = sender as UpdateStateMachine;
            switch (e.NowState)
            {
                case States.UpdateParseStates.SetField:
                    {
                        string field = machine.TokenStack.Pop().Text;
                        info.SetFields.Add(field);
                    }
                    break;
                case States.UpdateParseStates.ConditionField:
                    {
                        string field = machine.TokenStack.Pop().Text;
                        ConditionInfo condition = new ConditionInfo();
                        condition.Field = field;
                        info.Conditions.Add(condition);
                        machine.Context[CONTEXT_KEY_CONDITION] = condition;
                    }
                    break;
                case States.UpdateParseStates.ConditionOperator:
                    {
                        string opr = machine.TokenStack.Pop().Text;
                        ConditionInfo condition = machine.Context[CONTEXT_KEY_CONDITION] as ConditionInfo;
                        condition.Operators = opr;
                    }
                    break;
                case States.UpdateParseStates.NextCondition:
                    {
                        var token = machine.TokenStack.Pop();
                        ConditionJoiners joiners = token.Action == Actions.UpdateParseActions.Or
                            ? ConditionJoiners.Or
                            : ConditionJoiners.And;
                        var condition = (ConditionInfo)machine.Context[CONTEXT_KEY_CONDITION];
                        condition.JoinerToNext = joiners;
                    }
                    break;
                case States.UpdateParseStates.NextSetField:
                case States.UpdateParseStates.Condition:
                    machine.TokenStack.Pop();
                    break;
                default:
                    break;
            }
        }
    }
}
