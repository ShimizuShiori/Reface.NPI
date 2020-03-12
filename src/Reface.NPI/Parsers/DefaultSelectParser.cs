using Reface.NPI.Models;
using Reface.NPI.Parsers.StateMachines;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reface.NPI.Parsers
{
    public class DefaultSelectParser : ISelectParser
    {
        private SelectInfo selectInfo;

        public SelectInfo Parse(string command)
        {
            selectInfo = new SelectInfo();
            IEnumerable<string> words = command.SplitToWords();
            IEnumerable<SelectToken> tokens = words.Select(x => SelectToken.Create(x));
            var machine = new SelectStateMachine();
            machine.Parsing += Machine_Parsing;
            foreach (var token in tokens)
                machine.Push(token);
            return selectInfo;
        }

        private void Machine_Parsing(object sender, Events.TokenParsingEventArgs<States.SelectParseStates> e)
        {
            const string CONTEXT_KEY_CONDITION = "Condition";
            const string CONTEXT_KEY_ORDER = "Order";
            SelectStateMachine machine = (SelectStateMachine)sender;
            switch (e.NowState)
            {
                case SelectParseStates.OutputField:
                    selectInfo.Fields.Add
                        (
                            machine.TokenStack.Pop().Text
                        );
                    break;
                case SelectParseStates.ConditionField:
                    {
                        ConditionInfo condition = new ConditionInfo();
                        condition.Field = machine.TokenStack.Pop().Text;
                        machine.Context[CONTEXT_KEY_CONDITION] = condition;
                        selectInfo.Conditions.Add(condition);
                    }
                    break;
                case SelectParseStates.ConditionOperator:
                    {
                        ConditionInfo condition = (ConditionInfo)machine.Context[CONTEXT_KEY_CONDITION];
                        condition.Operators = machine.TokenStack.Pop().Text;
                    }
                    break;
                case SelectParseStates.NextCondition:
                    {
                        ConditionInfo condition = (ConditionInfo)machine.Context[CONTEXT_KEY_CONDITION];
                        condition.JoinerToNext = machine.TokenStack.Pop().Action == Actions.SelectParseActions.Or
                            ? ConditionJoiners.Or
                            : ConditionJoiners.And;
                    }
                    break;
                case SelectParseStates.OrderByField:
                    {
                        OrderInfo order = new OrderInfo(machine.TokenStack.Pop().Text);
                        selectInfo.Orders.Add(order);
                        machine.Context[CONTEXT_KEY_ORDER] = order;
                    }
                    break;
                case SelectParseStates.AscOrDesc:
                    {
                        OrderInfo order = (OrderInfo)machine.Context[CONTEXT_KEY_ORDER];
                        order.Type = machine.TokenStack.Pop().Text == SelectToken.TEXT_DESC
                            ? OrderTypes.Desc
                            : OrderTypes.Asc;
                    }
                    break;
                case SelectParseStates.Condition:
                case SelectParseStates.NextOutputField:
                case SelectParseStates.OrderBy:
                    machine.TokenStack.Pop();
                    break;
                default:
                    break;
            }
        }
    }
}
