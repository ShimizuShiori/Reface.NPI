using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reface.NPI.Models;
using Reface.NPI.Parsers.StateMachines;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;

namespace Reface.NPI.Parsers
{
    public class DefaultCountParser : ICountParser
    {
        private CountInfo countInfo;
        public CountInfo Parse(string command)
        {
            countInfo = new CountInfo();
            List<string> words = command.SplitToWords();
            IEnumerable<CountToken> tokens = words.Select(x => CountToken.Create(x));
            CountStateMachine stateMachine = new CountStateMachine();
            stateMachine.Parsing += StateMachine_Parsing;
            foreach (var token in tokens)
            {
                stateMachine.Push(token);
            }
            return countInfo;
        }

        private void StateMachine_Parsing(object sender, Events.TokenParsingEventArgs<CountParseStates> e)
        {
            const string CONTEXT_KEY_CONDITION = "CONDITION";
            CountStateMachine machine = sender as CountStateMachine;
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
                        countInfo.ConditionInfos.Add(conditionInfo);
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
