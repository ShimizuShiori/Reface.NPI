using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reface.NPI.Models;

namespace Reface.NPI.Parsers
{
    public class DefaultSelectParser : ISelectParser
    {
        private SelectInfo selectInfo;
        private Stack<SelectToken> tokenStack;

        public SelectInfo Parse(string command)
        {
            selectInfo = new SelectInfo();
            tokenStack = new Stack<SelectToken>();
            List<SelectToken> tokens = this.SplitCommandToTokens(command);
            var machine = SelectParseStateMachineBuilder.Default.Build();
            machine.Pushed += Machine_Pushed;

            foreach (var token in tokens)
            {
                tokenStack.Push(token);
                machine.Push(token.Action);
            }
            return selectInfo;
        }

        private void Machine_Pushed(object sender, StateMachine.Events.StateMachinePushedEventArgs<SelectParseStates, SelectParseActions> e)
        {
            string field;
            string opr;
            string joiner;
            switch (e.NewState)
            {
                case SelectParseStates.Start:
                    break;
                case SelectParseStates.OutputField:
                    selectInfo.Fields.Add(tokenStack.Pop().Text);
                    break;
                case SelectParseStates.NextOutputField:
                    tokenStack.Pop();
                    break;
                case SelectParseStates.Condition:
                    break;
                case SelectParseStates.ConditionField:
                    break;
                case SelectParseStates.ConditionOperator:
                    opr = tokenStack.Pop().Text;
                    field = tokenStack.Pop().Text;
                    selectInfo.Conditions.Add
                        (
                            new ConditionInfo
                                (
                                    field,
                                    ConditionOperators.Is
                                )
                        );
                    break;
                case SelectParseStates.NextCondition:
                    joiner = tokenStack.Pop().Text;
                    field = tokenStack.Pop().Text;
                    selectInfo.Conditions.Add(new ConditionInfo(field, ConditionOperators.Is, ConditionJoiners.And));
                    break;
                case SelectParseStates.OrderBy:
                    tokenStack.Pop();
                    if (tokenStack.Any() && tokenStack.Peek().Action == SelectParseActions.Field)
                    {
                        selectInfo.Conditions.Add(new ConditionInfo(tokenStack.Pop().Text, ConditionOperators.Is));
                    }
                    break;
                case SelectParseStates.OrderByField:
                    break;
                case SelectParseStates.OrderByAsc:
                    tokenStack.Pop();
                    selectInfo.Orders.Add(new OrderInfo(tokenStack.Pop().Text, OrderTypes.Asc));
                    break;
                case SelectParseStates.OrderByDesc:
                    tokenStack.Pop();
                    selectInfo.Orders.Add(new OrderInfo(tokenStack.Pop().Text, OrderTypes.Asc));
                    break;
                case SelectParseStates.NextOrderBy:
                    tokenStack.Pop();
                    break;
                case SelectParseStates.SkipOutputAndCondition:
                    tokenStack.Pop();
                    break;
                case SelectParseStates.End:
                    break;
                default:
                    break;
            }
        }

        public List<SelectToken> SplitCommandToTokens(string command)
        {
            List<SelectToken> result = new List<SelectToken>();
            StringBuilder sb = new StringBuilder();
            foreach (var c in command)
            {
                if (!Char.IsUpper(c))
                {
                    sb.Append(c);
                    continue;
                }

                if (sb.Length == 0)
                {
                    sb.Append(c);
                    continue;
                }

                result.Add(SelectToken.Create(sb.ToString()));
                sb.Clear();
                sb.Append(c);
            }
            result.Add(SelectToken.Create(sb.ToString()));
            return result;
        }
    }
}
